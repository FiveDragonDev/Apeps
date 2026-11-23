using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/Health/Tissue Layers Definition", order = 0)]
    public sealed class TissueLayersDefinition : ScriptableObject
    {
        public IReadOnlyList<TissueLayerDefinition> Layers => _layers;

        [SerializeField] private TissueLayerDefinition[] _layers;
    }
}
