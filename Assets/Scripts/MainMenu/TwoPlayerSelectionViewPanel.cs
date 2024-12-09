using Aik.Libs.StateMachine;
using Aik.Utils;

namespace MainMenu
{
    public class TwoPlayerSelectionViewPanel : AnimatableState
    {
        protected override void OnDispose()
        {
            LoggerUtil.Log("TwoPlayerSelectionViewPanel - OnDispose......");
        }

        protected override void OnHide()
        {
            LoggerUtil.Log("TwoPlayerSelectionViewPanel - OnHide......");
        }

        protected override void OnInitialize()
        {
            LoggerUtil.Log("TwoPlayerSelectionViewPanel - OnInitialize......");
        }

        protected override void OnShow()
        {
            LoggerUtil.Log("TwoPlayerSelectionViewPanel - OnShow......");
        }

        protected override void OnUpdate()
        {
            LoggerUtil.Log("TwoPlayerSelectionViewPanel - OnUpdate......");
        }
    }
}