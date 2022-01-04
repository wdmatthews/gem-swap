using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GemSwap.Tests
{
    public class GemGridTests
    {
        public class GridToWorldPosition
        {
            [Test]
            public void ReturnsZeroPlusPosition([Values(0, 0.5f)] float zeroX, [Values(0, 0.5f)] float zeroY, [Values(0, 1)] int x, [Values(0, 1)] int y)
            {
                Vector2 zeroWorldPosition = new(zeroX, zeroY);
                Vector2Int position = new(x, y);
                GemGrid gemGrid = ADefault.GemGrid
                    .WithZeroWorldPosition(zeroWorldPosition);

                Vector2 worldPosition = gemGrid.GridToWorldPosition(position);

                Assert.AreEqual(zeroX + x, worldPosition.x, float.Epsilon, "X");
                Assert.AreEqual(zeroY + y, worldPosition.y, float.Epsilon, "Y");
            }
        }

        public class PlaceGem
        {
            [Test]
            public void PlacesInCorrectPosition([Values(0, 1)] int x, [Values(0, 1)] int y)
            {
                Vector2Int position = new(x, y);
                Vector2Int gridSize = new(2, 2);
                GemGrid gemGrid = ADefault.GemGrid
                    .WithSize(gridSize);
                Gem gem = ADefault.Gem;

                gemGrid.PlaceGem(gem, position);

                Assert.AreEqual(gem, gemGrid.Gems[x + y * gridSize.x]);
            }
        }

        public class GetGem
        {
            [Test]
            public void ReturnsCorrectGem([Values(0, 1)] int x, [Values(0, 1)] int y)
            {
                Vector2Int position = new(x, y);
                Vector2Int gridSize = new(2, 2);
                GemGrid gemGrid = ADefault.GemGrid
                    .WithSize(gridSize);
                Gem gem = ADefault.Gem;
                gemGrid.PlaceGem(gem, position);

                Gem gemInGrid = gemGrid.GetGem(position);

                Assert.AreEqual(gem, gemInGrid);
            }
        }

        public class IsInLine
        {
            [Test]
            public void HorizontalLine_ReturnsTrue([Values(1, 2, 3)] int x)
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position = new(x - 1, 0);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(x, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(x + 1, 0));

                bool isInLine = gemGrid.IsInLine(position);

                Assert.IsTrue(isInLine);
            }

            [Test]
            public void VerticalLine_ReturnsTrue([Values(1, 2, 3)] int y)
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position = new(0, y - 1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(0, y));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(0, y + 1));

                bool isInLine = gemGrid.IsInLine(position);

                Assert.IsTrue(isInLine);
            }

            [Test]
            public void HorizontalLine_ReturnsFalse([Values(1, 2, 3)] int x)
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position = new(x - 1, 0);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(x, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), new Vector2Int(x + 1, 0));

                bool isInLine = gemGrid.IsInLine(position);

                Assert.IsFalse(isInLine);
            }

            [Test]
            public void VerticalLine_ReturnsFalse([Values(1, 2, 3)] int y)
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position = new(0, y - 1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(0, y));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), new Vector2Int(0, y + 1));

                bool isInLine = gemGrid.IsInLine(position);

                Assert.IsFalse(isInLine);
            }
        }

        public class SwapGems
        {
            [Test]
            public void SwapsGems()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position1 = new(0, 1);
                Vector2Int position2 = new(0, 0);
                Gem gem1 = ADefault.Gem.WithData(gemData);
                Gem gem2 = ADefault.Gem.WithData(A.GemSO);
                gemGrid.PlaceGem(gem1, position1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(2, 0));
                gemGrid.PlaceGem(gem2, position2);

                gemGrid.SwapGems(position1, position2);

                Assert.AreEqual(gem2, gemGrid.GetGem(position1), "Position 1");
                Assert.AreEqual(gem1, gemGrid.GetGem(position2), "Position 2");
            }
        }

        public class CanSwapGems
        {
            [Test]
            public void WillCreateMatch_ReturnsTrue()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position1 = new(0, 1);
                Vector2Int position2 = new(0, 0);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(2, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), position2);

                bool canSwapGems = gemGrid.CanSwapGems(position1, position2);

                Assert.IsTrue(canSwapGems);
            }

            [Test]
            public void TooFewSameGems_ReturnsFalse()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position1 = new(0, 1);
                Vector2Int position2 = new(0, 0);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), new Vector2Int(2, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), position2);

                bool canSwapGems = gemGrid.CanSwapGems(position1, position2);

                Assert.IsFalse(canSwapGems);
            }

            [Test]
            public void GemsTooFarApart_ReturnsFalse()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position1 = new(0, 1);
                Vector2Int position2 = new(2, 0);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position2);
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), new Vector2Int(0, 0));

                bool canSwapGems = gemGrid.CanSwapGems(position1, position2);

                Assert.IsFalse(canSwapGems);
            }

            [Test]
            public void GemsDiagonal_ReturnsFalse()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position1 = new(0, 0);
                Vector2Int position2 = new(1, 1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), position1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(0, 1));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position2);

                bool canSwapGems = gemGrid.CanSwapGems(position1, position2);

                Assert.IsFalse(canSwapGems);
            }

            [Test]
            public void PositionOutsideOfGrid_ReturnsFalse([Values(-1, 5)] int x, [Values(-1, 5)] int y)
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position1 = new(x, y);
                Vector2Int position2 = new(0, 0);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(0, 1));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(2, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), position2);

                bool canSwapGems = gemGrid.CanSwapGems(position1, position2);

                Assert.IsFalse(canSwapGems);
            }
        }

        public class GemCanFall
        {
            [Test]
            public void NothingBelow_ReturnsTrue([Values(1, 2)] int y)
            {
                Vector2Int position = new(0, y);
                GemGrid gemGrid = ADefault.GemGrid;
                gemGrid.PlaceGem(ADefault.Gem, position);

                (bool gemCanFall, _) = gemGrid.GemCanFall(position);

                Assert.IsTrue(gemCanFall);
            }

            [Test]
            public void AtBottom_ReturnsFalse()
            {
                Vector2Int position = new(0, 0);
                GemGrid gemGrid = ADefault.GemGrid;
                gemGrid.PlaceGem(ADefault.Gem, position);

                (bool gemCanFall, _) = gemGrid.GemCanFall(position);

                Assert.IsFalse(gemCanFall);
            }

            [Test]
            public void GemBelow_ReturnsFalse()
            {
                Vector2Int position = new(0, 1);
                GemGrid gemGrid = ADefault.GemGrid;
                gemGrid.PlaceGem(ADefault.Gem, position);
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(0, 0));

                (bool gemCanFall, _) = gemGrid.GemCanFall(position);

                Assert.IsFalse(gemCanFall);
            }

            [Test]
            public void NothingBelow_ReturnsBottom([Values(1, 2)] int y)
            {
                Vector2Int position = new(0, y);
                GemGrid gemGrid = ADefault.GemGrid;
                gemGrid.PlaceGem(ADefault.Gem, position);

                (_, Vector2Int positionAfterFall) = gemGrid.GemCanFall(position);

                Assert.AreEqual(new Vector2Int(0, 0), positionAfterFall);
            }

            [Test]
            public void GemFarBelow_ReturnsPositionAboveOtherGem([Values(2, 3)] int y)
            {
                Vector2Int position = new(0, y);
                GemGrid gemGrid = ADefault.GemGrid;
                gemGrid.PlaceGem(ADefault.Gem, position);
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(0, 0));

                (_, Vector2Int positionAfterFall) = gemGrid.GemCanFall(position);

                Assert.AreEqual(new Vector2Int(0, 1), positionAfterFall);
            }
        }

        public class MakeGemFallTo
        {
            [Test]
            public void PlacesInNewPosition([Values(1, 2)] int y)
            {
                Vector2Int position = new(0, y);
                Vector2Int positionAfterFall = new(0, 0);
                GemGrid gemGrid = ADefault.GemGrid;
                Gem gem = ADefault.Gem;
                gemGrid.PlaceGem(gem, position);

                gemGrid.MakeGemFallTo(position, positionAfterFall);

                Assert.AreEqual(gem, gemGrid.GetGem(positionAfterFall));
            }

            [Test]
            public void RemovesFromOldPosition([Values(1, 2)] int y)
            {
                Vector2Int position = new(0, y);
                GemGrid gemGrid = ADefault.GemGrid;
                gemGrid.PlaceGem(ADefault.Gem, position);

                gemGrid.MakeGemFallTo(position, new Vector2Int(0, 0));

                Assert.IsNull(gemGrid.GetGem(position));
            }
        }

        public class RemoveGem
        {
            [Test]
            public void RemovesGem([Values(0, 1)] int x, [Values(0, 1)] int y)
            {
                Vector2Int position = new(x, y);
                GemGrid gemGrid = ADefault.GemGrid
                    .WithSize(new Vector2Int(2, 2));
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(0, 0));
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(0, 1));
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(1, 1));

                gemGrid.RemoveGem(position);

                Assert.IsNull(gemGrid.GetGem(position));
            }
        }

        public class RemoveAllGems
        {
            [Test]
            public void RemovesAllGems()
            {
                GemGrid gemGrid = ADefault.GemGrid
                    .WithSize(new Vector2Int(2, 2));
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(0, 0));
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(0, 1));
                gemGrid.PlaceGem(ADefault.Gem, new Vector2Int(1, 1));

                gemGrid.RemoveAllGems();

                Assert.IsNull(gemGrid.Gems[0], "Gem 1");
                Assert.IsNull(gemGrid.Gems[1], "Gem 2");
                Assert.IsNull(gemGrid.Gems[2], "Gem 3");
                Assert.IsNull(gemGrid.Gems[3], "Gem 4");
            }
        }

        public class AnyPossibleMatches
        {
            [Test]
            public void WillCreateMatch_ReturnsTrue()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position1 = new(0, 1);
                Vector2Int position2 = new(0, 0);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(2, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), position2);

                bool anyPossibleMatches = gemGrid.AnyPossibleMatches();

                Assert.IsTrue(anyPossibleMatches);
            }

            [Test]
            public void WillNotCreateMatch_ReturnsFalse()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                Vector2Int position1 = new(0, 1);
                Vector2Int position2 = new(0, 0);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), position1);
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), new Vector2Int(2, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), position2);

                bool anyPossibleMatches = gemGrid.AnyPossibleMatches();

                Assert.IsFalse(anyPossibleMatches);
            }
        }

        public class AnyMatches
        {
            [Test]
            public void MatchesExist_ReturnsTrue()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(0, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(2, 0));

                bool anyMatches = gemGrid.AnyMatches();

                Assert.IsTrue(anyMatches);
            }

            [Test]
            public void MatchesExist_ReturnsFalse()
            {
                GemGrid gemGrid = ADefault.GemGrid;
                GemSO gemData = A.GemSO;
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(0, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(gemData), new Vector2Int(1, 0));
                gemGrid.PlaceGem(ADefault.Gem.WithData(A.GemSO), new Vector2Int(2, 0));

                bool anyMatches = gemGrid.AnyMatches();

                Assert.IsFalse(anyMatches);
            }
        }
    }
}
