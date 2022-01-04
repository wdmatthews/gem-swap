using UnityEngine;

namespace GemSwap
{
    public class GemGrid
    {
        private const int _minLineLength = 3;

        private Vector2 _zeroWorldPosition;
        private Vector2Int _size;
        private Gem[] _gems;

        public Gem[] Gems => _gems;

        public GemGrid(Vector2 zeroWorldPosition, Vector2Int size)
        {
            _zeroWorldPosition = zeroWorldPosition;
            _size = size;
            _gems = new Gem[_size.x * _size.y];
        }

        public Vector2 GridToWorldPosition(Vector2Int position)
        {
            return new Vector2(_zeroWorldPosition.x + position.x, _zeroWorldPosition.y + position.y);
        }

        public void PlaceGem(Gem gem, Vector2Int position)
        {
            _gems[GetIndexFromPosition(position)] = gem;
        }

        public Gem GetGem(Vector2Int position)
        {
            return _gems[GetIndexFromPosition(position)];
        }

        public bool IsInLine(Vector2Int position)
        {
            Gem gem = GetGem(position);
            if (!gem) return false;
            GemSO gemData = gem.Data;
            return IsInLine(gemData, position, new Vector2Int(1, 0))
                || IsInLine(gemData, position, new Vector2Int(0, 1));
        }

        public void SwapGems(Vector2Int position1, Vector2Int position2)
        {
            Gem gem1 = GetGem(position1);
            Gem gem2 = GetGem(position2);
            _gems[GetIndexFromPosition(position1)] = gem2;
            _gems[GetIndexFromPosition(position2)] = gem1;
        }

        public bool CanSwapGems(Vector2Int position1, Vector2Int position2)
        {
            if (position1.x < 0 || position1.x >= _size.x
                || position1.y < 0 || position1.y >= _size.y
                || position2.x < 0 || position2.x >= _size.x
                || position2.y < 0 || position2.y >= _size.y
                || position1.x != position2.x && position1.y != position2.y
                || Mathf.Abs(position1.x - position2.x) > 1
                || Mathf.Abs(position1.y - position2.y) > 1) return false;

            SwapGems(position1, position2);
            bool isInLine = IsInLine(position1) || IsInLine(position2);
            SwapGems(position1, position2);

            return isInLine;
        }

        public (bool, Vector2Int) GemCanFall(Vector2Int position)
        {
            Vector2Int positionAfterFall = new(position.x, position.y - 1);
            if (position.y == 0 || GetGem(positionAfterFall)) return (false, new Vector2Int());

            while (positionAfterFall.y > 0
                && !GetGem(new Vector2Int(positionAfterFall.x, positionAfterFall.y - 1)))
            {
                positionAfterFall.y--;
            }

            return (true, positionAfterFall);
        }

        public void MakeGemFallTo(Vector2Int position, Vector2Int positionAfterFall)
        {
            Gem gem = GetGem(position);
            _gems[GetIndexFromPosition(position)] = null;
            _gems[GetIndexFromPosition(positionAfterFall)] = gem;
        }

        public void RemoveGem(Vector2Int position)
        {
            _gems[GetIndexFromPosition(position)] = null;
        }

        public void RemoveAllGems()
        {
            _gems = new Gem[_size.x * _size.y];
        }

        public bool AnyPossibleMatches()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    Vector2Int position = new(x, y);

                    if (CanSwapGems(position, new Vector2Int(position.x - 1, position.y))
                        || CanSwapGems(position, new Vector2Int(position.x + 1, position.y))
                        || CanSwapGems(position, new Vector2Int(position.x, position.y - 1))
                        || CanSwapGems(position, new Vector2Int(position.x, position.y + 1))) return true;
                }
            }

            return false;
        }

        public bool AnyMatches()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    if (IsInLine(new Vector2Int(x, y))) return true;
                }
            }

            return false;
        }

        private int GetIndexFromPosition(Vector2Int position)
        {
            return position.x + position.y * _size.x;
        }

        private int GetLineLength(GemSO gemData, Vector2Int position, Vector2Int direction, int lineLength)
        {
            Vector2Int loopPosition = position + direction;

            while (lineLength < _minLineLength
                && loopPosition.x >= 0 && loopPosition.x < _size.x
                && loopPosition.y >= 0 && loopPosition.y < _size.y)
            {
                Gem loopGem = GetGem(loopPosition);

                if (loopGem && loopGem.Data == gemData)
                {
                    lineLength++;
                }
                else break;

                loopPosition += direction;
            }

            return lineLength;
        }

        private bool IsInLine(GemSO gemData, Vector2Int position, Vector2Int direction)
        {
            int lineLength = 1;
            lineLength = GetLineLength(gemData, position, direction, lineLength);
            if (lineLength >= _minLineLength) return true;
            lineLength = GetLineLength(gemData, position, -direction, lineLength);
            return lineLength >= _minLineLength;
        }
    }
}
