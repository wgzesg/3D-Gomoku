using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public int startSceneIndex;

    #region in game menu function
    public void OnResignClicked()
    {
        Resign();
    }

    public void Resign()
    {
        foreach (Player current in PhotonNetwork.PlayerList)
        {
            if (current != PhotonNetwork.LocalPlayer)
            {
                GameflowManager.GFM.winHandler(current);
                break;
            }
        }
    }

    public void OnQuitToLobbyClicked()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            SceneManager.sceneLoaded += onSceneFinishedLoading;
            PhotonNetwork.LoadLevel(startSceneIndex);
        }
    }

    public void onSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        PhotonLobby theLobby = FindObjectOfType<PhotonLobby>();
        theLobby.returnFromGameRoom();
    }



    public void OnResetCam()
    {
        CameraController mainController = FindObjectOfType<Camera>().GetComponent<CameraController>();
        mainController.OnResetCam();
    }
    #endregion
}
