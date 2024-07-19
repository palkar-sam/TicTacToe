using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Views;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlayController : PhotonBaseView
{
    [SerializeField] private TextMeshProUGUI roomNameText;

    public override void OnConnectedToMaster()
    {
        Debug.Log("GamePlayController : Photon Connected to master ............");
        base.OnConnectedToMaster();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("GamePlayController : OnLeftRoom ............");
        base.OnLeftRoom();
        SceneManager.LoadSceneAsync(1);
    }

    public void OnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void Start()
    {
        roomNameText.text = "Room : "+PhotonNetwork.CurrentRoom.Name;
    }


}
