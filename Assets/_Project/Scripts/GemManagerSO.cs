using System.Collections.Generic;
using UnityEngine;

namespace GemSwap
{
    [CreateAssetMenu(fileName = "GemManager", menuName = "Gem Swap/Gem Manager")]
    public class GemManagerSO : ScriptableObject
    {
        [SerializeField] private Vector2 _gridZeroWorldPosition;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Gem _gemPrefab;
        [SerializeField] private GemSO[] _gemData;
        [SerializeField] private AudioManagerSO _audioManager;

        [System.NonSerialized] private GemGrid _grid;
        [System.NonSerialized] private List<Gem> _activeGems;
        [System.NonSerialized] private List<Gem> _gemsInMatch;
        [System.NonSerialized] private Stack<Gem> _inactiveGems;
        [System.NonSerialized] private int _fallingGemCount;
        [System.NonSerialized] private bool _shouldRemoveGems;
        [System.NonSerialized] private System.Action<int> _onGemsRemoved;
        [System.NonSerialized] private System.Action _endGame;

        public Vector2Int GridSize => _gridSize;
        public GemGrid Grid => _grid;

        public void Initialize(System.Action<int> onGemsRemoved, System.Action endGame, bool keepInactiveGems = false)
        {
            _onGemsRemoved = onGemsRemoved;
            _endGame = endGame;
            _activeGems = new List<Gem>();
            _gemsInMatch = new List<Gem>();

            if (!keepInactiveGems)
            {
                _grid = new GemGrid(_gridZeroWorldPosition, _gridSize);
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
                _audioManager?.PlaySuccessfulSwapEffect();
            }
            else
            {
                _audioManager?.PlayInvalidSwapEffect();
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
                gem.Remove();
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

        private void RemoveGem(Gem gem)
        {
            _inactiveGems.Push(gem);
            _activeGems.Remove(gem);
            gem.Remove();
            _grid.RemoveGem(gem.Position);
        }

        private void RemoveMatchedGems()
        {
            _shouldRemoveGems = false;
            int removedGemCount = 0;
            _gemsInMatch.Clear();

            for (int i = _activeGems.Count - 1; i >= 0; i--)
            {
                Gem gem = _activeGems[i];
                Vector2Int position = gem.Position;
                if (!_grid.IsInLine(position)) continue;
                _gemsInMatch.Add(gem);
            }

            foreach (Gem gem in _gemsInMatch)
            {
                removedGemCount++;
                RemoveGem(gem);
            }

            if (removedGemCount > 0)
            {
                MakeGemsFall();
                MakeNewGemsFall();
                _onGemsRemoved?.Invoke(removedGemCount);
                _audioManager?.PlayGemsRemovedEffect();
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
                    Gem gem = _grid.GetGem(position);
                    if (!gem) continue;
                    (bool gemCanFall, Vector2Int positionAfterFall) = _grid.GemCanFall(position);
                    if (!gemCanFall) continue;
                    _fallingGemCount++;
                    gem.FallTo(positionAfterFall,
                        _grid.GridToWorldPosition(positionAfterFall),
                        OnGemStoppedFalling);
                    _grid.MakeGemFallTo(position, positionAfterFall);
                }
            }
        }

        private void MakeNewGemsFall()
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Vector2Int position = new(x, y);
                    if (_grid.GetGem(position)) continue;
                    Gem gem = PlaceGem(position, _grid.GridToWorldPosition(
                        new Vector2Int(position.x, position.y + _gridSize.y)));
                    gem.FallTo(position, _grid.GridToWorldPosition(position));
                }
            }
        }
    }
}
