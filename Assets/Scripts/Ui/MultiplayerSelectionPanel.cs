using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public class MultiplayerSelectionPanel : PlayerSelectionPanel
    {
        protected override void OnBackClick()
        {
            base.OnBackClick();
            NetworkManager.Instance.DisconnectToServer();
        }
    }
}

