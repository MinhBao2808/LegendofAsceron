using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMapSetup : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Camera.main.GetComponent<Jims.InputSystem.Jims_CameraController>().TargetToFollow 
              = PlayerManager.Instance.player.transform;
	}
}
