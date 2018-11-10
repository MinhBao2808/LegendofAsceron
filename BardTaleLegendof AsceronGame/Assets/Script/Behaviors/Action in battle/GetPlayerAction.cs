using UnityEngine;

public class GetPlayerAction : MonoBehaviour {
    private bool actionStarted = false;
    private Vector3 startPosition;
    public GameObject owner;
	private bool isAttack = false;
	private Vector3 targetPosition;
	private GameObject targetGameObject;

	private void Start() {
		startPosition = this.transform.position;
	}

	private void Update() {
		if (BattleManager.instance.callTurn == true && 
		    BattleManager.instance.isGameOver == false && BattleManager.instance.isVictory == false) {
			if (BattleManager.instance.currentUnit.transform.position.x == this.gameObject.transform.position.x 
			    && actionStarted == true)
            {
				//Debug.Log(this.gameObject.name);
				Debug.Log("a");
                if (isAttack == false)
                {
                    targetPosition = new Vector3(targetGameObject.transform.position.x - 2.0f, targetGameObject.transform.position.y, targetGameObject.transform.position.z);
                    if (this.transform.position == targetPosition)
                    {
                        isAttack = true;
                        Hit(targetGameObject);
                    }
                }
                else
                {
                    targetPosition = startPosition;
                    if (this.transform.position == targetPosition)
                    {
                        BattleManager.instance.isPlayerSelectEnemy = false;
                        BattleManager.instance.isUnitAction = false;
                        //StopAllCoroutines();
                        if (BattleManager.instance.isFirstTurn == true)
                        {
                            isAttack = false;
                            actionStarted = false;
                            BattleManager.instance.FristTurn();
                        }
                        else
                        {
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

	}

	public void AttackTarget (GameObject target) {
        if (BattleManager.instance.isEnemyTurn() == false) {
			targetGameObject = target;
			actionStarted = true;
			isAttack = false;
        }
    }

	//IEnumerator TimeForAction(GameObject target, Combatant targetCombatant)  {
  //      Vector3 targetPosition = new Vector3(target.transform.position.x - 2.0f, target.transform.position.y, target.transform.position.z);
  //      while (MoveTowardsTarget(targetPosition)) {
  //          yield return null;
  //      }
  //      //wait a bit
  //      yield return new WaitForSeconds(0.5f);
  //      //do damage
		//actionStarted = true;
		//Hit(target,targetCombatant);
		//yield return new WaitForSeconds(0.01f);
  //      //owner attack return to start positon
  //      Vector3 firstPosition = startPosition;
  //      while (MoveTowardsTarget(firstPosition)) {
  //          yield return null;
  //      }
		//StopCoroutine("TimeForAction");
		//yield break;
    //}

	private void Hit (GameObject target) {
		//PlayerStat ownerStat = this.owner.GetComponent<PlayerStat>();
		//PlayerStat targetStat = target.GetComponent<PlayerStat>();
		GenerateDamageText targetText = target.GetComponent<GenerateDamageText>();
		BattleManager.instance.isSelectorSpawn = false;
		targetText.ReceiveDamage(gameObject.GetComponent<PlayerStat>().player.baseStat.strength);
    }

    //private bool MoveTowardsTarget(Vector3 target) {
    //    return target != (transform.position = Vector3.MoveTowards(transform.position, target, 500.0f * Time.deltaTime));
    //}
}
