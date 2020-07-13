using Photon.Pun;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public int index;

    public int piecesCount;

    public int xCood;
    public int yCood;
    


    void Start()
    {
        piecesCount = 0;

    }

    private void OnMouseEnter()
    {
        Debug.Log("This is Pillar " + xCood + ", " + yCood + ".");
    }

    private void OnMouseDown()
    {
        if(piecesCount < 4)
        {
            if (GameflowManager.GFM.current_player == PhotonNetwork.LocalPlayer)
            {
                PillarManager.PM.PV.RPC("MakeMoveOn", RpcTarget.All, index, piecesCount);
            }
            else
            {
                Debug.Log("not your turn");
            }
        }
        else
        {
            Debug.Log("Invalid move!");
        }
    }
}
