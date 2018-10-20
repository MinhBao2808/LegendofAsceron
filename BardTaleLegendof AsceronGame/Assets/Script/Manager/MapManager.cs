using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
	public static MapManager instance = null;
	public GameObject[] enemyInMap;

	private void Awake() {
		if (instance == null) {
            instance = this;
        }

	}
}
