using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameflowManager : MonoBehaviour
{
    int no_of_rounds;
    public bool current_player;
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
    }

    // Update is called once per frame
    public void OnMakeMove(Vector3Int newMoveLocation)
    {
        if (current_player)
        {
            chessboard[newMoveLocation.x, newMoveLocation.y, newMoveLocation.z] = 1;
            Debug.Log("A black is placed at " + newMoveLocation.x + newMoveLocation.y + newMoveLocation.z);
            //checkWin(newMoveLocation, 1);
        }
        else
        {
            chessboard[newMoveLocation.x, newMoveLocation.y, newMoveLocation.z] = -1;
            Debug.Log("A white is placed.");
            //checkWin(newMoveLocation, -1);
        }
        current_player = !current_player;
        Debug.Log("CurrentPlayer: " + current_player);
        no_of_rounds ++;
    }

    bool checkWin(Vector3Int newmove, int current)
    {
        int x = newmove.x;
        int y = newmove.y;
        int z = newmove.z;

        bool isWin = false;

        int[,,] cube = new int[3, 3, 3];
        cube[1,1,1] = 1;

        Stack<Vector3Int> nextToCheck = new Stack<Vector3Int>();

        for(int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    Vector3Int CurrentMove = newmove + new Vector3Int(i, j, k);
                    if (i + j + k != 0 && chessboard[CurrentMove.x, CurrentMove.y, CurrentMove.z] == current)
                    {
                        nextToCheck.Push(CurrentMove);
                        cube[i, j, k]++;
                    }
                    else if(i + j + k != 0)
                    {
                        cube[i, j, k] = 0;
                    }
                }
            }
        }

        while(nextToCheck != null)
        {
            Vector3Int CurrentMove = nextToCheck.Pop();
            Vector3Int dir = new Vector3Int(Mathf.Clamp(CurrentMove.x - newmove.x, -1, 1), Mathf.Clamp(CurrentMove.y - newmove.y, -1, 1), Mathf.Clamp(CurrentMove.z - newmove.z, -1, 1));
            if (chessboard[CurrentMove.x, CurrentMove.y, CurrentMove.z] == current)
            {
                cube[dir.x, dir.y, dir.z]++;
                dir += CurrentMove;
                if (dir.x < 4 && dir.y < 4 && dir.z < 4 && dir.x > 0 && dir.y > 0 && dir.z > 0)
                    nextToCheck.Push(dir);
            }
            else
                cube[dir.x, dir.y, dir.z] = 0;
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    if (cube[i,j,k] + cube[2-i,2-j,2-k] == 3)
                    {
                        isWin = true;
                    }
                }
            }
        }

        return isWin;


    }
}
