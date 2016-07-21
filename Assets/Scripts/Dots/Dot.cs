using UnityEngine;
using System.Collections;

public enum DotState
{
    normal,
    active,
    connected
}

public abstract class Dot : MonoBehaviour
{
    protected DotState _dotState = DotState.normal;
    protected Vector2 _subGridPosition = new Vector2();
    protected SubGrid _parentSubGrid;
    protected IValidConnectionTest _validationTestBehaviour;
    protected Color _normalColor;

    public DotState DotState
    {
        get { return _dotState; }
    }

    public Vector2 SubGridPosition
    {
        get { return _subGridPosition; }
        set { _subGridPosition = value; }
    }

    public SubGrid ParentSubGrid
    {
        get { return _parentSubGrid; }
        set { _parentSubGrid = value; }
    }

    public void OnMouseDown()
    {
        DotManager.instance.RegisterClickedDot(this);
    }

    public abstract bool IsValidConnectionTo(Dot dot, Connection connection);
    public abstract void SetDotState(DotState state);

}
