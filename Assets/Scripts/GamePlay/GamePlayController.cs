using UnityEngine;
using Photon.Pun;
using Views;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

public class GamePlayController : PhotonBaseView
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private SummaryView summaryView;

    public override void OnInitialize()
    {
        base.OnInitialize();
        leaveRoomButton.gameObject.SetActive(GameManager.Instance.IsMultiplayer);

        if (GameManager.Instance.IsMultiplayer)
        {
            leaveRoomButton.onClick.AddListener(OnLeaveRoom);
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
        SceneManager.LoadSceneAsync(1);
    }

    public void OnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void RegoisterEvents()
    {
        EventManager.StartListening(Props.GameEvents.ON_ROUND_COMPLETE, OnRoundComplete);
    }

    private void OnRoundComplete()
    {
        summaryView.SetVisibility(true);
    }
}
