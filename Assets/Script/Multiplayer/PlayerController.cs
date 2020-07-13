using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int targetScene;
    public PhotonView PV;
    public TMPro.TextMeshProUGUI turnInfo;

    public GameObject playerPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += onSceneFinishedLoading;
        PV = GetComponent<PhotonView>();
        turnInfo.gameObject.SetActive(false);
    }

    public void OnNewTurnHandler()
    {

        if (GameflowManager.GFM.current_player == PhotonNetwork.LocalPlayer)
            turnInfo.text = "your turn";
        else
            turnInfo.text = "Not your turn";
    }

    public void onSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        int currentScene = scene.buildIndex;
        if(currentScene == targetScene && PV.IsMine)
        {
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        GameflowManager.GFM.OnNewTurn += OnNewTurnHandler;
        if (PV.IsMine)
            turnInfo.gameObject.SetActive(true);

    }
}
