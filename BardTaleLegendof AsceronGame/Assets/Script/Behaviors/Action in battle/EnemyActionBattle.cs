using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionBattle : MonoBehaviour {
	[SerializeField] private string targetsTag;
	private bool isActionStarted = false;
	public GameObject owner;
	private bool isAttack = false;
	GameObject target;
	private Vector3 targetPosition;
	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}

	GameObject FindRandomTarget() {
		GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetsTag);
		if (possibleTargets.Length > 0) {
			int targetIndex = Random.Range(0, possibleTargets.Length);
			if (possibleTargets.Length == 1) {
				targetIndex = 0;
			}
			target = possibleTargets[targetIndex];
			return target;
		}
		else {
			return null;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (BattleManager.instance.callTurn == true && BattleManager.instance.isGameOver == false && BattleManager.instance.isVictory == false) {
			if (BattleManager.instance.currentUnit.transform.position.x == this.gameObject.transform.position.x && isActionStarted == true) {
				if (isAttack == false) {
					Debug.Log(gameObject.transform.position.x);
					targetPosition = new Vector3(target.transform.position.x + 2.0f, 
					                             target.transform.position.y, target.transform.position.z);
					if (this.transform.position == targetPosition) {
						Hit();
						isAttack = true;
					}
				}
				else {
					targetPosition = startPosition;
					if (this.transform.position == targetPosition) {
						BattleManager.instance.isPlayerSelectEnemy = false;
						BattleManager.instance.isUnitAction = false;
						if (BattleManager.instance.isFirstTurn == true) {
							isAttack = false;
							isActionStarted = false;
							BattleManager.instance.FristTurn();
						}
						else {
							isAttack = false;
							isActionStarted = false;
							BattleManager.instance.nextTurn();
						}
					}
				}
				this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, 500.0f * Time.deltaTime);
			}
		}
	}

	public void Action () {
		if (BattleManager.instance.isEnemyTurn() == true) {
			target = FindRandomTarget();
			isActionStarted = true;
		}
	}

	private void Hit () {
		GenerateDamageText targetText = target.GetComponent<GenerateDamageText>();
		isActionStarted = true;
		targetText.ReceiveDamage(this.gameObject.GetComponent<EnemyStat>().enemy.baseStat.strength);
	}
}
