using UnityEditor;
using UnityEngine;

namespace GemSwap.Tests
{
    public class GemBuilder
    {
        private float _gravityScale;
        private float _swapSpeed;
        private GemSO _data;
        private Vector2Int _gridPosition;
        private Vector3 _worldPosition;

        public GemBuilder WithGravityScale(float gravityScale)
        {
            _gravityScale = gravityScale;
            return this;
        }

        public GemBuilder WithSwapSpeed(float swapSpeed)
        {
            _swapSpeed = swapSpeed;
            return this;
        }

        public GemBuilder WithData(GemSO data)
        {
            _data = data;
            return this;
        }

        public GemBuilder WithGridPosition(Vector2Int gridPosition)
        {
            _gridPosition = gridPosition;
            return this;
        }

        public GemBuilder WithWorldPosition(Vector3 worldPosition)
        {
            _worldPosition = worldPosition;
            return this;
        }

        public Gem Build()
        {
            GameObject gameObject = new GameObject();
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.Sleep();
            Gem gem = gameObject.AddComponent<Gem>();

            SerializedObject so = new SerializedObject(gem);
            so.FindProperty("_minGravityScale")
                .floatValue = _gravityScale;
            so.FindProperty("_maxGravityScale")
                .floatValue = _gravityScale;
            so.FindProperty("_swapSpeed")
                .floatValue = _swapSpeed;
            so.FindProperty("_meshFilter")
                .objectReferenceValue = gameObject.AddComponent<MeshFilter>();
            so.FindProperty("_renderer")
                .objectReferenceValue = gameObject.AddComponent<MeshRenderer>();
            so.FindProperty("_rigidbody")
                .objectReferenceValue = rigidbody;
            so.ApplyModifiedProperties();

            gem.Place(_data, _gridPosition, _worldPosition);

            return gem;
        }

        public static implicit operator Gem(GemBuilder builder)
        {
            return builder.Build();
        }
    }
}
