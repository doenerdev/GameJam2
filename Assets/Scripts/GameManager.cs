using UnityEngine;
using System.Collections;
using System.IO;

public class GameManager : SingletonClass<GameManager>
{

    [SerializeField] private bool isEditor = false;

    public bool IsEditor
    {
        get { return isEditor; }
        set { isEditor = value; }
    }

    private void Start()
    {
        if (isEditor == false)
        {
            LevelData data = DataManager.GetLevel(DataManager.CurrentLevel);
            GridManager.instance.GenerateLevel(data);
        }
        else
        {
            LevelData data = new LevelData();
            data.Init(3);
            GridManager.instance.GenerateLevel(data);
        }
    }
}
