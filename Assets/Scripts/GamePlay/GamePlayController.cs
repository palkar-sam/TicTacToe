using UnityEngine;
using Photon.Pun;
using Views;
using TMPro;
using UnityEngine.SceneManagement;
using Model;
using Board;

public class GamePlayController : BaseView
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private SummaryView summaryView;

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (GameManager.Instance.IsMultiplayer)
        {
            roomNameText.text = "Room : " + NetworkManager.Instance.RoomName;
        }
        else
        {
            roomNameText.text = "Room : " + GameManager.Instance.UserName;
        }

        RegoisterEvents();
    }

    protected override void OnBackClick()
    {
        base.OnBackClick();
        DeRegoisterEvents();

        if (GameManager.Instance.IsMultiplayer)
            NetworkManager.Instance.LeaveRoom();
        else
            SceneManager.LoadSceneAsync(1);
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
