using System.Collections.Generic;
using UnityEngine;

namespace GemSwap
{
    [CreateAssetMenu(fileName = "Gem Manager", menuName = "Gem Swap/Gem Manager")]
    public class GemManagerSO : ScriptableObject
    {
        [SerializeField] private Vector2 _gridZeroWorldPosition;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Gem _gemPrefab;
        [SerializeField] private GemSO[] _gemData;


        private GemGrid _grid;
        private List<Gem> _activeGems;
        private Stack<Gem> _inactiveGems;
        private int _fallingGemCount;
        private bool _shouldRemoveGems;
        private System.Action<int> _onGemsRemoved;
        private System.Action _endGame;

        public Vector2Int GridSize => _gridSize;
        public GemGrid Grid => _grid;

        public void Initialize(System.Action<int> onGemsRemoved, System.Action endGame, bool keepInactiveGems = false)
        {
            _onGemsRemoved = onGemsRemoved;
            _endGame = endGame;
            _grid = new GemGrid(_gridZeroWorldPosition, _gridSize);
            _activeGems = new List<Gem>();

            if (!keepInactiveGems)
            {
                _inactiveGems = new Stack<Gem>();
            }

            MakeNewGemsFall();

            while (_grid.AnyMatches() || !_grid.AnyPossibleMatches())
            {
                RemoveAllGems();
                MakeNewGemsFall();
            }
        }

        public void OnUpdate()
        {
            if (_shouldRemoveGems && !_grid.AnyPossibleMatches())
            {
                _endGame?.Invoke();
            }
            else if (_shouldRemoveGems && !AnyGemsMoving())
            {
                RemoveMatchedGems();
            }
        }

        public void SwapGems(Vector2Int position1, Vector2Int position2)
        {
            if (_grid.CanSwapGems(position1, position2))
            {
                Gem gem1 = _grid.GetGem(position1);
                Gem gem2 = _grid.GetGem(position2);
                gem1.SwapTo(position2, _grid.GridToWorldPosition(position2));
                gem2.SwapTo(position1, _grid.GridToWorldPosition(position1));
                _grid.SwapGems(position1, position2);
                _shouldRemoveGems = true;


            }
            else
            {

            }
        }

        public bool AnyGemsMoving()
        {
            foreach (Gem gem in _activeGems)
            {
                if (gem.IsMoving) return true;
            }

            return false;
        }

        public void RemoveAllGems()
        {
            foreach (Gem gem in _activeGems)
            {
                _inactiveGems.Push(gem);
            }

            _activeGems.Clear();
            _fallingGemCount = 0;
            _shouldRemoveGems = false;
            _grid.RemoveAllGems();
        }

        private Gem CreateGem()
        {
            return Instantiate(_gemPrefab);
        }

        private Gem ActivateGem()
        {
            return _inactiveGems.Pop();
        }

        private Gem PlaceGem(Vector2Int gridPosition, Vector3 worldPosition)
        {
            Gem gem;

            if (_inactiveGems.Count > 0)
            {
                gem = ActivateGem();
            }
            else
            {
                gem = CreateGem();
            }

            gem.Place(_gemData[Random.Range(0, _gemData.Length)], gridPosition, worldPosition);
            _grid.PlaceGem(gem, gridPosition);
            _activeGems.Add(gem);
            return gem;
        }

        private void RemoveGem(Vector2Int position)
        {
            Gem gem = _grid.GetGem(position);
            _inactiveGems.Push(gem);
            gem.Remove();
            _grid.RemoveGem(position);
        }

        private void RemoveMatchedGems()
        {
            _shouldRemoveGems = false;
            int removedGemCount = 0;

            for (int i = _activeGems.Count - 1; i >= 0; i--)
            {
                Vector2Int position = _activeGems[i].Position;
                if (!_grid.IsInLine(position)) continue;
                removedGemCount++;
                RemoveGem(position);
            }

            if (removedGemCount > 0)
            {
                MakeGemsFall();
                _onGemsRemoved?.Invoke(removedGemCount);


            }
        }

        private void OnGemStoppedFalling()
        {
            _fallingGemCount--;

            if (_fallingGemCount == 0)
            {
                _shouldRemoveGems = true;
            }
        }

        private void MakeGemsFall()
        {
            _fallingGemCount = 0;

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Vector2Int position = new(x, y);
                    (bool gemCanFall, Vector2Int positionAfterFall) = _grid.GemCanFall(position);
                    if (!gemCanFall) continue;
                    _fallingGemCount++;
                    _grid.GetGem(position).FallTo(positionAfterFall,
                        _grid.GridToWorldPosition(positionAfterFall),
                        OnGemStoppedFalling);
                }
            }
        }

        private void MakeNewGemsFall()
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = _gridSize.y - 1; y >= 0; y--)
                {
                    Vector2Int position = new(x, y);
                    if (_grid.GetGem(position)) continue;
                    (bool gemCanFall, Vector2Int positionAfterFall) = _grid.GemCanFall(position);

                    if (gemCanFall || position.y == _gridSize.y - 1)
                    {
                        Gem gem = PlaceGem(position,
                            _grid.GridToWorldPosition(new Vector2Int(position.x, position.y + _gridSize.y)));
                        gem.FallTo(positionAfterFall, _grid.GridToWorldPosition(positionAfterFall));
                    }
                    else
                    {
                        PlaceGem(position, _grid.GridToWorldPosition(position));
                    }
                }
            }
        }
    }
}
