using DataSaving;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Commands;


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

            _dataController = new DataController();
            StartGame();
        }

        public void StartGame() => _commandsManager.Start();
        public void PauseGame(bool isPaused) => _commandsManager.Pause(isPaused);
        private void StopGame() => _commandsManager.Stop();

        public void LoseGame()
        {
            StopGame();
            Sequence gameLostS = DOTween.Sequence();
            gameLostS.AppendInterval(2.0f).AppendCallback(LoadMenuScene);
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
        }
    }
}