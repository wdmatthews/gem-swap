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
        [SerializeField] private Transform _credits;
        [SerializeField] private Button _creditsCloseButton;


        private void Awake()
        {
            _playButton.onClick.AddListener(Play);
            _creditsButton.onClick.AddListener(ShowCredits);
            _creditsCloseButton.onClick.AddListener(HideCredits);
            HideCredits();
        }

        private void Start()
        {
            
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
    }
}
