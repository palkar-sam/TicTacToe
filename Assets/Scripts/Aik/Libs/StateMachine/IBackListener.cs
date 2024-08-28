namespace Aik.Libs.StateMachine
{
    public interface IBackListener
    {
        /// <summary>
        /// If true H/W back wont work when this view is being shown.
        /// </summary>
        bool OverrideBack { get; }
        /// <summary>
        /// Higher priority get presidence when backing. IBackListener will call higher priority panels when h/w back is pressed.
        /// Normaly return 0 for this.
        /// </summary>
        int Priority { get; }
        /// <summary>
        /// Called when h/w back is preseed.
        /// </summary>
        void OnBackEvent();
    }

}
