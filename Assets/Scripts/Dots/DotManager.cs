using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotManager : SingletonClass<DotManager>
{

    private Dot _activeDot = null;
    private int _qtyConnectedDots = 0;
    private int _qtyRevertedSteps = 0;
    private int _maxRevertSteps = 3;
    private int _rating = 2;
    private List<Connection> _connections = new List<Connection>();
    private List<Dot> _activeDots = new List<Dot>();
    private float _timer = 0f;

    public Dot ActiveDot
    {
        get { return _activeDot; }
    }

    public float Timer
    {
        get { return _timer; }
    }

    public int Rating
    {
        get { return _rating; }
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
                _qtyConnectedDots++;
                AudioManager.instance.PlayDotConnectedSound();
            }
        }

        Debug.Log(_qtyConnectedDots + "  " + GridManager.instance.CurrentLevel.QtyDots);
        
        if (_qtyConnectedDots == GridManager.instance.CurrentLevel.QtyDots -1)
        {
            InGameMenu.instance.ShowLevelComplete(_rating);
        }
    }

    public void RevertLastStep()
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

            InGameMenu.instance.DisableRevertStep(_qtyRevertedSteps);

            _qtyConnectedDots--;
            _qtyRevertedSteps++;

            if(_qtyRevertedSteps >= _maxRevertSteps)
                InGameMenu.instance.DisableRevertButton();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            RevertLastStep();
        }

        _timer += Time.deltaTime;
    }

}
