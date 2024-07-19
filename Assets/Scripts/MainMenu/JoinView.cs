using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Views;

public class JoinView : PhotonBaseView
{
    [SerializeField] private TextMeshProUGUI joinText;
    [SerializeField] private RoomListPanel roomListPanel;

    public override void OnShow()
    {
        base.OnShow();
        PhotonNetwork.JoinLobby();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        roomListPanel.OnRoomSelected += JoinRoom;
    }

    public void OnJoinRoom()
    {
        string str = joinText.text;
        JoinRoom(str);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        roomListPanel.UpdateRoomList(roomList);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("JoinView : OnJoinedRoom");
        PhotonNetwork.LoadLevel(2);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("JoinView : OnJoinedLobby");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("JoinView : OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    private void JoinRoom(string roomName)
    {
        if (!string.IsNullOrEmpty(roomName))
            PhotonNetwork.JoinRoom(roomName);
    }
}
