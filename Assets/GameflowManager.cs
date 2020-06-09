using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameflowManager : MonoBehaviour
{
    int no_of_rounds;
    public bool current_player;
    private List<List<List<int>>> chessboard;

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
                    chessboard[x][y][z] = 0;
                }
            }
        }
    }

    // Update is called once per frame
    public void OnMakeMove(Vector3 newMoveLocation)
    {
        if (current_player)
        {
            chessboard[(int)newMoveLocation.x][(int)newMoveLocation.y][(int)newMoveLocation.z] = 1;
            checkWin(newMoveLocation, 1);
        }
        else
        {
            chessboard[(int)newMoveLocation.x][(int)newMoveLocation.y][(int)newMoveLocation.z] = -1;
            checkWin(newMoveLocation, -1);
        }
        current_player = !current_player;
        no_of_rounds ++;
    }

    void checkWin(Vector3 newmove, int current)
    {
        int x = (int) newmove.x;
        int y = (int) newmove.y;
        int z = (int) newmove.z;

        bool isWin = false;

        for(int i = 0; i < 4; i++)
        {

        }


    }
}
