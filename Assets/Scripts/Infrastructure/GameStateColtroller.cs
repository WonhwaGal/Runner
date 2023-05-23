using DataSaving;
using GameUI;
using PlayerSystem;
using ProgressSystem;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static ProgressSystem.GameProgressConfig;
using Tools;

namespace Infrastructure
{
    internal class GameStateColtroller
    {
        public Action OnGameStop;

        private IPlayerControlSystem _playerController;
        private IProgressController _progressController;
        private PlayerConfig _currentPlayer;
        private IUiController _uiController;

        private DataController _dataController;
        private bool _isPaused;

        public GameStateColtroller(IPlayerControlSystem playerController,
            IProgressController progressController,
            IUiController uiController)
        {
            _playerController = playerController;
            _progressController = progressController;
            _uiController = uiController;
            _currentPlayer = progressController.RecieveCurrentPlayer();

            _uiController.PauseView.OnContinueGame += PauseGame;
            _uiController.PauseView.OnExit += LoseGame;
            _uiController.PauseView.OnBackToMenu += LoadMenuScene;

            StartGame();
        }

        public void StartGame()
        {
            _playerController.CreatePlayer(_currentPlayer);
            _progressController.UIModel.StartDistanceCount();
        }
        public void PauseGame() 
        {
            _isPaused = !_isPaused;
            _uiController.PauseGame(_isPaused);
            _playerController.PausePlayer(_isPaused);
        }

        public void LoseGame()
        {
            SaveProgressAfterPlaying();

            Sequence gameLostS = DOTween.Sequence();
            gameLostS.AppendInterval(2.0f).AppendCallback(LoadMenuScene);
        }
        private void SaveProgressAfterPlaying()
        {
            _playerController.StopPlayer();
            _progressController.RecieveCurrentProgress();
            _dataController = new DataController(_progressController.GameConfig);
            _dataController.SaveProgress();
        }
        private void LoadMenuScene()
        {
            SaveProgressAfterPlaying();
            SceneManager.LoadScene("MenuScene");
        }

        private void IncreaseGameSpeed() // ?????????
        {
        }

        public void Dispose() 
        {
            _uiController.PauseView.OnContinueGame -= PauseGame;
            _uiController.PauseView.OnExit -= LoseGame;
        }
    }
}