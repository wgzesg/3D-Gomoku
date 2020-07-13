using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public int gameLevel;
    public Room room;
    public TMPro.TextMeshProUGUI roomName;
    public RoomInfo thisRoomInfo;

    public GameObject startKey;
    //public GameObject readyKey;

    public override void OnEnable()
    {
        room = PhotonNetwork.CurrentRoom;
        PhotonNetwork.AutomaticallySyncScene = true;
        roomName.text = "Room " + room.Name;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Onjoined Room is called");
        if (!PhotonNetwork.IsMasterClient)
            return;
        thisRoomInfo = PhotonNetwork.CurrentRoom;
        Debug.Log("the number of player now is " + thisRoomInfo.PlayerCount);
        if (thisRoomInfo.PlayerCount == 2)
        {
            room.IsVisible = false;
            room.IsOpen = false;
            Debug.Log("I am master client");
            startKey.SetActive(true);
        }
    }

    public void OnRealStartClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameLevel);
        }
    }
}
