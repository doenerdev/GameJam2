using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System;

public class MainMenu : MonoBehaviour {

    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject menuBackground;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject optionsOverlay;
    [SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject levelSelectionPanel;
	[SerializeField] private GameObject creditsOverlay;
    [SerializeField] private AudioMixer mainMixer;

    private static MainMenu _instance = null;
    private bool isPaused = false;
	public AudioClip mouseOverSound;
	public AudioClip backgMusic;
	public AudioSource audioMusic;
	public AudioSource audioSFX;

	private LevelSelection levelSelection;

    public static MainMenu instance
    {
        get
        {
            if (_instance = null)
            {
                CreateMenu();
                return _instance;
            }
            else
            {
                return _instance;
            }
        }
    }

    private static void CreateMenu()
    {
        MainMenu[] mainMenuObjects = FindObjectsOfType(typeof(MainMenu)) as MainMenu[];

        if (_instance == null)
        {
            if (mainMenuObjects.Length > 1)
            {
                Debug.LogError("There are multiple instances of this singleton in the scene!");
            }
            else if (mainMenuObjects.Length == 1)
            {
                Debug.Log("An already existing singelton instance was found on initialization!s");
                _instance = mainMenuObjects[0];
            }
            else
            {
                GameObject singletonGO = Instantiate(Resources.Load("MainMenu")) as GameObject;
                singletonGO.name = "MainMenu";
                _instance = singletonGO.AddComponent(typeof(MainMenu)) as MainMenu;
            }
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(eventSystem);
		audioMusic.PlayOneShot (backgMusic, 0.7F);

		levelSelection = levelSelectionPanel.GetComponent<LevelSelection> ();
    }

	//void OnMouseOver()
	//{
	//	audio.PlayOneShot (mouseOverSound, 1);
	//}

	public void OnSelect(BaseEventData eventData){
		audioSFX.PlayOneShot (mouseOverSound, 0.7F);
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            PauseGame();
        }
    }

	//Level-Selektion
	public void ShowLevelSelectionPanel()
	{
		levelSelectionPanel.SetActive(true);
		levelSelection.Init ();
	}

	public void HideLevelSelectionPanel()
	{
		levelSelectionPanel.SetActive(false);
	}

    public void ShowOptionsPanel()
    {
        Debug.Log("Show");
        optionsPanel.SetActive(true);
        optionsOverlay.SetActive(true);
    }

    public void HideOptionsPanel()
    {
        optionsPanel.SetActive(false);
        optionsOverlay.SetActive(false);
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        optionsOverlay.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        optionsOverlay.SetActive(false);
    }

	//Credits anzeigen
	public void ShowCreditsPanel()
	{
		creditsOverlay.SetActive(true);
	}

	//Credits ausschlaten
	public void HideCreditsPanel()
	{
		creditsOverlay.SetActive(false);
	}


    public void PauseGame()
    {
        ShowPausePanel();
        Time.timeScale = 0;
        isPaused = true;
    }

    public void UnPauseGame()
    {
        HidePausePanel();
        Time.timeScale = 1;
        isPaused = false;
    }

	//Steuert Musik-Lautstärke
    public void SetMusicLevel(float musicLvl)
    {
		mainMixer.SetFloat("musicVol", Mathf.Log10(musicLvl <= 0 ? 0.001f : musicLvl)*40f);

    }

	//Steuert SFX-Lautstärke
	public void SetSFXLevel(float sfxLvl)
	{
		mainMixer.SetFloat("sfxVol", Mathf.Log10(sfxLvl <= 0 ? 0.001f : sfxLvl)*40f);
	}

	public void StartGame(int levelNo)
    {
        menuBackground.SetActive(false);
        menuPanel.SetActive(false);
        SceneManager.LoadScene("FaceTrackingTest01");
    }

    public void QuitGame()
    {
        #if UNITY_STANDALONE
                Application.Quit();
        #endif

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
