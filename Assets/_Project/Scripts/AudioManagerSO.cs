using UnityEngine;

namespace GemSwap
{
    [CreateAssetMenu(fileName = "AudioManager", menuName = "Gem Swap/Audio Manager")]
    public class AudioManagerSO : ScriptableObject
    {
        [SerializeField] private AudioClip _menuMusic;
        [SerializeField] private AudioClip _gameMusic;
        [SerializeField] private AudioClip _gameOverMusic;
        [SerializeField] private AudioClip _successfulSwapEffect;
        [SerializeField] private AudioClip _invalidSwapEffect;
        [SerializeField] private AudioClip _gemsRemovedEffect;
        [SerializeField] private AudioClip _levelUpEffect;

        [System.NonSerialized] private AudioSource _musicSource;
        [System.NonSerialized] private AudioSource _effectSource;

        public void Initialize(AudioSource musicSource, AudioSource effectSource)
        {
            _musicSource = musicSource;
            _effectSource = effectSource;
        }

        public void PlayMenuMusic()
        {
            PlayClip(_musicSource, _menuMusic);
        }

        public void PlayGameMusic()
        {
            PlayClip(_musicSource, _gameMusic);
        }

        public void PlayGameOverMusic()
        {
            PlayClip(_musicSource, _gameOverMusic);
        }

        public void PlaySuccessfulSwapEffect()
        {
            PlayClip(_effectSource, _successfulSwapEffect);
        }

        public void PlayInvalidSwapEffect()
        {
            PlayClip(_effectSource, _invalidSwapEffect);
        }

        public void PlayGemsRemovedEffect()
        {
            PlayClip(_effectSource, _gemsRemovedEffect);
        }

        public void PlayLevelUpEffect()
        {
            PlayClip(_effectSource, _levelUpEffect);
        }

        private void PlayClip(AudioSource source, AudioClip clip)
        {
            source.Stop();
            source.clip = clip;
            source.Play();
        }
    }
}
