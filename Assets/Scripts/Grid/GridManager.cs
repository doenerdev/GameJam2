using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class GridManager : SingletonClass<GridManager>
{

    [SerializeField] private ushort _gridSize;
    [SerializeField] private GameObject _cube;

    private float _cubeSize = 1;
    private Dictionary<CubeSide, Grid> _grids = new Dictionary<CubeSide, Grid>();
    private LevelData _currentLevel;

    public ushort GridSize
    {
        get { return _gridSize;}
    }

    public float CubeSize
    {
        get { return _cubeSize; }
    }

    public LevelData CurrentLevel
    {
        get { return _currentLevel; }
    }

    public void GenerateGrids()
    {
        for (int i = 0; i < 6; i++)
        {
            _grids[(CubeSide)i] = Grid.Create((CubeSide)i, _cube);
        }
    }

    public void GenerateLevel(LevelData data)
    {        
        _gridSize = data.GridSize;
        _currentLevel = data;
        GridManager.instance.GenerateGrids();
    }


}
