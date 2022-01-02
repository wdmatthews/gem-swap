using UnityEngine;

namespace GemSwap.Tests
{
    public class GemSOBuilder
    {
        public GemSO Build()
        {
            return ScriptableObject.CreateInstance<GemSO>();
        }

        public static implicit operator GemSO(GemSOBuilder builder)
        {
            return builder.Build();
        }
    }
}
