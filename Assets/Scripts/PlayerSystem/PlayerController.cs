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
        [SerializeField] private ParticleSystem _shield;
        [SerializeField] private ParticleSystem _magnet;

        private PlayerAnimator _playerAnimator;
        public PlayerAnimator PlayerAnimator { get => _playerAnimator;}
        public BaseMover Mover { get => _mover; }
        public PlayerTriggerModule TriggerModule { get => _trigger; }

        public void Initialize(IInput input, float jumpForce, TriggerHandler handler)
        {
            _shield.gameObject.SetActive(false);
            _magnet.gameObject.SetActive(false);
            _mover.Init(input, jumpForce);
            _playerMagnetController.Init(_magnetCollider);
            PlayerUpgradeController upgrader = handler.Upgrader;
            upgrader.AddComponents(_playerMagnetController, _shield.gameObject, _magnet.gameObject);
            TriggerModule.Init(_collider, handler);
            TriggerModule.ChangeLaneOnTurning += _mover.SetDefaultLane;
            _playerAnimator = new PlayerAnimator(_animator);
            _mover.OnChangingSpeed += _playerAnimator.Move;
            _mover.OnJumping += _playerAnimator.Jump;
        }

        public void ResumePlayerMove()
        {
            _mover.StartMove();
            PlayerAnimator.ResumeAnimation();
            PauseUpgrades(_shield, false);
            PauseUpgrades(_magnet, false);
        }

        public void PausePlayerMove()
        {
            _mover.PauseMoving();
            PlayerAnimator.FreezeAnimation();
            PauseUpgrades(_shield, true);
            PauseUpgrades(_magnet, true);
        }

        public void StopPlayerMove() => _mover.StopMoving();

        private void PauseUpgrades(ParticleSystem particles, bool toPause)
        {
            if (toPause)
                if (particles.gameObject.activeInHierarchy)
                    particles.Pause();
            else
                if (particles.gameObject.activeInHierarchy)
                    particles.Play();
        }


        private void OnDestroy()
        {
            _mover.OnChangingSpeed -= _playerAnimator.Move;
            _mover.OnJumping -= _playerAnimator.Jump;
            TriggerModule.ChangeLaneOnTurning -= _mover.SetDefaultLane;
            _mover.Dispose();
        }
    }
}