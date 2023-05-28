using DataSaving;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;


namespace Infrastructure
{
    internal class GameStateColtroller
    {
        private CommandsManager _commandsManager;
        private DataController _dataController;

        public GameStateColtroller(CommandsManager commandsManager)
        {
            _commandsManager = commandsManager;

            _commandsManager.PauseView.OnContinueGame += PauseGame;
            _commandsManager.PauseView.OnExit += LoseGame;
            _commandsManager.PauseView.OnBackToMenu += LoadMenuScene;

            StartGame();
        }

        public void StartGame() => _commandsManager.Start();
        public void PauseGame(bool isPaused) => _commandsManager.Pause(isPaused);
        private void StopGame() => _commandsManager.Stop();

        public void LoseGame()
        {
            StopGame();
            SaveProgressAfterPlaying();
            Sequence gameLostS = DOTween.Sequence();
            gameLostS.AppendInterval(2.0f).AppendCallback(LoadMenuScene);
        }
        private void SaveProgressAfterPlaying()
        {
            if (_commandsManager.ProgressCommander is ProgressCommander commander)
            {
                commander.RegisterCurrentProgress();
                _dataController = new DataController(commander.GameConfig);
                _dataController.SaveProgress();
            }
            else
                UnityEngine.Debug.Log("Failed to save data");
        }

        private void LoadMenuScene()
        {
            SaveProgressAfterPlaying();
            SceneManager.LoadScene("MenuScene");
        }

        public void Dispose() 
        {
            _commandsManager.PauseView.OnContinueGame -= PauseGame;
            _commandsManager.PauseView.OnExit -= LoseGame;
            _commandsManager.PauseView.OnBackToMenu -= LoadMenuScene;
        }
    }
}