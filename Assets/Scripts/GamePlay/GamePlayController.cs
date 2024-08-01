using UnityEngine;
using Photon.Pun;
using Views;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Model;

public class GamePlayController : PhotonBaseView
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private SummaryView summaryView;

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (GameManager.Instance.IsMultiplayer)
        {
            roomNameText.text = "Room : " + PhotonNetwork.CurrentRoom.Name;
        }
        else
        {
            roomNameText.text = "Room : " + GameManager.Instance.UserName;
        }

        RegoisterEvents();
    }

    public override void OnConnectedToMaster()
    {
        LoggerUtil.Log("GamePlayController : Photon Connected to master ............");
        base.OnConnectedToMaster();
    }

    public override void OnLeftRoom()
    {
        LoggerUtil.Log("GamePlayController : OnLeftRoom ............");
        base.OnLeftRoom();
        SceneManager.LoadSceneAsync(1);
    }

    protected override void OnBackClick()
    {
        base.OnBackClick();
        DeRegoisterEvents();
        NetworkManager.Instance.LeaveRoom();
    }

    private void RegoisterEvents()
    {
        EventManager<BoardModel>.StartListening(Props.GameEvents.ON_ROUND_COMPLETE, OnRoundComplete);
        summaryView.OnHide += () => { OnBackClick(); };
    }

    private void DeRegoisterEvents()
    {
        EventManager<BoardModel>.StopListening(Props.GameEvents.ON_ROUND_COMPLETE, OnRoundComplete);
        summaryView.OnHide -= () => { OnBackClick(); };
    }

    private void OnRoundComplete(BoardModel model)
    {
        summaryView.SetData(new Gameplay.Rewards.RewardsData() { Coins = 10, Winner = model.Winner, Type = model.Type });
        summaryView.SetVisibility(true);
    }
}
