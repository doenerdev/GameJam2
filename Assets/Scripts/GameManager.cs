using UnityEngine;
using System.Collections;
using System.IO;

public class GameManager : SingletonClass<GameManager>
{
    private void Awake()
    {
        
    }

    private void Start()
    {
		DataManager.instance.LoadLevels ();
		LevelData data = DataManager.instance.GetLevel (1);
        GridManager.instance.GenerateLevel(data);
    }
}
