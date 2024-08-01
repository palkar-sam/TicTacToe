using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;
using Views;

public class CreateView : BaseView
{
    [SerializeField] private TextMeshProUGUI inputText;

    public void CreateRoom()
    {
        NetworkManager.Instance.CreateRoom(inputText.text);
    }

    protected override void OnBackClick()
    {
        base.OnBackClick();
        NetworkManager.Instance.DisconnectToServer();
    }

}
