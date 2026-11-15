using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tests
{
    [DefaultExecutionOrder(-100)]
    public sealed class MovingPlatform : MonoBehaviour
    {
        private struct Sample
        {
            public Vector3 Position;
            public float Distance;
        }

        [Header("Path")]
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private bool _loop = true;

        [SerializeField, Min(0)] private float _cornerRadius = 1;
        [SerializeField, Min(1)] private int _cornerSubdivisions = 1;

        [Header("Motion")]
        [SerializeField, Min(0)] private float _maxSpeed = 1;
        [SerializeField, Min(0)] private float _acceleration = 1;

        private List<Sample> _path;

        private float _speed;

        private float _distance;
        private float _totalLength;

        private bool _effectiveLoop;
        private float _directionSign = 1;

        private void Awake()
        {
            _effectiveLoop = _loop && _waypoints != null && _waypoints.Length >= 3;

            _path = BuildPathList(_waypoints, _effectiveLoop, _cornerRadius, _cornerSubdivisions);
            _totalLength = _path.Count > 0 ? _path[^1].Distance : 0f;

            _distance = FindNearestDistance(transform.position);
            if (_path.Count > 0) transform.position = SamplePosition(_distance);
        }

        private void FixedUpdate()
        {
            if (_path == null || _path.Count < 2 || _totalLength <= 0) return;
            var dt = Time.fixedDeltaTime;

            if (_effectiveLoop)
            {
                _speed = Mathf.MoveTowards(_speed, _maxSpeed, _acceleration * dt);
                _distance = Mathf.Repeat(_distance + _speed * dt, _totalLength);
            }
            else
            {
                var remaining = _directionSign > 0 ? _totalLength - _distance : _distance;
                var brakeSpeed = Mathf.Sqrt(2 * _acceleration * remaining);
                var targetSpeed = Mathf.Min(_maxSpeed, brakeSpeed);

                _speed = Mathf.MoveTowards(_speed, targetSpeed, _acceleration * dt);
                _distance += _speed * _directionSign * dt;

                if (_distance >= _totalLength)
                {
                    _distance = _totalLength;
                    _speed = 0;
                    _directionSign = -1;
                }
                else if (_distance <= 0)
                {
                    _distance = 0;
                    _speed = 0;
                    _directionSign = 1;
                }
            }

            transform.position = SamplePosition(_distance);
        }

        private Vector3 SamplePosition(float dist)
        {
            if (_totalLength <= 0 || _path.Count == 0) return transform.position;

            dist = _loop ? Mathf.Repeat(dist, _totalLength) : Mathf.Clamp(dist, 0, _totalLength);

            int lo = 0, hi = _path.Count - 1;
            while (hi - lo > 1)
            {
                int mid = (lo + hi) >> 1;

                if (_path[mid].Distance <= dist) lo = mid;
                else hi = mid;
            }

            Sample a = _path[lo], b = _path[hi];

            float segLen = b.Distance - a.Distance;
            if (segLen < 1e-6f) return b.Position;

            return Vector3.LerpUnclamped(a.Position, b.Position, (dist - a.Distance) / segLen);
        }

        private float FindNearestDistance(Vector3 p)
        {
            if (_path == null || _path.Count < 2) return 0;

            float bestSq = float.MaxValue, bestD = 0;

            for (int i = 1; i < _path.Count; i++)
            {
                Vector3 a = _path[i - 1].Position, b = _path[i].Position, ab = b - a;

                float lenSq = ab.sqrMagnitude;
                float t = lenSq > 1e-8f ? Mathf.Clamp01(Vector3.Dot(p - a, ab) / lenSq) : 0;
                float dsq = (p - (a + ab * t)).sqrMagnitude;

                if (dsq < bestSq)
                {
                    bestSq = dsq;
                    bestD = _path[i - 1].Distance + Mathf.Sqrt(lenSq) * t;
                }
            }

            return bestD;
        }

        private static List<Sample> BuildPathList(Transform[] waypoints, bool loop, float cornerRadius, int subdivisions)
        {
            List<Sample> path = new();
            if (waypoints == null || waypoints.Length < 2) return path;

            var n = waypoints.Length;
            var corners = new Vector3[n];
            for (int i = 0; i < n; i++) corners[i] = waypoints[i].position;

            var radii = new float[n];
            for (int i = 0; i < n; i++)
            {
                if (!loop && (i == 0 || i == n - 1))
                {
                    radii[i] = 0;
                    continue;
                }

                int prev = (i - 1 + n) % n, next = (i + 1) % n;

                float lenIn = Vector3.Distance(corners[prev], corners[i]);
                float lenOut = Vector3.Distance(corners[i], corners[next]);

                if (lenIn < 1e-5f || lenOut < 1e-5f)
                {
                    radii[i] = 0;
                    continue;
                }

                Vector3 dirIn = (corners[i] - corners[prev]) / lenIn;
                Vector3 dirOut = (corners[next] - corners[i]) / lenOut;

                if (Vector3.Dot(dirIn, dirOut) < -0.7f)
                {
                    radii[i] = 0;
                    continue;
                }

                radii[i] = Mathf.Min(cornerRadius, Mathf.Min(lenIn, lenOut) * 0.49f);
            }

            var cStart = new Vector3[n];
            var cEnd = new Vector3[n];

            for (int i = 0; i < n; i++)
            {
                int prev = (i - 1 + n) % n, next = (i + 1) % n;

                Vector3 dirIn = (corners[i] - corners[prev]).normalized;
                Vector3 dirOut = (corners[next] - corners[i]).normalized;

                cStart[i] = corners[i] - dirIn * radii[i];
                cEnd[i] = corners[i] + dirOut * radii[i];
            }

            if (loop)
            {
                path.Add(new Sample { Position = cStart[0], Distance = 0f });

                for (int i = 0; i < n; i++)
                {
                    if (radii[i] > 0f) AppendQuadBezier(path, cStart[i], corners[i], cEnd[i], subdivisions);
                    else AppendLine(path, corners[i]);

                    AppendLine(path, cStart[(i + 1) % n]);
                }
            }
            else
            {
                path.Add(new Sample { Position = corners[0], Distance = 0f });

                for (int i = 1; i < n - 1; i++)
                {
                    AppendLine(path, cStart[i]);

                    if (radii[i] > 0f) AppendQuadBezier(path, cStart[i], corners[i], cEnd[i], subdivisions);
                    else AppendLine(path, corners[i]);
                }

                AppendLine(path, corners[n - 1]);
            }
            return path;
        }

        private static void AppendLine(List<Sample> path, Vector3 p)
        {
            Sample last = path[^1];
            path.Add(new Sample { Position = p, Distance = last.Distance + Vector3.Distance(last.Position, p) });
        }

        private static void AppendQuadBezier(List<Sample> path, Vector3 p0, Vector3 p1, Vector3 p2, int subdivisions)
        {
            Sample last = path[^1];

            float prevDist = last.Distance;
            Vector3 prevPos = last.Position;

            for (int s = 1; s <= subdivisions; s++)
            {
                float t = s / (float)subdivisions;
                float it = 1 - t;

                Vector3 p = (it * it * p0) + (2f * it * t * p1) + (t * t * p2);

                prevDist += Vector3.Distance(prevPos, p);
                prevPos = p;

                path.Add(new Sample { Position = p, Distance = prevDist });
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_waypoints == null || _waypoints.Length < 2) return;

            Gizmos.color = Color.cyan;
            var viz = BuildPathList(_waypoints, _loop, _cornerRadius, _cornerSubdivisions);
            for (int i = 1; i < viz.Count; i++) Gizmos.DrawLine(viz[i - 1].Position, viz[i].Position);

            Gizmos.color = Color.yellow;
            foreach (var wp in _waypoints) if (wp != null) Gizmos.DrawWireSphere(wp.position, 0.1f);
        }
    }
}
