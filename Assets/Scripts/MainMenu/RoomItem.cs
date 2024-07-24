using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI nameText;

    public event Action<string> OnItemClick;

    private string _roomName;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemClick?.Invoke(_roomName);
    }

    public void SetRoomName(string name)
    {
        nameText.text = _roomName = name;
    }
}
