using UnityEngine;
using System.Collections;

public class LevelEditor : SingletonClass<LevelEditor>
{

    private SubGrid _activeSubGrid = null;
    private SubGrid _armedSubGrid = null;

    public void RegisterSubGridHover(SubGrid subGrid, GameObject surface)
    {
        if(_activeSubGrid != null)
            _activeSubGrid.Surface.SetActive(false);
        _activeSubGrid = subGrid;
        _activeSubGrid.Surface.SetActive(true);
    }

    public void RegisterSubGridArmed(SubGrid subGrid, GameObject surface)
    {
        if(_armedSubGrid != null)
            _armedSubGrid.ArmedSurface.SetActive(false);
        _armedSubGrid = subGrid;
        _armedSubGrid.ArmedSurface.SetActive(true);
    }

    public void PlaceDot()
    {
        if (_armedSubGrid != null)
        {
            _armedSubGrid.HasDot = false;
            _armedSubGrid.ToggleDot();
        }
    }

    public void DeleteDot()
    {
        if (_armedSubGrid != null)
        {
            _armedSubGrid.HasDot = true;
            _armedSubGrid.ToggleDot();
        }
    }

    public void SaveLevel()
    {
        DataManager.SaveEditorLevelData(GridManager.instance.CurrentLevel);
    }

}
