using Infrastructure;

namespace PlayerSystem
{
    internal interface IPlayerController
    {
        public PlayerAnimator PlayerAnimator { get; }
        public BaseMover Mover { get; }
        void Initialize(IInput input, float jumpForce, TriggerHandler handler);
        void ResumePlayerMove();
        void PausePlayerMove();
        void StopPlayerMove();
    }
}