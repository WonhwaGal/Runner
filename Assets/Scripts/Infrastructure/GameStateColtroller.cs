using System;
using DataSaving;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Commands;


namespace Infrastructure
{
    internal class GameStateColtroller: IDisposable
    {
        private readonly CommandsManager _commandsManager;
        private readonly DataController _dataController;
        private const string MenuScene = "MenuScene";

        public GameStateColtroller(CommandsManager commandsManager)
        {
            _commandsManager = commandsManager;

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
            SceneManager.LoadScene(MenuScene);
        }

        private void SaveProgressAfterPlaying()
        {
            if (_commandsManager.ProgressCommander is ProgressCommander commander)
            {
                commander.RegisterCurrentProgress();
                _dataController.SaveProgressFromConfig(commander.GameConfig);
            }
            else
            {
                UnityEngine.Debug.LogWarning("Failed to save data");
            }
        }

        public void Dispose() 
        {
            _commandsManager.PauseView.OnExit -= LoseGame;
            _commandsManager.PauseView.OnBackToMenu -= LoadMenuScene;

            GameEventSystem.UnSubscribe<PauseGameEvent>(PauseGame);
        }
    }
}