using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GemSwap.Tests
{
    public class GemTests
    {
        public class FallTo
        {
            [Test]
            public void StartsFalling()
            {
                Gem gem = ADefault.Gem;

                gem.FallTo(new Vector2Int(), new Vector3());

                Assert.IsTrue(gem.IsMoving);
            }

            [Test]
            public void SetsPosition([Values(1, 2)] int x, [Values(1, 2)] int y)
            {
                Vector2Int position = new(x, y);
                Gem gem = ADefault.Gem;

                gem.FallTo(position, new Vector3());

                Assert.AreEqual(position, gem.Position);
            }

            [Test]
            public void WakesRigidbody()
            {
                Gem gem = ADefault.Gem;
                Rigidbody2D rigidbody = gem.GetComponent<Rigidbody2D>();

                gem.FallTo(new Vector2Int(), new Vector3());

                Assert.IsFalse(rigidbody.IsSleeping());
            }

            [Test]
            public void SetsGravityScale([Values(2, 3)] float gravityScale)
            {
                Gem gem = ADefault.Gem
                    .WithGravityScale(gravityScale);
                Rigidbody2D rigidbody = gem.GetComponent<Rigidbody2D>();

                gem.FallTo(new Vector2Int(), new Vector3());

                Assert.AreEqual(gravityScale, rigidbody.gravityScale, float.Epsilon);
            }
        }

        public class SwapTo
        {
            [Test]
            public void StartsSwapping()
            {
                Gem gem = ADefault.Gem;

                gem.SwapTo(new Vector2Int(), new Vector3());

                Assert.IsTrue(gem.IsMoving);
            }

            [Test]
            public void SetsPosition([Values(1, 2)] int x, [Values(1, 2)] int y)
            {
                Vector2Int position = new(x, y);
                Gem gem = ADefault.Gem;

                gem.SwapTo(position, new Vector3());

                Assert.AreEqual(position, gem.Position);
            }

            [Test]
            public void WakesRigidbody()
            {
                Gem gem = ADefault.Gem;
                Rigidbody2D rigidbody = gem.GetComponent<Rigidbody2D>();

                gem.SwapTo(new Vector2Int(), new Vector3());

                Assert.IsFalse(rigidbody.IsSleeping());
            }

            [Test]
            public void SwapLeft_SetsVelocityLeft([Values(1, 2)] float swapSpeed)
            {
                Gem gem = ADefault.Gem
                    .WithSwapSpeed(swapSpeed)
                    .WithGridPosition(new Vector2Int(1, 1));
                Rigidbody2D rigidbody = gem.GetComponent<Rigidbody2D>();

                gem.SwapTo(new Vector2Int(0, 1), new Vector3());

                Assert.AreEqual(-swapSpeed, rigidbody.velocity.x, float.Epsilon);
            }

            [Test]
            public void SwapRight_SetsVelocityRight([Values(1, 2)] float swapSpeed)
            {
                Gem gem = ADefault.Gem
                    .WithSwapSpeed(swapSpeed)
                    .WithGridPosition(new Vector2Int(1, 1));
                Rigidbody2D rigidbody = gem.GetComponent<Rigidbody2D>();

                gem.SwapTo(new Vector2Int(2, 1), new Vector3());

                Assert.AreEqual(swapSpeed, rigidbody.velocity.x, float.Epsilon);
            }

            [Test]
            public void SwapDown_SetsVelocityDown([Values(1, 2)] float swapSpeed)
            {
                Gem gem = ADefault.Gem
                    .WithSwapSpeed(swapSpeed)
                    .WithGridPosition(new Vector2Int(1, 1));
                Rigidbody2D rigidbody = gem.GetComponent<Rigidbody2D>();

                gem.SwapTo(new Vector2Int(1, 0), new Vector3());

                Assert.AreEqual(-swapSpeed, rigidbody.velocity.y, float.Epsilon);
            }

            [Test]
            public void SwapUp_SetsVelocityUp([Values(1, 2)] float swapSpeed)
            {
                Gem gem = ADefault.Gem
                    .WithSwapSpeed(swapSpeed)
                    .WithGridPosition(new Vector2Int(1, 1));
                Rigidbody2D rigidbody = gem.GetComponent<Rigidbody2D>();

                gem.SwapTo(new Vector2Int(1, 2), new Vector3());

                Assert.AreEqual(swapSpeed, rigidbody.velocity.y, float.Epsilon);
            }
        }

        public class FixedUpdate
        {
            [UnityTest]
            public IEnumerator IsFallingAndAtTargetPosition_StopsFalling()
            {
                Gem gem = ADefault.Gem;
                gem.FallTo(new Vector2Int(), new Vector3());

                yield return new WaitForFixedUpdate();

                Assert.IsFalse(gem.IsMoving);
            }

            [UnityTest]
            public IEnumerator IsSwappingAndAtTargetPosition_StopsSwapping()
            {
                Gem gem = ADefault.Gem
                    .WithGridPosition(new Vector2Int(0, 0));
                gem.SwapTo(new Vector2Int(1, 0), new Vector3());

                yield return new WaitForFixedUpdate();

                Assert.IsFalse(gem.IsMoving);
            }
        }

        public class Remove
        {
            [Test]
            public void DeactivatesGameObject()
            {
                Gem gem = ADefault.Gem;

                gem.Remove();

                Assert.IsFalse(gem.gameObject.activeSelf);
            }
        }
    }
}
