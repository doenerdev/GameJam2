using UnityEngine;
using System.Collections;

public enum CubeSide
{
    front,
    back,
    top,
    bottom,
    left,
    right
}

public enum ConnectionDirectionGeneral
{
    vertical,
    horizontal
}

public class Grid : MonoBehaviour {

    [SerializeField] private GameObject _cube;
    [SerializeField] private CubeSide _cubeSide;

    private SubGrid[,] _subGrids;
    private float _subGridSize;

    public CubeSide CubeSide
    {
        get { return _cubeSide; }
    }

    private void Awake()
    {
        _subGrids = new SubGrid[GridManager.instance.GridSize, GridManager.instance.GridSize];
    }

    private void Initialize(CubeSide side, GameObject cube)
    {
        _cubeSide = side;
        _cube = cube;
        this.gameObject.transform.parent = _cube.transform;
        CalculateSubGridSize();
        GenerateSubGrids();
        SetPositionAndOrientation(side);
    }

    public static Grid Create(CubeSide side, GameObject cube)
    {
        GameObject gridGO = Instantiate(Resources.Load("Grid")) as GameObject;
        gridGO.name = "Grid " + side.ToString();
        Grid grid = gridGO.GetComponent<Grid>();
        grid.Initialize(side, cube);
        return grid;
    }

    private void SetPositionAndOrientation(CubeSide side)
    {
       switch (side)
        {
            case CubeSide.front:
                transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case CubeSide.back:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case CubeSide.top:
                transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case CubeSide.bottom:
                transform.rotation = Quaternion.Euler(270, 0, 0);
                break;
            case CubeSide.right:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case CubeSide.left:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
    }

    private void CalculateSubGridSize()
    {
        _subGridSize = GridManager.instance.CubeSize / GridManager.instance.GridSize; //old
        //_subGridSize = (GridManager.instance.CubeSize/GridManager.instance.GridSize) * 3;
    }

    private void GenerateSubGrids()
    {
        for(int x = 0; x < GridManager.instance.GridSize; x++)
        {
            for(int y = 0; y < GridManager.instance.GridSize; y++)
            {
                bool hasDot = GridManager.instance.CurrentLevel.GridData[_cubeSide][x + GridManager.instance.GridSize * y];
                _subGrids[x, y] = SubGrid.Create(CalculateSubgridPosition(x, y), this.gameObject.transform, hasDot, this, new Vector2(x,y));
                _subGrids[x, y].BoxCollider.size = new Vector3(_subGridSize, _subGridSize, 0);
                //_subGrids[x, y].gameObject.transform.localScale = new Vector3(_subGridSize, _subGridSize, 1);
                if (_cubeSide == CubeSide.left)
                {
                    
                }
            }
        }
    }

    private Vector3 CalculateSubgridPosition(int posX, int posY)
    {
        var subgridPosition = new Vector3();
        subgridPosition.x = (-(GridManager.instance.CubeSize / 2) + _subGridSize / 2) + posX * _subGridSize;
        subgridPosition.y = (-(GridManager.instance.CubeSize / 2) + _subGridSize / 2) + posY * _subGridSize;
        return subgridPosition;
    }

    public SubGrid GetSubGridAt(int x, int y)
    {
        return _subGrids[x, y];
    }

    public static int FromSideToSideDirectionVertical(CubeSide start, CubeSide destination)
    {
        switch (start)
        {
            case CubeSide.top:
                return (destination == CubeSide.back) ? 1 : -1;
                break;
            case CubeSide.bottom:
                return (destination == CubeSide.front) ? 1 : -1;
                break;
            default:
                return(destination == CubeSide.top) ? 1 : -1;
                break;
        }
    }

    public static int FromSideToSideDirectionHorizontal(CubeSide start, CubeSide destination)
    {
        switch (start)
        {
            case CubeSide.left:
                return (destination == CubeSide.front) ? 1 : -1;
                break;
            case CubeSide.right:
                return (destination == CubeSide.back) ? 1 : -1;
                break;
            default:
                return (destination == CubeSide.right) ? 1 : -1;
                break;
        }
    }

    public static ConnectionDirectionGeneral FromSideToSideDirectionGeneral(CubeSide start, CubeSide destination, Dot startDot, Dot destDot)
    {
        switch (start)
        {
            case CubeSide.top:
                if (destination == start)
                {
                    return ((int)startDot.ParentSubGrid.SubGridPosition.x == (int)destDot.ParentSubGrid.SubGridPosition.x)
                        ? ConnectionDirectionGeneral.vertical
                        : ConnectionDirectionGeneral.horizontal;
                }
                return (destination == CubeSide.front || destination == CubeSide.back)
                    ? ConnectionDirectionGeneral.vertical
                    : ConnectionDirectionGeneral.horizontal;
                break;
            case CubeSide.bottom:
                if (destination == start)
                {
                    return ((int)startDot.ParentSubGrid.SubGridPosition.x == (int)destDot.ParentSubGrid.SubGridPosition.x)
                        ? ConnectionDirectionGeneral.vertical
                        : ConnectionDirectionGeneral.horizontal;
                }
                return (destination == CubeSide.front || destination == CubeSide.back)
                    ? ConnectionDirectionGeneral.vertical
                    : ConnectionDirectionGeneral.horizontal;
                break;
            default:
                if (destination == start)
                {
                    return ((int)startDot.ParentSubGrid.SubGridPosition.x == (int)destDot.ParentSubGrid.SubGridPosition.x) 
                        ? ConnectionDirectionGeneral.vertical
                        : ConnectionDirectionGeneral.horizontal;
                }
                return (destination == CubeSide.top || destination == CubeSide.bottom)
                    ? ConnectionDirectionGeneral.vertical
                    : ConnectionDirectionGeneral.horizontal;
                break;
        }
    }
}
