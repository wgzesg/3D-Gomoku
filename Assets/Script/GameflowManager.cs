using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameflowManager : MonoBehaviour
{
    int no_of_rounds;
    public bool current_player;
    public TMPro.TextMeshProUGUI Winmessage;

    private int[,,] chessboard = new int[4,4,4];

    // Start is called before the first frame update
    void Start()
    {
        no_of_rounds = 0;
        current_player = true;
        for(int x = 0; x < 4; x++)
        {
            for(int y = 0; y < 4; y++)
            {
                for(int z = 0; z < 4; z++)
                {
                    chessboard[x,y,z] = 0;
                }
            }
        }
        Winmessage.text = null;
    }

    // Update is called once per frame
    public void OnMakeMove(Vector3Int newMoveLocation)
    {
        if (current_player)
        {
            chessboard[newMoveLocation.x, newMoveLocation.y, newMoveLocation.z] = 1;
            if(checkWin(newMoveLocation, 1))
            {
                Winmessage.text = "Black wins";
            }
        }
        else
        {
            chessboard[newMoveLocation.x, newMoveLocation.y, newMoveLocation.z] = -1;
            if (checkWin(newMoveLocation, -1))
            {
                Winmessage.text = "White wins";
            }
        }
        current_player = !current_player;
        no_of_rounds ++;
    }

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

}
