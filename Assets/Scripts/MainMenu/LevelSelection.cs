using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class LevelSelection : MonoBehaviour {

	private RectTransform rectTransform;
	public float duration = 0.5f;
	private bool isSliding = false;

	public MainMenu mainMenu;

	// position cursor to position the level stats ui
	private float positionCursor = 0.025f; // start position

	// container to spawn UI Elements in
	private Transform levelStatsContainer;
	private Transform lineContainer;

	// Do not slide beneath or over levels
	private int slideLimit = 10;
	private int currentMiddlePosition = 3;

	// references to arrow buttons in order to activate or deacivate them
	public GameObject arrowRight;
	public GameObject arrowLeft;

	// different sprites for different level stats
	public Sprite CubeDone;
	public Sprite CubeUnlocked;
	public Sprite CubeLocked;

	public Sprite PipeActive;
	public Sprite PipePassive;

	public Sprite RauteActive;
	public Sprite RautePassive;

	// hold reference to array of levels
	private LevelData[] arrLevels;

	// Use this for initialization
	public void Init() {
		
		arrLevels = DataManager.LoadLevels();	
		slideLimit = arrLevels.Length;
	}

	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Called from main script, when level selection screen gets active.
	 */
	public void Awake() {

		//Init (); // Only for Debugging, otherwise from MainMenu

		levelStatsContainer = GameObject.Find ("LevelStatsContainer").transform;
		lineContainer = GameObject.Find ("LineContainer").transform;

		// gameobject gets active
		rectTransform = GameObject.Find("MovableContent").GetComponent<RectTransform> ();

		// set arrow keys
		SetArrowButtons();

		// foreach level edit UI view
		foreach (LevelData levelData in arrLevels) {
			SetUIView (levelData);
		}
	}

	private void SetUIView(LevelData levelData)
	{
		// Compute position cursor
		// 0.25f => start position
		// + 0.75 => for every pipe
		// + 0.1 => for every cube
		positionCursor = 0.025f + 0.075f * levelData.LevelNo + 0.1f * ( levelData.LevelNo - 1 );

		// Instantiate Level_Stats_UI prefab

		GameObject levelStats = Instantiate(Resources.Load("UI/Level_Stats_UI")) as GameObject;
		levelStats.transform.SetParent(levelStatsContainer.transform, false);

		RectTransform prefabTransform = levelStats.GetComponent<RectTransform> ();

		// add 0.1 for horizontal size each left and right
		prefabTransform.anchorMin = new Vector2 (positionCursor - 0.01f, 0.25f);
		prefabTransform.anchorMax = new Vector2 (positionCursor + 0.11f, 0.75f);

		// Big number letter
		Text letter = levelStats.transform.FindChild("TextNumero").gameObject.GetComponent<Text>();
		letter.text = levelData.LevelNo.ToString ();

		// --- Level / Cube status => Unlocked, Locked or Done
		GameObject cube = levelStats.transform.FindChild("Cube").gameObject;
		Image imgCube = cube.GetComponent<Image>();
		Button btnCube = cube.GetComponent<Button> ();

		btnCube.onClick.AddListener (() => {
			//print(levelData.LevelNo);
			mainMenu.StartGame(levelData.LevelNo);
		});

		//btnCube.GetComponent<CubeClickListener> ().SetLevelNo (levelData.LevelNo);

		switch (levelData.LevelStatus) {
		case LevelStatus.Unlocked:
				imgCube.sprite = CubeUnlocked;
				btnCube.interactable = true;
				break;
			case LevelStatus.Locked:
				imgCube.sprite = CubeLocked;
				btnCube.interactable = false;
				break;
			case LevelStatus.Done:
				imgCube.sprite = CubeDone;
				btnCube.interactable = true;
				break;
		}

		// --- Pipe status => passive, if locked; active, if unlocked

		if (levelData.LevelNo > 1) {

			GameObject line = Instantiate(Resources.Load("UI/Line_UI")) as GameObject;
			line.transform.SetParent(lineContainer.transform, false);

			RectTransform lineTransform = line.GetComponent<RectTransform> ();

			// add 0.1 for horizontal size each left and right
			lineTransform.anchorMin = new Vector2 (positionCursor - 0.090f, 0.526109f);
			lineTransform.anchorMax = new Vector2 (positionCursor + 0.0125f, 0.54f);

			Image imgPipe = line.GetComponent<Image>();

			switch (levelData.LevelStatus) {
				case LevelStatus.Unlocked:
				case LevelStatus.Done:
					imgPipe.sprite = PipeActive;
					break;
				case LevelStatus.Locked:
					imgPipe.sprite = PipePassive;
					break;
			}
		}

		// --- badget => rating == 3

		Transform badget = levelStats.transform.FindChild ("Badget");
		GameObject badgetGO = badget.gameObject;

		if (levelData.Rating == 3) {
			badgetGO.SetActive (true);
		} else {
			badgetGO.SetActive (false);
		}

		// rating

		for (int i = 1; i < 4; i++) 
		{
			Image imgRaute = levelStats.transform.FindChild("Raute " + i).GetComponent<Image>();

			if (i <= levelData.Rating) {
				imgRaute.sprite = RauteActive;
			} else {
				imgRaute.sprite = RautePassive;
			}
		}
	}

	private void SetArrowButtons() {

		if (currentMiddlePosition >= slideLimit - 2) {
			arrowRight.SetActive (false);
		} else {
			arrowRight.SetActive (true);
		}

		if (currentMiddlePosition <= 3) {
			arrowLeft.SetActive (false);
		} else {
			arrowLeft.SetActive (true);
		}
	}

	public void SlideToRight()
	{
		SlideTo("right");
	}

	public void SlideToLeft()
	{
		SlideTo("left");
	}

	private void SlideTo(string direction)
	{
		if (!isSliding) {
				
			isSliding = true;

			// compute translation depending on screen size
			// if screen size is 1156 then translate about 206
			float translation = 206 * Screen.width / 1156;

			float oldPos = rectTransform.offsetMin.x;

			float newPos = oldPos;
			newPos += direction == "left" ? translation : translation * (-1) ;

			StartCoroutine (Slide (oldPos, newPos));

			currentMiddlePosition += direction == "left" ? -1 : 1;
			SetArrowButtons ();
		}
	}

	/**
	 * @param oldPos, newPos horizontal position.
	 */ 
	private IEnumerator Slide( float oldPos, float newPos )
	{
		float i = 0.0f;
		float rate = 1.0f / duration;

		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;

			float posIncrement = Mathf.SmoothStep (0.0f, 1.0f, i) * ( newPos - oldPos ) + oldPos;

			rectTransform.offsetMin = new Vector2( posIncrement, 0 );
			rectTransform.offsetMax = new Vector2( posIncrement, 0 );

			yield return null; 
		}

		isSliding = false;
	}
}


