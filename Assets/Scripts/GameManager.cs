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
		print (DataManager.CurrentLevel);
		LevelData data = DataManager.GetLevel (1);
        GridManager.instance.GenerateLevel(data);
    }
}
