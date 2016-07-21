using UnityEngine;
using System.Collections;
using System.Security.Policy;

public class LevelEditor : SingletonClass<LevelEditor>
{

    private SubGrid _activeSubGrid = null;
    private SubGrid _armedSubGrid = null;

    public void RegisterSubGridHover(SubGrid subGrid, GameObject surface)
    {
        if (GameManager.instance.IsEditor == true)
        {
            if (_activeSubGrid != null)
                _activeSubGrid.Surface.SetActive(false);
            _activeSubGrid = subGrid;
            _activeSubGrid.Surface.SetActive(true);
        }
    }

    public void RegisterSubGridArmed(SubGrid subGrid, GameObject surface)
    {
        if (GameManager.instance.IsEditor == true)
        {
            if (_armedSubGrid != null)
                _armedSubGrid.ArmedSurface.SetActive(false);
            _armedSubGrid = subGrid;
            _armedSubGrid.ArmedSurface.SetActive(true);
        }
    }

    public void PlaceDot()
    {
        if (GameManager.instance.IsEditor == true)
        {
            if (_armedSubGrid != null)
            {
                _armedSubGrid.HasDot = true;
                _armedSubGrid.ToggleDot();
                GridManager.instance.CurrentLevel.SetGridDataAt(_armedSubGrid.ParentGrid.CubeSide, (int)(_armedSubGrid.SubGridPosition.x + GridManager.instance.GridSize * _armedSubGrid.SubGridPosition.y), true);
            }
        }
    }

    public void DeleteDot()
    {
        if (GameManager.instance.IsEditor == true)
        {
            if (_armedSubGrid != null)
            {
                _armedSubGrid.HasDot = false;
                _armedSubGrid.ToggleDot();
                GridManager.instance.CurrentLevel.SetGridDataAt(_armedSubGrid.ParentGrid.CubeSide, (int)(_armedSubGrid.SubGridPosition.x + GridManager.instance.GridSize * _armedSubGrid.SubGridPosition.y), false);
            }
        }
    }

    public void SaveLevel()
    {
        if (GameManager.instance.IsEditor == true)
        {
            DataManager.SaveEditorLevelData(GridManager.instance.CurrentLevel);
        }
    }

}
