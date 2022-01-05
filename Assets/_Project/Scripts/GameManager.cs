using UnityEngine;
using UnityEngine.InputSystem;

namespace GemSwap
{
    [AddComponentMenu("Gem Swap/Game Manager")]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GemManagerSO _gemManager;
        [SerializeField] private GameManagerSO _gameManager;
        [SerializeField] private GameHUD _gameHUD;
        [SerializeField] private GameOverScreen _gameOverScreen;
        [SerializeField] private float _cursorZPosition;
        [SerializeField] private Transform _cursor;
        [SerializeField] private Transform _selectedGemCursor;

        private Vector2Int _cursorPosition;
        private Vector2Int _selectedGemPosition;
        private bool _isSwapping;

        private void Start()
        {
            _gameManager.Initialize(_gameHUD, _gameOverScreen, OnStartGame);
            _gameManager.StartGame();
        }

        private void Update()
        {
            _gemManager.OnUpdate();
        }

        public void MoveCursorLeft(InputAction.CallbackContext context)
        {
            if (!context.performed || _gameManager.IsOver) return;
            MoveCursorTo(new Vector2Int(_cursorPosition.x - 1, _cursorPosition.y));
        }

        public void MoveCursorRight(InputAction.CallbackContext context)
        {
            if (!context.performed || _gameManager.IsOver) return;
            MoveCursorTo(new Vector2Int(_cursorPosition.x + 1, _cursorPosition.y));
        }

        public void MoveCursorDown(InputAction.CallbackContext context)
        {
            if (!context.performed || _gameManager.IsOver) return;
            MoveCursorTo(new Vector2Int(_cursorPosition.x, _cursorPosition.y - 1));
        }

        public void MoveCursorUp(InputAction.CallbackContext context)
        {
            if (!context.performed || _gameManager.IsOver) return;
            MoveCursorTo(new Vector2Int(_cursorPosition.x, _cursorPosition.y + 1));
        }

        public void Swap(InputAction.CallbackContext context)
        {
            if (!context.performed || _gameManager.IsOver) return;

            if (_isSwapping)
            {
                ConfirmSwap();
            }
            else
            {
                StartSwap();
            }
        }

        public void CancelSwap(InputAction.CallbackContext context)
        {
            if (!context.performed || _gameManager.IsOver) return;
            EndSwap();
        }

        private void MoveCursorTo(Vector2Int gridPosition)
        {
            Vector2Int gridSize = _gemManager.GridSize;
            _cursorPosition = new Vector2Int(Mathf.Clamp(gridPosition.x, 0, gridSize.x - 1),
                Mathf.Clamp(gridPosition.y, 0, gridSize.y - 1));
            Vector3 worldPosition = _gemManager.Grid.GridToWorldPosition(_cursorPosition);
            worldPosition.z = _cursorZPosition;
            _cursor.position = worldPosition;
        }

        private void StartSwap()
        {
            _isSwapping = true;
            _selectedGemPosition = _cursorPosition;
            Vector3 worldPosition = _gemManager.Grid.GridToWorldPosition(_selectedGemPosition);
            worldPosition.z = _cursorZPosition;
            _selectedGemCursor.position = worldPosition;
            _selectedGemCursor.gameObject.SetActive(true);
        }

        private void EndSwap()
        {
            _isSwapping = false;
            _selectedGemCursor.gameObject.SetActive(false);
        }

        private void ConfirmSwap()
        {
            _gemManager.SwapGems(_selectedGemPosition, _cursorPosition);
            EndSwap();
        }

        private void OnStartGame()
        {
            MoveCursorTo(new Vector2Int(0, 0));
            EndSwap();
        }
    }
}
