using UnityEngine;
using System.Collections;

public enum ConnectionLineType
{
    horzPos,
    horzNeg,
    vertPos,
    vertNeg,
}

public class ConnectionLines : MonoBehaviour
{

    [SerializeField] private SubGrid _parentSubGrid;

    [SerializeField]
    private GameObject _connectionLineHorzPosGo;
    [SerializeField]
    private GameObject _connectionLineHorzNegGo;
    [SerializeField]
    private GameObject _connectionLineVertPosGo;
    [SerializeField]
    private GameObject _connectionLineVetNegGo;
    [SerializeField]
    private GameObject _connectionLineCurveHorzPosGo;
    [SerializeField]
    private GameObject _connectionLineCurveHorzNegGo;
    [SerializeField]
    private GameObject _connectionLineCurveVertPosGo;
    [SerializeField]
    private GameObject _connectionLineCurveVetNegGo;

    public GameObject connectionLineHorzPosGo;
    public GameObject connectionLineHorzNegGo;
    public GameObject connectionLineVertPosGo;
    public GameObject connectionLineVetNegGo;


    public void InitConnectionTypes(int x, int y)
    {
        connectionLineHorzPosGo = _connectionLineHorzPosGo;
        connectionLineHorzNegGo = _connectionLineHorzNegGo;
        connectionLineVertPosGo = _connectionLineVertPosGo;
        connectionLineVetNegGo = _connectionLineVetNegGo;


        if (x == 0)
        {
            connectionLineHorzNegGo = _connectionLineCurveHorzNegGo;
        }
        else if (x == GridManager.instance.GridSize - 1)
        {
            connectionLineHorzPosGo = _connectionLineCurveHorzPosGo;
        }

        if (y == 0)
        {
            connectionLineVetNegGo = _connectionLineCurveVetNegGo;
        }
        else if (y == GridManager.instance.GridSize - 1)
        {
            connectionLineVertPosGo = _connectionLineCurveVertPosGo;
        }

    }

    /*public GameObject GetConnectionLine(ConnectionLineType connectType, int x, int y, bool firstSide, int direction)
    {
        switch (connectType)
        {
            case ConnectionLineType.horzPos:
                if(direction > 0 && x == GridManager.instance.GridSize-1)
                    return connectionLineCurveHorzPosGo;
                else if(direction > 0 && x == GridManager.instance.GridSize - 1)
                break;
            case ConnectionLineType.horzNeg:
            break;
            case ConnectionLineType.vertPos:
            case ConnectionLineType.vertNeg:
            break;
        }

        return new GameObject();
    }*/
}
