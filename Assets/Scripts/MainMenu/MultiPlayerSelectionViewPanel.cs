using Aik.Libs.StateMachine;
using Aik.Utils;

namespace MainMenu
{
    public class MultiPlayerSelectionViewPanel : AnimatableState
    {
        protected override void OnDispose()
        {
            LoggerUtil.Log("MultiPlayerSelectionViewPanel - OnDispose......");
        }

        protected override void OnHide()
        {
            LoggerUtil.Log("MultiPlayerSelectionViewPanel - OnHide......");
        }

        protected override void OnInitialize()
        {
            LoggerUtil.Log("MultiPlayerSelectionViewPanel - OnInitialize......");
        }

        protected override void OnShow()
        {
            LoggerUtil.Log("MultiPlayerSelectionViewPanel - OnShow......");
        }

        protected override void OnUpdate()
        {
            LoggerUtil.Log("MultiPlayerSelectionViewPanel - OnUpdate......");
        }
    }
}