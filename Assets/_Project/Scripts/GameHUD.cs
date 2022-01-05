using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GemSwap
{
    [AddComponentMenu("Gem Swap/Game HUD")]
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private RectTransform _pointsBar;
        [SerializeField] private RectTransform _pointsBarFill;
        [SerializeField] private TextMeshProUGUI _levelLabel;
        [SerializeField] private Button _endGameButton;
        [SerializeField] private Transform _endGameConfirmWindow;
        [SerializeField] private Button _endGameCancelButton;
        [SerializeField] private Button _endGameConfirmButton;

        private System.Action _endGame;

        private void Awake()
        {
            _endGameButton.onClick.AddListener(OpenEndGameConfirmWindow);
            _endGameCancelButton.onClick.AddListener(CloseEndGameConfirmWindow);
            _endGameConfirmButton.onClick.AddListener(EndGame);
            CloseEndGameConfirmWindow();
        }

        public void Initialize(System.Action endGame)
        {
            _endGame = endGame;
            UpdateLevel(1);
            UpdatePointsPercentage(0);
        }

        public void UpdateLevel(int level)
        {
            _levelLabel.text = $"{level}";
        }

        public void UpdatePointsPercentage(float percentage)
        {
            _pointsBarFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                percentage * _pointsBar.rect.width);
        }

        private void OpenEndGameConfirmWindow()
        {
            _endGameConfirmWindow.gameObject.SetActive(true);
        }

        private void CloseEndGameConfirmWindow()
        {
            _endGameConfirmWindow.gameObject.SetActive(false);
        }

        private void EndGame()
        {
            _endGame?.Invoke();
        }
    }
}
