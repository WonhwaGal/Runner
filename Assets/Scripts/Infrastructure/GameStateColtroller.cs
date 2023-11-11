using System;
using DataSaving;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Commands;


namespace Infrastructure
{
    internal class GameStateColtroller: IDisposable
    {
        private CommandsManager _commandsManager;
        private DataController _dataController;

        public GameStateColtroller(CommandsManager commandsManager)
        {
            _commandsManager = commandsManager;

            _commandsManager.PauseView.OnContinueGame += PauseGame;
            _commandsManager.PauseView.OnExit += LoseGame;
            _commandsManager.PauseView.OnBackToMenu += LoadMenuScene;

            GameEventSystem.Subscribe<PauseGameEvent>(PauseGame);
            GameEventSystem.Subscribe<UpgradeEvent>(HitObstacle);
            _dataController = new DataController();
            StartGame();
        }

        public void StartGame() => _commandsManager.Start();
        public void PauseGame(PauseGameEvent pauseEvent) => _commandsManager.Pause(pauseEvent.IsPaused);
        private void StopGame() => _commandsManager.Stop();

        public void LoseGame()
        {
            StopGame();
            Sequence gameLostS = DOTween.Sequence();
            gameLostS.AppendInterval(2.0f).AppendCallback(LoadMenuScene);
        }

        private void HitObstacle(UpgradeEvent @event)
        {
            if (@event.UpgradeType == Collectables.UpgradeType.None)
                LoseGame();
        }

        private void LoadMenuScene()
        {
            SaveProgressAfterPlaying();
            SceneManager.LoadScene("MenuScene");
        }

        private void SaveProgressAfterPlaying()
        {
            if (_commandsManager.ProgressCommander is ProgressCommander commander)
            {
                commander.RegisterCurrentProgress();
                UnityEngine.Debug.Log($"registered current player is {commander.GameConfig.CurrentPlayer.Name}");
                _dataController.SaveProgressFromConfig(commander.GameConfig);
            }
            else
                UnityEngine.Debug.Log("Failed to save data");
        }

        public void Dispose() 
        {
            _commandsManager.PauseView.OnContinueGame -= PauseGame;
            _commandsManager.PauseView.OnExit -= LoseGame;
            _commandsManager.PauseView.OnBackToMenu -= LoadMenuScene;

            GameEventSystem.UnSubscribe<PauseGameEvent>(PauseGame);
        }
    }
}