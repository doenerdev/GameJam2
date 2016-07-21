using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class LevelDataBase : ScriptableObject {


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

    public void SetRating(ushort rating)
    {
        _rating = rating;
    }

    public void SetLevelStatus(LevelStatus status)
    {
        _levelStatus = status;
    }
}
