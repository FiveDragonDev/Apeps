using Assets.Scripts.Weapons.Base;
using UnityEngine;

namespace Assets.Scripts.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/Gun/Gun Definition", order = 0)]
    public sealed class GunDefinition : ScriptableObject
    {
        [field: SerializeField] public FireType FireType { get; private set; }

        [field: SerializeField, Min(0.01f)] public float FireRate { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float FireRange { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float KineticEnergy { get; private set; } = 1; // J, test-only!!! (i hope)
    }
}
