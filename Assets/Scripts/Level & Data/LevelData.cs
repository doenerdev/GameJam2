using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public enum LevelStatus 
{
	Unlocked,
	Locked,
	Done
}

[XmlRoot("LevelData")]
public class LevelData : MonoBehaviour
{
	private string _fileName;
	private ushort _levelNo;
    private ushort _qtyDots;
    private ushort _gridSize;
	private LevelStatus _levelStatus;
	private ushort _rating;
    private Dictionary<CubeSide, List<bool>> _gridData;

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
	}

	public ushort Rating 
	{
		get { return _rating; }
	}

    public LevelData(ushort levelNo, string filename, LevelStatus levelStatus, ushort rating, Dictionary<CubeSide, List<bool>> grids, ushort gridSize, ushort qtyDots)
    {
        _gridData = grids;
        _gridSize = gridSize;
        _qtyDots = qtyDots;
		_levelNo = levelNo;
		_levelStatus = levelStatus;
		_rating = rating;
		_fileName = filename;
    }
}
