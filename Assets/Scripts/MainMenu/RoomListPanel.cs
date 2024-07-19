using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Views;
using System;

public class RoomListPanel : BaseView
{
    [SerializeField] private RoomItem roomItemPrefab;
    [SerializeField] private Transform itemContainer;

    public event Action<string> OnRoomSelected;

    private List<RoomItem> roomItemLists = new List<RoomItem>(); 

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("RoomListPanel : Room Lists : " + string.Join(",",roomList));

        foreach(RoomItem item in roomItemLists)
        {
            item.OnItemClick -= OnRoomItemClick;
            Destroy(item.gameObject);
        }

        roomItemLists.Clear();

        foreach(RoomInfo room in roomList)
        {
            RoomItem item = Instantiate(roomItemPrefab, itemContainer);
            item.OnItemClick += OnRoomItemClick;
            item.SetRoomName(room.Name);
            roomItemLists.Add(item);
        }
    }

    private void OnRoomItemClick(string roomName)
    {
        OnRoomSelected?.Invoke(roomName);
    }
}
