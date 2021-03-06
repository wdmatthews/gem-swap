using UnityEngine;

namespace GemSwap
{
    [CreateAssetMenu(fileName = "GameManager", menuName = "Gem Swap/Game Manager")]
    public class GameManagerSO : ScriptableObject
    {
        [SerializeField] private int _pointsPerGem;
        [SerializeField] private AnimationCurve _pointsPerLevel;
        [SerializeField] private GemManagerSO _gemManager;
        [SerializeField] private AudioManagerSO _audioManager;

        [System.NonSerialized] private bool _isOver;
        [System.NonSerialized] private int _points;
        [System.NonSerialized] private int _level;
        [System.NonSerialized] private int _pointsNeededToLevelUp;
        [System.NonSerialized] private int _lastPointsNeededToLevelUp;
        [System.NonSerialized] private System.Action _onStartGame;
        [System.NonSerialized] private GameHUD _gameHUD;
        [System.NonSerialized] private GameOverScreen _gameOverScreen;

        public bool IsOver => _isOver;

        public void Initialize(GameHUD gameHUD, GameOverScreen gameOverScreen, System.Action onStartGame)
        {
            _gameHUD = gameHUD;
            _gameOverScreen = gameOverScreen;
            _onStartGame = onStartGame;
        }

        public void StartGame(bool keepInactiveGems = false)
        {
            _isOver = false;
            _points = 0;
            _level = 1;
            _lastPointsNeededToLevelUp = 0;
            _pointsNeededToLevelUp = Mathf.RoundToInt(_pointsPerLevel.Evaluate(_level));
            _gemManager.Initialize(OnGemsRemoved, EndGame, keepInactiveGems);
            _onStartGame?.Invoke();
            _gameHUD?.Initialize(EndGame);
            _gameHUD?.Show();
            _gameOverScreen?.Hide();
            _audioManager?.PlayGameMusic();
        }

        public void EndGame()
        {
            _isOver = true;
            _gemManager.RemoveAllGems();
            _gameHUD?.Hide();
            _gameOverScreen?.Show(_level, _points, StartGame);
            _audioManager?.PlayGameOverMusic();
        }

        private void LevelUp()
        {
            _level++;
            _lastPointsNeededToLevelUp = _pointsNeededToLevelUp;
            int maxLevel = _pointsPerLevel.keys.Length;
            int clampedLevel = Mathf.Clamp(_level, 1, maxLevel);
            _pointsNeededToLevelUp = Mathf.RoundToInt(_pointsPerLevel.Evaluate(clampedLevel))
                + (_level > maxLevel ? _lastPointsNeededToLevelUp : 0);
            _gameHUD?.UpdateLevel(_level);
            _audioManager?.PlayLevelUpEffect();
        }

        private void OnGemsRemoved(int gemCount)
        {
            _points += gemCount * _pointsPerGem;

            while (_points >= _pointsNeededToLevelUp)
            {
                LevelUp();
            }

            _gameHUD?.UpdatePointsPercentage(1f * (_points - _lastPointsNeededToLevelUp)
                / (_pointsNeededToLevelUp - _lastPointsNeededToLevelUp));
        }
    }
}
