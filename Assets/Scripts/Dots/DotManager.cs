using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotManager : SingletonClass<DotManager>
{

    private ushort _qtyDots = 0;
    private Dot _activeDot = null;
    private int _qtyConnectedDots = 0;
    private int _qtyRevertedSteps = 0;
    private int _maxRevertSteps = 3;
    private List<Connection> _connections = new List<Connection>();
    private List<Dot> _activeDots = new List<Dot>();

    public Dot ActiveDot
    {
        get { return _activeDot; }
    }

    public ushort QtyDots
    {
        get { return _qtyDots; }
    }

    public void RegisterClickedDot(Dot dot)
    {
        if (_activeDot == null)
        {
            _activeDot = dot;
            dot.SetDotState(DotState.active);
            _activeDots.Add(_activeDot);
            AudioManager.instance.PlayDotConnectedSound();
        }
        else
        {
            //Debug.Log(_activeDot.IsValidConnectionTo(dot));
            var connection = new Connection();
            if (_activeDot.IsValidConnectionTo(dot, connection))
            {
                _activeDot.SetDotState(DotState.connected);
                _activeDot = dot;
                dot.SetDotState(DotState.active);
                _connections.Add(connection);
                _activeDots.Add(_activeDot);
                _connections[_connections.Count -1 ].EnableConnection();
                _qtyConnectedDots++; //-----> check for win condition!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                AudioManager.instance.PlayDotConnectedSound();
            }
        } 
    }

    private void RevertLastStep()
    {
        if (_qtyRevertedSteps < _maxRevertSteps && _activeDots.Count > 0)
        {
            _activeDots[_activeDots.Count - 1].SetDotState(DotState.normal);
            _activeDots[_activeDots.Count - 1].ParentSubGrid.Reset();
            _activeDots.RemoveAt(_activeDots.Count - 1);

            if (_connections.Count > 0)
            {
                _connections[_connections.Count - 1].DisableConnection();
                _connections.RemoveAt(_connections.Count - 1);
            }

            if (_activeDots.Count > 1)
            {
                _activeDots[_activeDots.Count - 1].SetDotState(DotState.active);
                _activeDot = _activeDots[_activeDots.Count - 1];
            }
            else
            {
                Debug.Log("SetNull");
                _activeDot = null;
            }

            _qtyConnectedDots--;
            _qtyRevertedSteps++;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            RevertLastStep();
        }
    }

}
