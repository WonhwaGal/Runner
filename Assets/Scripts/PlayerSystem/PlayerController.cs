using Infrastructure;
using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerTriggerModule), typeof(BaseMover))]
    internal class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private BaseMover _mover;
        [SerializeField] private PlayerTriggerModule _trigger;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private CapsuleCollider _magnetCollider;
        [SerializeField] private PlayerMagnetController _playerMagnetController;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _magnet;

        private PlayerAnimator _playerAnimator;
        public PlayerAnimator PlayerAnimator { get => _playerAnimator;}
        public BaseMover Mover { get => _mover; }

        public void Initialize(IInput input, float jumpForce, TriggerHandler handler)
        {
            _shield.SetActive(false);
            _magnet.SetActive(false);
            _mover.Init(input, jumpForce);
            _playerMagnetController.Init(_magnetCollider);
            PlayerUpgradeController upgrader = handler.Upgrader;
            upgrader.AddComponents(_playerMagnetController, _shield, _magnet);
            _trigger.Init(_collider, handler);
            _trigger.ChangeLaneOnTurning += _mover.SetLane;
            _playerAnimator = new PlayerAnimator(_animator);
            _mover.OnChangingSpeed += _playerAnimator.Move;
            _mover.OnJumping += _playerAnimator.Jump;
        }

        public void ResumePlayerMove()
        {
            _mover.StartMove();
            PlayerAnimator.ResumeAnimation();
        }

        public void PausePlayerMove()
        {
            _mover.PauseMoving();
            PlayerAnimator.FreezeAnimation();
        }

        public void StopPlayerMove() => _mover.StopMoving();

        private void OnDestroy()
        {
            _mover.OnChangingSpeed -= _playerAnimator.Move;
            _mover.OnJumping -= _playerAnimator.Jump;
            _trigger.ChangeLaneOnTurning -= _mover.SetLane;
            _mover.Dispose();
        }
    }
}