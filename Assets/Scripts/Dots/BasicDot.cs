using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class BasicDot : Dot
{
    private void Awake()
    {
        _validationTestBehaviour = new ValidConnectionBasic();
        _normalColor = this.gameObject.GetComponent<Renderer>().material.color;
    }

    public override bool IsValidConnectionTo(Dot dot, Connection connection)
    {
        return _validationTestBehaviour.IsValidConnection(this, dot, connection);
    }

    public override void SetDotState(DotState state)
    {
        _dotState = state;
        if (_dotState == DotState.active)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;

			// whenever dot state changes to active, rotate cube towards active dot
			CubeRotation cubeRotation = GameObject.Find ("Cube").GetComponent<CubeRotation> ();
			CubeSide cubeSide = this.ParentSubGrid.ParentGrid.CubeSide;
			cubeRotation.RotateTowards(cubeSide);
        }
        else if (_dotState == DotState.connected)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (_dotState == DotState.normal)
        {
            this.gameObject.GetComponent<Renderer>().material.color = _normalColor;
        }

    }
}
