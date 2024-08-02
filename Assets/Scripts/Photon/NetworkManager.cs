using Model;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Views;

public class NetworkManager : PhotonBaseView, IPunObservable
{
    private const int MAX_PLAYER = 2;

    public static NetworkManager Instance => _instance;

    private static NetworkManager _instance;
    private static readonly object lockObj = new object();

    [SerializeField] private Text statusText;
    [SerializeField] private GameObject loader;

    public event Action<Vector2, int> OnDataRecived;

    public bool IsConnected => PhotonNetwork.IsConnected;
    public string RoomName => PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.Name : string.Empty;

    public string ActiveUserName { get; set; }
    public Vector2 CellIndexs { get; set; }
    public int CellColor { get; set; }
    public int UserCellColorIndex { get; set; }

    private string _gameVersion = "v1";
    private bool _isMasterJoiningRoom;
    
    public override void OnInitialize()
    {
        base.OnInitialize();
        loader.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = _gameVersion;
    }

    #region MonobehaviourPunCallbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        LoggerUtil.Log("NetworkManager : Photon Connected to master ............");
        ShowStatus("Connection to server Done. Now Connecting to Lobby.....");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ShowStatus("Disconnected To Server Going to Lobyy......");
        EventManager.TriggerEvent(Props.GameEvents.ON_DISCONNECTED);
        StartCoroutine(HideLoader(0.5f));
    }

    public override void OnJoinedLobby()
    {
        LoggerUtil.Log("NetworkManager : Photon Connected to Lobby ............");
        base.OnJoinedLobby();
        ShowStatus("Server Connected to Lobby.");
        StartCoroutine(HideLoader());
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        LoggerUtil.Log($"NetworkManager : OnJoinRandomFailed : return code : {returnCode} : message : {message}");


        if (_isMasterJoiningRoom)
        {
            ShowLoader("Joining random room failed. No room is available to Join go back to lobby.");
            StartCoroutine(HideLoader(1.0f));
        }
        else
        {
            ShowLoader("Joining random room failed. Craeting new room with empty room name.");
            CreateRandomRoom(null);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        ShowStatus("Successfully Connected to Room.");
        if(!PhotonNetwork.IsMasterClient)
            StartCoroutine(HideLoader(0.5f));
        else
            ShowStatus("Joined room Waiting for another player to Join.");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        LoggerUtil.Log("NetworkManager : OnPlayerEnteredRoom : " + newPlayer.NickName+" : Total Player Count : "+ PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(HideLoader(0.2f));
            PhotonNetwork.LoadLevel(2);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        EventManager<RoomListModel>.TriggerEvent(Props.GameEvents.ON_ROOM_LIST_UPDATE, new RoomListModel { RoomList = roomList });
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        base.OnErrorInfo(errorInfo);
        LoggerUtil.Log("NetworkManager : OnErrorInfo : "+ errorInfo.Info);
    }

    public override void OnLeftRoom()
    {
        LoggerUtil.Log("NetworkManager : OnLeftRoom ............");
        base.OnLeftRoom();
        SceneManager.LoadSceneAsync(1);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        LoggerUtil.Log("NetworkManager : OnMasterClientSwitched : New Master : "+ newMasterClient.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer); 
        LoggerUtil.Log("NetworkManager : OnPlayerLeftRoom : Other Player : " + otherPlayer.NickName);
    }
    #endregion

    #region IPunObservable interface Functions, Photon event handling and synchronisaton.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            LoggerUtil.Log("NetworkManager : OnPhotonSerializeView : sending : "+CellIndexs);
            stream.SendNext(CellIndexs);
            stream.SendNext(UserCellColorIndex);
        }
        else
        {
            Vector2 cells = (Vector2)stream.ReceiveNext();
            int colorIndex = (int)stream.ReceiveNext();
            LoggerUtil.Log("NetworkManager : OnPhotonSerializeView : recieving : "+cells+" : Color ind : "+colorIndex);
            OnDataRecived?.Invoke(cells, colorIndex);
        }
    }
    #endregion

    public void ConnectToServer()
    {
        LoggerUtil.Log("NetworkManager : ConnectToServer : ");

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinLobby();
        else
        {

            ShowLoader("Connecting To Server......");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = ActiveUserName;
        }
    }

    public void CreateRoom(string roomName)
    {
        LoggerUtil.Log("NetworkManager : CreateRoom : " + roomName);
        if (string.IsNullOrEmpty(roomName))
        {
            ShowLoader("Room name is empty connecting to random room.");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ShowLoader("Creating New Room : " + roomName);
            CreateRandomRoom(roomName);
        }

    }

    public void JoinRoom(string roomName)
    {
        _isMasterJoiningRoom = true;
        LoggerUtil.Log("NetworkManager : JoinRoom : " + roomName);
        
        if (string.IsNullOrEmpty(roomName) || roomName.Length < 2)
        {
            ShowLoader("Room name is empty joining random room - ");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ShowLoader("Joining Room - " + roomName);
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void DisconnectToServer()
    {
        LoggerUtil.Log("NetworkManager : Photon Disconnected ............");
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void CreateRandomRoom(string roomName)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = MAX_PLAYER };
        if (string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.CreateRoom(null, options);
        }
        else
        {
            PhotonNetwork.CreateRoom(roomName, options);
        }
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

    private void ShowStatus(string statusStr)
    {
        if (statusText == null) return;

        LoggerUtil.Log("NetworkManager : Status : " + statusStr);
        statusText.text = statusStr;
    }

    private void ShowLoader(string str)
    {
        loader.SetActive(true);
        ShowStatus(str);
    }

    private IEnumerator HideLoader(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        loader.SetActive(false);
    }

}
