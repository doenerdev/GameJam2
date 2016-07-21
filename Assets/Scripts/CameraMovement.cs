using UnityEngine;
using System.Collections;

/**
 * Responsible for rotating the camera via arrow keys or tracking input.
 */
public class CameraMovement : MonoBehaviour 
{
	[SerializeField]
	public UDPReceive receiver = null;

	// because the received values of the face tracking aren't in correct dimensions, scale some of them
	public float scalePitch = 0;
	public float scaleHorizontalMovement = 0;

	// whether to show tracking information
	public bool showTrackingData = false;
	public bool showTransformData = false;

	// for debugging purposes include keyboard control via arrow keys for rotating camera
	private float rotationX = 0;
	private float rotationY = 0;
	public bool pfeiltasten = false;

	// which projection type to use
	public bool parallelProjection = false;

	// instantiate new rotation only once, not every frame
	private Vector3 eulerAngles;

	// hold references to the rotating objects 
	private Transform mainCamera;
	private Transform cameraRig;

	public void Start() 
	{
		mainCamera = this.transform;
		cameraRig = mainCamera.parent;
	}

	/**
	 * Compute rotation.
	 */
	void Update() 
	{
		// set projection type of camera
		GetComponent<Camera>().orthographic = parallelProjection;

		// rotate
		eulerAngles = Vector3.zero;

		if(pfeiltasten) 
		{
			rotationY += 2 * Input.GetAxis ("Horizontal"); 
			rotationY = Mathf.Clamp(rotationY, -60, 60);

			rotationX += (-2) * Input.GetAxis ("Vertical"); 
			rotationX = Mathf.Clamp(rotationX, -60, 60);

			eulerAngles.x = rotationX;
			eulerAngles.y = rotationY;
		} 
		else 
		{
			eulerAngles.x = receiver.pitch * scalePitch;
			eulerAngles.y = receiver.xPos * scaleHorizontalMovement;
		}

		cameraRig.localEulerAngles = eulerAngles;
	}

	/**
	 * Show tracking information and current position / rotation of the gameobject.
	 */ 
	void OnGUI()
	{
		if ( showTrackingData || showTransformData ) 
		{
			Rect rectObj = new Rect(40, 10, 200, 800);
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.UpperLeft;

			string output = "";

			if ( showTrackingData ) 
			{
				output += "Tracking Data:" + "\n";

				output += "xPos: " + receiver.xPos + "\n";
				output += "yPos: " + receiver.yPos + "\n";
				output += "zPos: " + receiver.zPos + "\n";
				output += "pitch: " + receiver.pitch + "\n";
				output += "yaw: " + receiver.yaw + "\n";
				output += "roll: " + receiver.roll + "\n";

				output += "\n\n";
			}


			if( showTransformData ) 
			{
				output += "Transform Data:" + "\n";

				output += "xPos: " + transform.position.x + "\n" ;
				output += "yPos: " + transform.position.y + "\n" ;
				output += "zPos: " + transform.position.z + "\n" ;
				output += "xRot: " + transform.eulerAngles.x + "\n";
				output += "yRot: " + transform.eulerAngles.y + "\n";
				output += "zRot: " + transform.eulerAngles.z + "\n";
			}

			GUI.Box(rectObj, output, style);
		}
	}
}
