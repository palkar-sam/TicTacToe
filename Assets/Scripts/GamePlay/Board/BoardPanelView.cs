using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Views;

namespace Board
{
    public class BoardPanelView : BaseView
    {
        [SerializeField] private List<GameObject> boards;
        [SerializeField] private Transform boardContainer;

        public override void OnInitialize()
        {
            base.OnInitialize();
            if (GameManager.Instance.IsMultiplayer && PhotonNetwork.IsMasterClient)
            {
                //Instantiate(boards[0], boardContainer);
                GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Board/", "MultiplayerBoard"), Vector3.zero, Quaternion.identity);
            }
            else if (GameManager.Instance.IsSinglePlayer || GameManager.Instance.Is1vs1Enabled)
                Instantiate(boards[1], boardContainer);
        }
    }
}

