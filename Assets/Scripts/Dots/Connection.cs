using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connection : MonoBehaviour
{

    private List<GameObject> _connection = new List<GameObject>();
    private List<SubGrid> _subgrids = new List<SubGrid>();

    public void AddPartToConnection(GameObject partGo, SubGrid subgrid)
    {
        _connection.Add(partGo);
        _subgrids.Add(subgrid);
    }

    public void DisableConnection()
    {
        foreach (var connectionPart in _connection)
        {
            connectionPart.SetActive(false);
            var subgrid = connectionPart.gameObject.transform.parent.parent.GetComponent<SubGrid>();
            if(subgrid.HasDot == false)
                subgrid.Reset();
        }
    }

    public void EnableConnection()
    {
        foreach (var connectionPart in _connection)
        {
            connectionPart.SetActive(true);
        }
    }
}
