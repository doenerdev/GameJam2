using System;
using UnityEngine;
using System;
using System.Collections;

public enum ConnectionLine
{
    horizontal,
    vertical
}

[RequireComponent(typeof(BoxCollider))]
public class SubGrid : MonoBehaviour
{
    [SerializeField] private Dot _dot;
    [SerializeField] private ConnectionLines _connectionlines;

    private Grid _parentGrid;
    private GameObject _horizontalConnectionLine;
    private GameObject _verticalConnectionLine;
    private BoxCollider _boxCollider;
    private Vector2 _subGridPosition;
    private Vector2 _altSubGridPosition;
    private bool _isPaintable = true;
    private bool _hasDot;

    public Vector3 CenterPosition
    {
        get { return _boxCollider.center;}
    }

    public ConnectionLines ConnectionLines
    {
        get { return _connectionlines; }
    }

    public BoxCollider BoxCollider
    {
        get { return _boxCollider; }
    }

    public bool HasDot
    {
        get { return _hasDot; }
    }

    public Grid ParentGrid
    {
        get { return _parentGrid; }
    }

    public bool IsPaintable
    {
        get { return _isPaintable; }
    }

    public Dot Dot
    {
        get { return _dot; }
    }

    public Vector2 SubGridPosition
    {
        get { return _subGridPosition; }
        set { _subGridPosition = value; }
    }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void SetDot(bool hasDot)
    {
        _hasDot = hasDot;
        _dot.SubGridPosition = SubGridPosition;
        _dot.ParentSubGrid = this;
        if (_hasDot != true)
        {
            transform.FindChild("Dot").gameObject.SetActive(false);
        }
        else
        {
            _isPaintable = false;
        }
    }

    public void Reset()
    {
        _isPaintable = true;
    }

    public static SubGrid Create(Vector3 position, Transform parent, bool hasDot, Grid parentGrid, Vector2 subGridPosition)
    {
        GameObject subGridGo = Instantiate(Resources.Load("Subgrid"), position, Quaternion.identity) as GameObject;
        subGridGo.transform.position = position;
        subGridGo.transform.parent = parent;
        subGridGo.name = subGridPosition.x + "|" + subGridPosition.y;
        SubGrid subGrid = subGridGo.GetComponent<SubGrid>();
        subGrid._parentGrid = parentGrid;
        subGrid.SubGridPosition = subGridPosition;
        subGrid.SetDot(hasDot);
        subGrid._connectionlines.InitConnectionTypes((int)subGridPosition.x, (int)subGridPosition.y);
        return subGrid;
    }

    public void SetConnectionLineActive(ConnectionLine line)
    {
        //if(line == ConnectionLine.horizontal) horizontalConnectionLine.SetActive(true);
        //else verticalConnectionLine.SetActive(true);
    }
}
