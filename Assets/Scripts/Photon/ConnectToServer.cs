using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Views;

public class ConnectToServer : PhotonBaseView
{
    [SerializeField] private TextMeshProUGUI status;


    public override void OnInitialize()
    {
        base.OnInitialize();
        PhotonNetwork.ConnectUsingSettings();
        status.text = "Loading ***";
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
