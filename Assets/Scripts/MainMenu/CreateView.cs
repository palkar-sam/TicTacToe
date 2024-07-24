using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;
using Views;

public class CreateView : PhotonBaseView
{
    [SerializeField] private TextMeshProUGUI inputText;

    public void CreateRoom()
    {
        string str = inputText.text;

        if (!string.IsNullOrEmpty(str))
            PhotonNetwork.CreateRoom(str);
    }

    public override void OnJoinedRoom()
    {
        LoggerUtil.Log("On JoinRoom ---- ");
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(2);
    }

}
