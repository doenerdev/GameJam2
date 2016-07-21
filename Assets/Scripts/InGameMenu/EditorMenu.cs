using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EditorMenu : MonoBehaviour {

    public void BackToMainMenu()
    {
        GameManager.instance.IsEditor = false;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}

