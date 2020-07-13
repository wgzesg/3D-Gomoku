using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class GameflowManager : MonoBehaviour
{
    public static GameflowManager GFM;

    int no_of_rounds;
    public int current_playerIndex;
    public TMPro.TextMeshProUGUI Winmessage;

    private int[,,] chessboard = new int[4,4,4];

    private Player[] playerList;
    public Player current_player;

    public UnityAction OnNewTurn;
    private bool GameEnded;
    public UnityAction OnGameEnded;

    public Player winner;


    // Singleton structure
    private void Awake()
    {
        if (GFM == null)
        {
            GFM = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(GFM != null)
            {
                Destroy(GFM.gameObject);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GameEnded = false;
        playerList = PhotonNetwork.PlayerList;

        PillarManager.PM.makeMoveEvent += OnMakeMove;

        no_of_rounds = 0;
        current_playerIndex = 1;
        current_player = playerList[current_playerIndex];

        for (int x = 0; x < 4; x++)
        {
            for(int y = 0; y < 4; y++)
            {
                for(int z = 0; z < 4; z++)
                {
                    chessboard[x,y,z] = 0;
                }
            }
        }
        Winmessage.gameObject.SetActive(false);

        if(OnNewTurn != null)
        {
            OnNewTurn.Invoke();
        }
    }


    public void SwitchPlayer()
    {
        Debug.Log("Switching player");
        current_playerIndex = 1 - current_playerIndex;
        current_player = playerList[current_playerIndex];

        if (OnNewTurn != null)
        {
            OnNewTurn.Invoke();
        }

    }

    public void OnMakeMove(Vector3Int newMoveLocation)
    {
        Debug.Log("made move callback");
        if (current_playerIndex == 0)
        {
            chessboard[newMoveLocation.x, newMoveLocation.y, newMoveLocation.z] = 1;
            if(checkWin(newMoveLocation, 1))
            {
                winHandler(current_player);
            }
        }
        else
        {
            chessboard[newMoveLocation.x, newMoveLocation.y, newMoveLocation.z] = -1;
            if (checkWin(newMoveLocation, -1))
            {
                winHandler(current_player);
            }
        }
        if (GameEnded)
            return;
        SwitchPlayer();
        no_of_rounds ++;
    }

    public void winHandler(Player winner)
    {
        Winmessage.gameObject.SetActive(true);
        if (winner == PhotonNetwork.LocalPlayer)
        {
            Winmessage.text = "You win";
        }
        else
        {
            Winmessage.text = "You lose";
        }
        GameEnded = true;

        if(OnGameEnded != null)
        {
            OnGameEnded.Invoke();
        }

    }

    #region check win logic
    bool checkWin(Vector3Int newmove, int current)
    {
        int x = newmove.x;
        int y = newmove.y;
        int z = newmove.z;

        bool isWin = false;

        int[,,] cube = new int[3, 3, 3];

        Stack<Vector3Int> nextToCheck = new Stack<Vector3Int>();

        // Check the 3*3*3 cubes around the current move;
        for(int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    Vector3Int CurrentMove = newmove + new Vector3Int(i, j, k);

                    bool moveStatus = Validmove(CurrentMove, newmove);
                    if (moveStatus && chessboard[CurrentMove.x, CurrentMove.y, CurrentMove.z] == current)
                    {
                        nextToCheck.Push(CurrentMove);
                        cube[i+ 1, j+ 1, k+1]++;
                    }
                }
            }
        }

        while (nextToCheck.Count != 0)
        {
            Vector3Int CurrentMove = nextToCheck.Pop();
            Vector3Int dir = new Vector3Int(Mathf.Clamp(CurrentMove.x - newmove.x, -1, 1), Mathf.Clamp(CurrentMove.y - newmove.y, -1, 1), Mathf.Clamp(CurrentMove.z - newmove.z, -1, 1));
            CurrentMove += dir;
            if (Validmove(CurrentMove, newmove) && chessboard[CurrentMove.x, CurrentMove.y, CurrentMove.z] == current)
            {
                cube[dir.x + 1, dir.y + 1, dir.z + 1]++;
                nextToCheck.Push(CurrentMove);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (cube[i, j, k] + cube[2 - i, 2 - j, 2 - k] == 3)
                    {
                        isWin = true;
                    }
                }
            }
        }

        Debug.Log("status is " + isWin);
        return isWin;
    }

    bool Validmove(Vector3Int CurrentMove, Vector3Int newmove)
    {
        if (CurrentMove == newmove)
        {
            return false;
        }
        if (CurrentMove.x < 0 || CurrentMove.y < 0 || CurrentMove.z < 0)
            return false;
        if (CurrentMove.x > 3 || CurrentMove.y > 3 || CurrentMove.z > 3)
            return false;
        return true;
    }
    #endregion
}
