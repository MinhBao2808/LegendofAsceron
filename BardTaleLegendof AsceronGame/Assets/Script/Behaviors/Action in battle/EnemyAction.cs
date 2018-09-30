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

	void Start() {
        startPosition = transform.position;	
	}

	GameObject FindRandomTarget() {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetsTag);
        if (possibleTargets.Length > 0) {
            int targetIndex = Random.Range(0, possibleTargets.Length);
            GameObject target = possibleTargets[targetIndex];
            return target;
        }
        else {
            return null;
        }
    }

    public void Action () {
        GameObject target = FindRandomTarget();
        if (BattleManager.instance.isEnemyTurn() == true) {
            StartCoroutine(TimeForAction(target));
        }
    }


    IEnumerator TimeForAction(GameObject target) {
        Vector3 targetPosition = new Vector3(target.transform.position.x + 1.0f, target.transform.position.y, target.transform.position.z);
		Debug.Log(targetPosition);
        while (MoveTowardsTarget(targetPosition)) {
            yield return null;
        }
        //wait a bit
        yield return new WaitForSeconds(0.5f);
        //do damage
        actionStarted = true;
        Hit(target);
        //owner attack return to start positon
        Vector3 firstPosition = startPosition;
        while (MoveTowardsTarget(firstPosition)) {
            yield return null;
        }
        if (actionStarted == true) {
            GameObject turnSystem = GameObject.Find("BattleManager");
			if (BattleManager.instance.isFirstTurn == true) {
				turnSystem.GetComponent<BattleManager>().FristTurn();
			}
			else {
				turnSystem.GetComponent<BattleManager>().nextTurn();
			}
        }
    }

    private void Hit(GameObject target) {
        //PlayerStat ownerStat = this.owner.GetComponent<PlayerStat>();
        //PlayerStat targetStat = target.GetComponent<PlayerStat>();
		GenerateDamageText targetText = target.GetComponent<GenerateDamageText>();
        CombatantComponent combatantComponent = gameObject.GetComponent<CombatantComponent>();
        Combatant combatant = combatantComponent.combatant;
        targetText.ReceiveDamage(combatant.Status[4].GetValue());
        //targetStat.ReceiveDamage(ownerStat.attack);
    }

    private bool MoveTowardsTarget(Vector3 target) {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, 100.0f * Time.deltaTime));
    }
}
