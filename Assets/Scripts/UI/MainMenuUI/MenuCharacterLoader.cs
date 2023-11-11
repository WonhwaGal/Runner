using ProgressSystem;
using UnityEngine;


namespace MainMenu
{
    internal class MenuCharacterLoader
    {
        private readonly GameProgressConfig _gameConfig;
        private MenuCharacterController _menuCharacterPrefab;

        public MenuCharacterLoader(GameProgressConfig gameConfig, Transform characterSpot)
        {
            _gameConfig = gameConfig;
            AssignMenuCharacterPrefab();
            if (_menuCharacterPrefab != null)
                LoadMenuCharacter(characterSpot);
            else
                UnityEngine.Debug.Log("No prefab found");
        }

        public MenuCharacterController MenuCharacter { get; private set; }

        private void LoadMenuCharacter(Transform characterSpot)
        {
            MenuCharacter = GameObject.Instantiate(_menuCharacterPrefab, characterSpot.position,
                    Quaternion.LookRotation(-Vector3.forward, Vector3.up));
        }

        private void AssignMenuCharacterPrefab()
        {
            if (_gameConfig.CurrentPlayer != null && _gameConfig.CurrentPlayer.IsOpen)
                _menuCharacterPrefab = _gameConfig.CurrentPlayer.MenuCharacter;
            else
            {
                for (int i = 0; i < _gameConfig.Players.Count; i++)
                {
                    if (_gameConfig.Players[i].IsDefault)
                        _menuCharacterPrefab = _gameConfig.Players[i].MenuCharacter;
                }
            }
        }

        public void ShowMenuPlayer(bool shouldShow) => MenuCharacter.gameObject.SetActive(shouldShow);
    }
}