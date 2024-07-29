using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Views;

public class NetworkManager : PhotonBaseView
{
    public static NetworkManager Instance => _instance;

    private static NetworkManager _instance;
    private static readonly object lockObj = new object();

    public bool IsConnected => PhotonNetwork.IsConnected;

    [SerializeField] private Text status;
    [SerializeField] private GameObject loader;

    public override void OnInitialize()
    {
        base.OnInitialize();
        loader.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        LoggerUtil.Log("NetworkManager : Photon Connected to master ............");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LoggerUtil.Log("NetworkManager : Photon Connected to Lobby ............");
        base.OnJoinedLobby();
        loader.SetActive(false);
        //SceneManager.LoadSceneAsync(1);
    }

    public void ConnectToServer()
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.JoinLobby();
        else
            PhotonNetwork.ConnectUsingSettings();
        //status.text = "Loading ***";
        loader.SetActive(true);
    }

    public void DisconnectToServer()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else if (_instance == null)
        {
            lock (lockObj)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
