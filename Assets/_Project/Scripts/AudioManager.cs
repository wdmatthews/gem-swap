using UnityEngine;

namespace GemSwap
{
    [AddComponentMenu("Gem Swap/Audio Manager")]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectSource;
        [SerializeField] private AudioManagerSO _audioManager;

        private void Awake()
        {
            _audioManager.Initialize(_musicSource, _effectSource);
        }
    }
}
