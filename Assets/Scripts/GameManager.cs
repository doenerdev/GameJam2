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
		LevelData data = DataManager.GetLevel (DataManager.CurrentLevel);
        GridManager.instance.GenerateLevel(data);
    }
}
