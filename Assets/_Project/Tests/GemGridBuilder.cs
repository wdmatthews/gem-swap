using UnityEngine;

namespace GemSwap.Tests
{
    public class GemGridBuilder
    {
        private Vector2 _zeroWorldPosition;
        private Vector2Int _size;

        public GemGridBuilder WithZeroWorldPosition(Vector2 zeroWorldPosition)
        {
            _zeroWorldPosition = zeroWorldPosition;
            return this;
        }

        public GemGridBuilder WithSize(Vector2Int size)
        {
            _size = size;
            return this;
        }

        public GemGrid Build()
        {
            return new GemGrid(_zeroWorldPosition, _size);
        }

        public static implicit operator GemGrid(GemGridBuilder builder)
        {
            return builder.Build();
        }
    }
}
