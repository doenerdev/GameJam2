using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    public void BackToMainMenu()
    {
		DataManager.showLevelSelection = true;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}

