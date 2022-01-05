using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace GemSwap
{
    [AddComponentMenu("Gem Swap/Game Over Screen")]
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelLabel;
        [SerializeField] private TextMeshProUGUI _pointsLabel;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _menuButton;

        private System.Action<bool> _startGame;

        private void Awake()
        {
            _playAgainButton.onClick.AddListener(PlayAgain);
            _menuButton.onClick.AddListener(GoToMenu);
        }

        public void Show(int level, int points, System.Action<bool> startGame)
        {
            _levelLabel.text = $"Level {level}";
            _pointsLabel.text = $"{points} Points";
            _startGame = startGame;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void PlayAgain()
        {
            _startGame?.Invoke(true);
        }

        private void GoToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
