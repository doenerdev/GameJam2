using UnityEngine;
using System.Collections;

/**
 * Some script to perform operations on the cube, e.g. rotating it.
 */ 
public class CubeRotation : MonoBehaviour {

	// whether to show instructions or not and use "R" and "L" to rotate cube
	public bool debug = false;

	// time of rotation animation
	public float duration = 0.75f; // in seconds

	// store rotation value
	private bool isRotating = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (debug) 
		{
			if (Input.GetKeyDown(KeyCode.P)) {
				RotateCube("up");
			}

			if (Input.GetKeyDown(KeyCode.L)) {
				RotateCube("left");
			}

			if (Input.GetKeyDown(KeyCode.BackQuote)) { // Ö
				RotateCube("down");
			}

			if (Input.GetKeyDown(KeyCode.Quote)) { // Ä
				RotateCube("right");
			}
		}
	}

	/**
	 * When dot is clicked and valid, rotate to according side.
	 */
	public void RotateTowards(CubeSide cubeSide)
	{
		// first, get the vector showing to the direction of the cubeSide to rotate to

		Vector3 vectorFrom = Vector3.zero;

		switch (cubeSide) {
			case CubeSide.front:
				vectorFrom = transform.forward * (-1); // weird, but front is back
				break;
			case CubeSide.left:
				vectorFrom = transform.right * (-1);
				break;
			case CubeSide.right:
				vectorFrom = transform.right;
				break;
			case CubeSide.top:
				vectorFrom = transform.up;
				break;
			case CubeSide.bottom:
				vectorFrom = transform.up * (-1);
				break;
			case CubeSide.back:
				vectorFrom = transform.forward;
				break;
		}

		// second, compute dot product with all possible sides
		// 1 => direction found, 0 => no alignment

		float dotProduct = Mathf.Infinity;
		string rotateDirection = "none";
		bool sideFound = false;

		for (int i = 0; i < 6; i++) 
		{
			Vector3 possibleSide = Vector3.zero;

			switch (i) 
			{
				case 0: // up
					possibleSide = Vector3.up;
					rotateDirection = "up";
					break;
				case 1: // down
					possibleSide = Vector3.down;
					rotateDirection = "down";
					break;
				case 2: // left
					possibleSide = Vector3.left;
					rotateDirection = "left";
					break;
				case 3: // right
					possibleSide = Vector3.right;
					rotateDirection = "right";
					break;
			}

			dotProduct = (int) Vector3.Dot (possibleSide, vectorFrom);

			if (dotProduct == 1) { 
				sideFound = true;
				break;
			}
		}

		if (sideFound) {
			RotateCube (rotateDirection);
		}
	}

	/**
	 * Intermediate function to provide control from outside the script.
	 * @param string direction of the rotation, either up, down, left, right
	 */
	public void RotateCube(string direction) {

		if (isRotating)
			return;

		isRotating = true;

		Vector3 rotationAxis = Vector3.back;
		switch (direction) {
			case "up":
				rotationAxis = Vector3.left;
				break;
			case "down":
				rotationAxis = Vector3.right;
				break;
			case "left":
				rotationAxis = Vector3.down;
				break;
			case "right":
				rotationAxis = Vector3.up;
				break;
		}

		Quaternion oldRot = transform.rotation;
		transform.Rotate (rotationAxis * 90, Space.World);
		Quaternion quaternionTo = transform.rotation;

		StartCoroutine(PerformRotation(oldRot, quaternionTo));
	}

	/**
	 * Start this function using a coroutine or via RotateCube()
	 * http://answers.unity3d.com/questions/28977/how-to-ease-a-rotation.html
	 */
	private IEnumerator PerformRotation(Quaternion startRot, Quaternion endRot)
	{
		float i = 0.0f;
		float rate = 1.0f / duration;

		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			transform.rotation = Quaternion.Slerp(startRot, endRot, Mathf.SmoothStep(0.0f, 1.0f, i));
			yield return null; 
		}

		isRotating = false;
	}

	/**
	 * Show textbox that says the player has to hit "R" for rotating cube
	 * and listen for key event.
	 */ 
	void OnGUI()
	{
		if (debug) 
		{		
			// --- print textbox

			Rect rectObj = new Rect (40, 10, 200, 200);
			GUIStyle style = new GUIStyle ();
			style.alignment = TextAnchor.UpperLeft;

			string output = "Drehe den Würfel mit P, L, Ö, Ä.";

			GUI.Box (rectObj, output, style);
		}
	}
}
