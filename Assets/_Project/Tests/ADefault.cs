using UnityEngine;

namespace GemSwap.Tests
{
    public static class ADefault
    {
        public static GemBuilder Gem => A.Gem
            .WithGravityScale(1)
            .WithSwapSpeed(1)
            .WithData(A.GemSO)
            .WithGridPosition(new Vector2Int())
            .WithWorldPosition(new Vector3());
        public static GemGridBuilder GemGrid => A.GemGrid
            .WithZeroWorldPosition(new Vector3())
            .WithSize(new Vector2Int(5, 5));
    }
}
