using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviourPunCallbacks
{
    public int startSceneIndex;
    public GameObject EndGameMsg;
    public GameObject startNewGameKey;

    private PhotonLobby PL;

    private void Start()
    {
        GameflowManager.GFM.OnGameEnded += OnEndGameHandler;
        EndGameMsg.SetActive(false);
        startNewGameKey.SetActive(false);
        PL = FindObjectOfType<PhotonLobby>();
    }

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
                GameflowManager.GFM.PV.RPC("winHandler", RpcTarget.All, current);
                break;
            }
        }
    }

    public void OnQuitToLobbyClicked()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }


    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.sceneLoaded += onSceneFinishedLoading;
        SceneManager.LoadScene(startSceneIndex);
    }

    public void onSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == startSceneIndex)
            PL.returnFromGameRoom();
    }

    public void OnResetCam()
    {
        CameraController mainController = FindObjectOfType<Camera>().GetComponent<CameraController>();
        mainController.OnResetCam();
    }
    #endregion

    public void OnEndGameHandler()
    {
        Debug.Log("Game Ended");
        EndGameMsg.SetActive(true);
        startNewGameKey.SetActive(true);
    }

    #region EndGameMsg functions

    public void OnStartNewGameClicked()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}
