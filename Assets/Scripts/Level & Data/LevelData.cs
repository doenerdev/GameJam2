using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public enum LevelStatus {
	Unlocked,
	Locked,
	Done
}

public class LevelData : ScriptableObject
{
	protected string _fileName;
    protected ushort _levelNo;
    protected ushort _qtyDots;
    protected ushort _gridSize;
    protected LevelStatus _levelStatus;
    protected ushort _rating;
    protected float _perfectRatingTime = 20f;
    protected Dictionary<CubeSide, List<bool>> _gridData;

	public ushort LevelNo
	{
		get { return _levelNo; }
	}

	public string FileName
	{
		get { return _fileName; }
	}

    public ushort QtyDots
    {
        get { return _qtyDots; }
    }

    public ushort GridSize
    {
        get { return _gridSize; }
    }

    public Dictionary<CubeSide, List<bool>> GridData
    {
        get { return _gridData; }
    }

	public LevelStatus LevelStatus 
	{
		get { return _levelStatus; }
		set { _levelStatus = value; }
	}

	public ushort Rating 
	{
		get { return _rating; }
		set { _rating = value; }
	}

    public float perfectRatingTime
    {
        get { return _perfectRatingTime; }
    }

	public void Init(ushort levelNo, string filename, LevelStatus levelStatus, ushort rating, Dictionary<CubeSide, List<bool>> grids, ushort gridSize, ushort qtyDots)
    {
        _gridData = grids;
        _gridSize = gridSize;
        _qtyDots = qtyDots;
		_levelNo = levelNo;
		_levelStatus = levelStatus;
		_rating = rating;
		_fileName = filename;
    }

    public void Init(ushort gridSize)
    {
        _gridSize = gridSize;
        _gridData = GenerateBlankGrid();
    }

    private Dictionary<CubeSide, List<bool>> GenerateBlankGrid()
    {
        Dictionary<CubeSide, List<bool>> gridData = new Dictionary<CubeSide, List<bool>>();
        for (int i = 0; i < 6; i++)
        {
            gridData[(CubeSide) i] = new List<bool>();
            for (int j = 0; j < _gridSize * _gridSize; j++)
            {
                gridData[(CubeSide)i].Add(false);
            }
        }
        return gridData;
    }

    public void SetGridDataAt(CubeSide side, int index, bool value)
    {
        _gridData[(CubeSide) side][index] = value;
    }

    public void SetRating(ushort rating)
    {
        _rating = rating;
    }

    public void SetLevelStatus(LevelStatus status)
    {
        _levelStatus = status;
    }
}
