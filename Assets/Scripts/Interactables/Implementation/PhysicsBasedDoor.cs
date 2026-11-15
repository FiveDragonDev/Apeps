using Assets.Scripts.Interactables.Base;
using System;
using UnityEngine;

namespace Assets.Scripts.Interactables.Implementation
{
    public sealed class PhysicsBasedDoor : Toggle
    {
        [SerializeField] private HingeJoint _joint;

        private void OnEnable() => Toggled += DoorAction;
        private void OnDisable() => Toggled -= DoorAction;

        private void DoorAction(bool open)
        {
            if (_joint is null) return;

            var spring = _joint.spring;
            spring.targetPosition = open ? _joint.limits.max : _joint.limits.min;
            _joint.spring = spring;
        }
    }
}
