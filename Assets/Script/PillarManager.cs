using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PillarManager: MonoBehaviour
{
    public static PillarManager PM;
    public GameObject BlackpiecePrefab;
    public GameObject WhitepiecePrefab;

    public GameObject pillarPrefab;
    public List<Pillar> pillars { get; private set; }
    public int numberOfTotalPillar;
    public PhotonView PV;

    public UnityAction<Vector3Int> makeMoveEvent;

    private void Awake()
    {
        #region singleton
        if (PM == null)
        {
            PM = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(PM != null)
            {
                Destroy(PM.gameObject);
            }
        }
        #endregion

        numberOfTotalPillar = 0;
        PV = GetComponent<PhotonView>();

        pillars = new List<Pillar>();
    }

    public void Start()
    {
        SpawnPillars();
    }

    private void SpawnPillars()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                GameObject newItem = Instantiate(pillarPrefab, new Vector3(-30f + 20f * i , 25f, -30f + 20f * j), Quaternion.identity);
                Pillar pillar = newItem.GetComponent<Pillar>();
                pillar.xCood = i;
                pillar.yCood = j;
                RegisterPillar(pillar);
            }
        }
    }

    public void RegisterPillar(Pillar pillar)
    {
        pillars.Add(pillar);
        pillar.index = numberOfTotalPillar;

        numberOfTotalPillar++;
    }

    [PunRPC]
    public void MakeMoveOn(int pillarIndex, int zCood)
    {
        Pillar currentPillar = pillars[pillarIndex];
        currentPillar.piecesCount++;
        Instantiate(GameflowManager.GFM.current_playerIndex == 0 ? BlackpiecePrefab : WhitepiecePrefab, currentPillar.transform.position + new Vector3(0, 10 * currentPillar.piecesCount - 25, 0), Quaternion.identity);

        if (makeMoveEvent != null)
        {
            Vector3Int newmovelocation = new Vector3Int(currentPillar.xCood, currentPillar.yCood, currentPillar.piecesCount - 1);
            makeMoveEvent(newmovelocation);
        }
    }
}

