using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ORKFramework;
using ORKFramework.Behaviours;

public class EnemyAction : MonoBehaviour {
    [SerializeField] private string targetsTag;
    private bool actionStarted = false;
    private Vector3 startPosition;
    public GameObject owner;
	private bool isAttack = false;
	GameObject target;
	private Vector3 targetPosition;

	void Start() {
        startPosition = transform.position;	
	}

	GameObject FindRandomTarget() {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetsTag);
        if (possibleTargets.Length > 0) {
            int targetIndex = Random.Range(0, possibleTargets.Length);
            target = possibleTargets[targetIndex];
            return target;
        }
        else {
            return null;
        }
    }

	private void Update() {
		if (BattleManager.instance.currentUnit.GameObject.name == this.gameObject.name && actionStarted == true) {
			if(isAttack == false) {
				targetPosition = new Vector3(target.transform.position.x + 2.0f, target.transform.position.y, target.transform.position.z);
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
                    //StopAllCoroutines();
                    if (BattleManager.instance.isFirstTurn == true) {
                        isAttack = false;
                        actionStarted = false;
                        BattleManager.instance.FristTurn();

                    }
                    else {
                        //BattleManager.instance.unitLists.Enqueue(BattleManager.instance.currentUnit);
                        isAttack = false;
                        actionStarted = false;
                        BattleManager.instance.nextTurn();
                    }
                }
			}
			this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, 500.0f * Time.deltaTime);
		}
	}

	public void Action () {
        if (BattleManager.instance.isEnemyTurn() == true) {
			target = FindRandomTarget();
			actionStarted = true;
            //StartCoroutine(TimeForAction(target));
        }
    }


  //  IEnumerator TimeForAction(GameObject target) {
  //      Vector3 targetPosition = new Vector3(target.transform.position.x + 1.0f, target.transform.position.y, target.transform.position.z);
  //      while (MoveTowardsTarget(targetPosition)) {
  //          yield return null;
  //      }
  //      //wait a bit
  //      yield return new WaitForSeconds(0.5f);
  //      //do damage
		//actionStarted = true;
  //      Hit(target);
  //      //owner attack return to start positon
  //      Vector3 firstPosition = startPosition;
  //      while (MoveTowardsTarget(firstPosition)) {
  //          yield return null;
  //      }
		//StopCoroutine("TimeForAction");
		//yield break;
    //}

    private void Hit() {
        //PlayerStat ownerStat = this.owner.GetComponent<PlayerStat>();
        //PlayerStat targetStat = target.GetComponent<PlayerStat>();
		GenerateDamageText targetText = target.GetComponent<GenerateDamageText>();
        CombatantComponent combatantComponent = gameObject.GetComponent<CombatantComponent>();
        Combatant combatant = combatantComponent.combatant;
        targetText.ReceiveDamage(combatant.Status[4].GetValue());
		actionStarted = true;
        //targetStat.ReceiveDamage(ownerStat.attack);
    }

    //private bool MoveTowardsTarget(Vector3 target) {
    //    return target != (transform.position = Vector3.MoveTowards(transform.position, target, 450.0f * Time.deltaTime));
    //}
}
