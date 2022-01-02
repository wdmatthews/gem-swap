using UnityEngine;

namespace GemSwap
{
    [CreateAssetMenu(fileName = "New Gem", menuName = "Gem Swap/Gem")]
    public class GemSO : ScriptableObject
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;

        public Mesh Mesh => _mesh;
        public Material Material => _material;
    }
}
