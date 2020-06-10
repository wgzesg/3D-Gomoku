using UnityEngine;
using UnityEngine.Events;

public class Pillar : MonoBehaviour
{
    public int index;
    public GameObject BlackpiecePrefab;
    public GameObject WhitepiecePrefab;

    int piecesCount;

    GameflowManager gameFlowManager;
    PillarManager pillarManager;

    private UnityAction<Vector3Int> makeMoveEvent;
    private int xCood, yCood;
    


    void Start()
    {
        pillarManager = FindObjectOfType<PillarManager>();
        gameFlowManager = FindObjectOfType<GameflowManager>();

        pillarManager.RegisterPillar(this);
        Debug.Log(index);
        piecesCount = 0;
        makeMoveEvent += gameFlowManager.OnMakeMove;
        string name = transform.name;
        string parentName = transform.parent.name;
        xCood = parentName[3] - '0';
        yCood = name[10] - '0';

    }

    private void OnMouseEnter()
    {
        Debug.Log("This is Pillar " + xCood + ", " + yCood + ".");
    }

    private void OnMouseDown()
    {
        if(piecesCount < 4)
        {
            Debug.Log("A move on Pillar " + xCood + ", " + yCood + ".");
            piecesCount++;
            Instantiate(gameFlowManager.current_player? BlackpiecePrefab: WhitepiecePrefab, new Vector3(transform.position.x, 10 * piecesCount, transform.position.z), Quaternion.identity);

            if (makeMoveEvent != null)
            {
                Vector3Int newmovelocation = new Vector3Int(xCood, yCood, piecesCount - 1);
                makeMoveEvent(newmovelocation);
            }
        }
        else
        {
            Debug.Log("Invalid move!");
        }
    }
}
