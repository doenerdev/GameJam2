using UnityEngine;
using System.Collections;

public class LevelAuswahl : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		DataManager.instance.LoadLevels();	
	}

	// Update is called once per frame
	void Update () {
		
	}
}
