using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorMovement : MonoBehaviour {
	private int selectorCurrentPosition;
	//// Use this for initialization
	void Start () {
		selectorCurrentPosition = BattleManager.instance.enemySelectedPositionIndex;
		//selectorCurrentPosition = 0;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Return) && BattleManager.instance.isEnemyDead[selectorCurrentPosition] == false) {
			Destroy(this.gameObject);
		}
		if (BattleManager.instance.isPlayerSelectEnemy == false) {
			if (transform.position == BattleManager.instance.enemySpawnPositions[0].transform.position && BattleManager.instance.enemyPositionIndex >= 1) {
				if (Input.GetKeyDown(KeyCode.DownArrow) && BattleManager.instance.enemyPositionIndex >= 3) {
					transform.position = BattleManager.instance.enemySpawnPositions[2].transform.position;
					selectorCurrentPosition = 2;
				}
				if (Input.GetKeyDown(KeyCode.RightArrow) && BattleManager.instance.enemyPositionIndex >= 5) {
					transform.position = BattleManager.instance.enemySpawnPositions[4].transform.position;
					selectorCurrentPosition = 4;
				}
				if (Input.GetKeyDown(KeyCode.UpArrow) && BattleManager.instance.enemyPositionIndex >= 2) {
					transform.position = BattleManager.instance.enemySpawnPositions[1].transform.position;
					selectorCurrentPosition = 1;
				}
			}
			if (transform.position == BattleManager.instance.enemySpawnPositions[1].transform.position && BattleManager.instance.enemyPositionIndex >= 2) {
				if (Input.GetKeyDown(KeyCode.DownArrow)) {
					transform.position = BattleManager.instance.enemySpawnPositions[0].transform.position;
					selectorCurrentPosition = 0;
				}
				if (Input.GetKeyDown(KeyCode.RightArrow) && BattleManager.instance.enemyPositionIndex >= 4) {
					transform.position = BattleManager.instance.enemySpawnPositions[3].transform.position;
					selectorCurrentPosition = 3;
				}
			}
			if (transform.position == BattleManager.instance.enemySpawnPositions[2].transform.position && BattleManager.instance.enemyPositionIndex >= 3) {
				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					transform.position = BattleManager.instance.enemySpawnPositions[0].transform.position;
					selectorCurrentPosition = 0;
				}
				if (Input.GetKeyDown(KeyCode.RightArrow) && BattleManager.instance.enemyPositionIndex >= 6) {
					transform.position = BattleManager.instance.enemySpawnPositions[5].transform.position;
					selectorCurrentPosition = 5;
				}
			}
			if (transform.position == BattleManager.instance.enemySpawnPositions[3].transform.position && BattleManager.instance.enemyPositionIndex >= 4) {
				if (Input.GetKeyDown(KeyCode.DownArrow) && BattleManager.instance.enemyPositionIndex >= 5) {
					transform.position = BattleManager.instance.enemySpawnPositions[4].transform.position;
					selectorCurrentPosition = 4;
				}
				if (Input.GetKeyDown(KeyCode.LeftArrow)) {
					transform.position = BattleManager.instance.enemySpawnPositions[1].transform.position;
					selectorCurrentPosition = 1;
				}
			}
			if (transform.position == BattleManager.instance.enemySpawnPositions[4].transform.position && BattleManager.instance.enemyPositionIndex >= 5) {
				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					transform.position = BattleManager.instance.enemySpawnPositions[3].transform.position;
					selectorCurrentPosition = 3;
				}
				if (Input.GetKeyDown(KeyCode.DownArrow) && BattleManager.instance.enemyPositionIndex >= 6) {
					transform.position = BattleManager.instance.enemySpawnPositions[5].transform.position;
					selectorCurrentPosition = 5;
				}
				if (Input.GetKeyDown(KeyCode.LeftArrow)) {
					transform.position = BattleManager.instance.enemySpawnPositions[0].transform.position;
					selectorCurrentPosition = 0;
				}
			}
			if (transform.position == BattleManager.instance.enemySpawnPositions[5].transform.position && BattleManager.instance.enemyPositionIndex >= 6) {
				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					transform.position = BattleManager.instance.enemySpawnPositions[4].transform.position;
					selectorCurrentPosition = 4;
				}
				if (Input.GetKeyDown(KeyCode.LeftArrow)) {
					transform.position = BattleManager.instance.enemySpawnPositions[2].transform.position;
					selectorCurrentPosition = 2;
				}
			}

		}
	}

	private void OnDestroy() {
		BattleManager.instance.enemySelectedPositionIndex = selectorCurrentPosition;
        BattleManager.instance.isPlayerSelectEnemy = true;
		BattleManager.instance.isSelectorSpawn = true;
        if (BattleManager.instance.isFirstTurn == true) {
            BattleManager.instance.FristTurn();
        }
        else {
            BattleManager.instance.nextTurn();
        }
	}
}
