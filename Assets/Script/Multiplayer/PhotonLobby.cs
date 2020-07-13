using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Collections;

public class PhotonLobby : MonoBehaviourPunCallbacks, IInRoomCallbacks
{

    public GameObject offlineKey;
    public GameObject onlineKey;

    public GameObject FrontPage;
    public GameObject LobbyPage;
    public GameObject RoomPage;

    public int gameLevel;
    public Room room;
    public TMPro.TextMeshProUGUI roomName;
    public RoomInfo thisRoomInfo;
    public GameObject startKey;

    public GameObject ExitRoomPanel;
    public GameObject JoinRoomPanel;


    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("This is connected to master");
        offlineKey.SetActive(false);
        onlineKey.SetActive(true);
    }


    #region FrontPage functions

    public void OnStartClicked()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LobbyPage.SetActive(true);
        FrontPage.SetActive(false);
        Debug.Log("joined lobby");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    #endregion

    #region LobbyPage function
    public void OnCreateRoomClicked()
    {
        CreateRoom();
    }

    public void OnJoinRandomRoomClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnExitLobbyClicked()
    {
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby()
    {
        LobbyPage.SetActive(false);
        FrontPage.SetActive(true);
        Debug.Log("exited lobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("there is no room available");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("I am in a room");
        LobbyPage.SetActive(false);
        RoomPage.SetActive(true);
        room = PhotonNetwork.CurrentRoom;
        PhotonNetwork.AutomaticallySyncScene = true;
        roomName.text = "Room " + room.Name;

        PhotonNetwork.Instantiate(Path.Combine("MultiplayerPrefabs", "NetPlayer"), transform.position, Quaternion.identity);
    }

    bool CreateRoom()
    {
        Debug.Log("creating a new room");
        string roomName = Random.Range(0, 100).ToString();
        RoomOptions roomOps = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 2
        };
        return PhotonNetwork.CreateRoom(roomName, roomOps);
    }
    #endregion

    #region Room functions
    public void OnRealStartClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameLevel);
        }
    }

    public void OnLeaveRoomClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        StartCoroutine(RejoinCoroutine());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Onjoined Room is called");
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("the number of player now is " + room.PlayerCount);
        if (room.PlayerCount == 2)
        {
            room.IsVisible = false;
            room.IsOpen = false;
            startKey.SetActive(true);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("One player left");
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("the number of player now is " + room.PlayerCount);
        if (room.PlayerCount == 2)
        {
            room.IsVisible = false;
            room.IsOpen = false;
            Debug.Log("Room is full hence locked");
            startKey.SetActive(true);
        }
        else
        {
            room.IsVisible = true;
            room.IsOpen = true;
            Debug.Log("room is open again");
            startKey.SetActive(false);
        }
    }

    IEnumerator RejoinCoroutine()
    {
        ExitRoomPanel.SetActive(true);
        while(PhotonNetwork.Server != ServerConnection.MasterServer || PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
            yield return null;
        PhotonNetwork.JoinLobby();
        ExitRoomPanel.SetActive(false);
        LobbyPage.SetActive(true);
        RoomPage.SetActive(false);
    }
    #endregion
}
