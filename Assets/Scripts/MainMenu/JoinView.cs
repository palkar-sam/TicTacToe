using Model;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views;

public class JoinView : BaseView
{
    [SerializeField] private TextMeshProUGUI joinText;
    [SerializeField] private RoomListPanel roomListPanel;
    [SerializeField] private Button joinButton;

    public override void OnInitialize()
    {
        base.OnInitialize();
        roomListPanel.OnRoomSelected += JoinRoom;
        EventManager<RoomListModel>.StartListening(Props.GameEvents.ON_ROOM_LIST_UPDATE, OnRoomListUpdate);
        joinButton.onClick.AddListener(OnJoinRoom);
    }

    public void OnJoinRoom()
    {
        string str = joinText.text;
        JoinRoom(str);
    }

    protected override void OnBackClick()
    {
        base.OnBackClick();
        NetworkManager.Instance.DisconnectToServer();
    }

    private void OnRoomListUpdate(RoomListModel model)
    {
        roomListPanel.UpdateRoomList(model.RoomList);
    }

    private void JoinRoom(string roomName)
    {
        NetworkManager.Instance.JoinRoom(joinText.text);
    }

    private void OnDestroy()
    {
        EventManager<RoomListModel>.StopListening(Props.GameEvents.ON_ROOM_LIST_UPDATE, OnRoomListUpdate);
    }
}
