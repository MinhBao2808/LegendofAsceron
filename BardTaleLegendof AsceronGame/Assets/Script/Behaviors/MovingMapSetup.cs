using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMapSetup : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Camera.main.GetComponent<RTS_Cam.RTS_Camera>().targetFollow
             = PlayerManager.Instance.player.transform;
	}
}
