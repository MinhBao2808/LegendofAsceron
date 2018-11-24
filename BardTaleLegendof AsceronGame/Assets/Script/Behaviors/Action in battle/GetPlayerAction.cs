using UnityEngine;

public class GetPlayerAction : MonoBehaviour {
    private bool actionStarted = false;
    private Vector3 startPosition;
	private bool isAttack = false;
	private bool isPerformSkill = false;
	private Vector3 targetPosition;
	private GameObject targetGameObject;
	private Animator animator;
	private string skillPlayerName;
	[SerializeField] private AudioSource playerSource;
	private float damage;

	private void Start() {
		animator = GetComponent<Animator>();
		startPosition = this.transform.position;
		playerSource = GetComponent<AudioSource>();
	}

	private void Update() {
		if (BattleManager.instance.callTurn == true && 
		    BattleManager.instance.isGameOver == false && BattleManager.instance.isVictory == false) {
			if (BattleManager.instance.currentUnit.transform.position.x == this.gameObject.transform.position.x 
			    && actionStarted == true)
            {
				//Debug.Log(this.gameObject.name);
                if (isAttack == false)
                {
                    targetPosition = new Vector3(targetGameObject.transform.position.x - 2.0f, targetGameObject.transform.position.y, targetGameObject.transform.position.z);
                    if (this.transform.position == targetPosition)
                    {
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

	private void SkillAnimationEnd() {
		isAttack = true;
		animator.SetBool(skillPlayerName,false);
		damage = Expression.SkillATK(this.gameObject.GetComponent<PlayerStat>().player.battleStats.patk, 3.0f,
		                              targetGameObject.GetComponent<EnemyStat>().enemy.stats.vitality,1.0f,1.0f);
		GenerateDamageText targetText = targetGameObject.GetComponent<GenerateDamageText>();
        BattleManager.instance.isSelectorSpawn = false;
		targetText.ReceiveDamage((int)damage);
		isPerformSkill = false;
	}

	public void PerformSkill (int skillIndex, GameObject target) {
		isPerformSkill = true;
		isAttack = false;
		actionStarted = true;
		skillPlayerName = this.gameObject.GetComponent<PlayerStat>().player.info.skills[skillIndex];
		this.gameObject.GetComponent<PlayerStat>().player.battleStats.mp -= this.gameObject.GetComponent<PlayerStat>().player.info.totalSkills[skillIndex].mpCost;
		Debug.Log(skillPlayerName);
		//if (this.gameObject.GetComponent<PlayerStat>().player.info.totalSkills[skillIndex].isPassive == true) {
		//	if (this.gameObject.GetComponent<PlayerStat>().player.info.type == SkillType.Buff)
		//}
		targetGameObject = target;
	}
    
	private void Hit (GameObject target) {
		if (isPerformSkill == true) {
			animator.SetBool(skillPlayerName,true);
			playerSource.clip = AudioManager.Instance.battleBgms[1];
			playerSource.Play();
		}
		else {
			isAttack = true;
			playerSource.clip = AudioManager.Instance.battleBgms[2];
            playerSource.Play();
			GenerateDamageText targetText = target.GetComponent<GenerateDamageText>();
            BattleManager.instance.isSelectorSpawn = false;
			targetText.ReceiveDamage((int)gameObject.GetComponent<PlayerStat>().player.battleStats.patk);
		}
		//isAttack = true;
    }

    //private bool MoveTowardsTarget(Vector3 target) {
    //    return target != (transform.position = Vector3.MoveTowards(transform.position, target, 500.0f * Time.deltaTime));
    //}
}
