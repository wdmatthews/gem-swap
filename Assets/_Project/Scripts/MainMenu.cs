using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GemSwap
{
    [AddComponentMenu("Gem Swap/Main Menu")]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _tutorialButton;
        [SerializeField] private Transform _credits;
        [SerializeField] private Button _creditsCloseButton;
        [SerializeField] private Transform _tutorial;
        [SerializeField] private Button _tutorialCloseButton;
        [SerializeField] private AudioManagerSO _audioManager;

        private void Awake()
        {
            _playButton.onClick.AddListener(Play);
            _creditsButton.onClick.AddListener(ShowCredits);
            _tutorialButton.onClick.AddListener(ShowTutorial);
            _creditsCloseButton.onClick.AddListener(HideCredits);
            _tutorialCloseButton.onClick.AddListener(HideTutorial);
            HideCredits();
            HideTutorial();
        }

        private void Start()
        {
            _audioManager.PlayMenuMusic();
        }

        private void Play()
        {
            SceneManager.LoadScene("Game");
        }

        private void ShowCredits()
        {
            _credits.gameObject.SetActive(true);
        }

        private void HideCredits()
        {
            _credits.gameObject.SetActive(false);
        }

        private void ShowTutorial()
        {
            _tutorial.gameObject.SetActive(true);
        }

        private void HideTutorial()
        {
            _tutorial.gameObject.SetActive(false);
        }
    }
}
