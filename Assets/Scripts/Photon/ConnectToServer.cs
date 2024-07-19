using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Views;

public class ConnectToServer : PhotonBaseView
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("ConnectToServer : Photon Connected to master ............");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        SceneManager.LoadSceneAsync(1);
    }
}
