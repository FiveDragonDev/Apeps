using System;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public sealed class UnityUpdateDispatcher : MonoBehaviour
    {
        public static event Action OnUpdate;

        private static UnityUpdateDispatcher _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            if (_instance == null)
            {
                GameObject go = new(nameof(UnityUpdateDispatcher));
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<UnityUpdateDispatcher>();
            }
        }

        private void Update() => OnUpdate?.Invoke();
        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}
