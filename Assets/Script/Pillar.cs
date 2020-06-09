using UnityEngine;
using UnityEngine.Events;

public class Pillar : MonoBehaviour
{
    public int index;
    public GameObject BlackpiecePrefab;
    public GameObject WhitepiecePrefab;

    float piecesCount;

    GameflowManager gameFlowManager;
    PillarManager pillarManager;

    private UnityAction<Vector3> makeMoveEvent;
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
            if (gameFlowManager.current_player)
                Instantiate(BlackpiecePrefab, new Vector3(transform.position.x, 10 * piecesCount, transform.position.z), Quaternion.identity);
            else
                Instantiate(WhitepiecePrefab, new Vector3(transform.position.x, 10 * piecesCount, transform.position.z), Quaternion.identity);
            if (makeMoveEvent != null)
            {
                Vector3 newmovelocation = new Vector3(xCood, yCood, piecesCount);
                makeMoveEvent(newmovelocation);
            }
            piecesCount++;
        }
        else
        {
            Debug.Log("Invalid move!");
        }
    }
}
