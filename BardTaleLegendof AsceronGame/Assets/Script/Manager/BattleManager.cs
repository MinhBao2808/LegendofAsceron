using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ORKFramework;
using ORKFramework.Behaviours;
using ORKFramework.Events;

public class BattleManager : MonoBehaviour {
    public static BattleManager instance = null;
	[SerializeField] private GameObject[] playerSpawnPositions;
	[SerializeField] public GameObject[] enemySpawnPositions;
	[SerializeField] private GameObject selector;
	[SerializeField] private GameObject playerSelector;
	[SerializeField] private Slider timerSlider;
	[SerializeField] private Image[] listImageShowUnitTurn;
	[SerializeField] private GameObject actionMenu;
	[SerializeField] private GameObject attack_DefendMenu;
	//private GameObject playerSelectorSpawn;
	[SerializeField] private GameObject[] playerSelectorPositions;
	private GameObject playerSelectorSpawned;
	private Combatant[] enemyCombatant = new Combatant[6];
	private Combatant[] playerCombatant = new Combatant[3];
	private Group enemyGroup;
	private Group playerGroup;
	public List<Combatant> unitStats;
	public List<Combatant> enemyList;
	public Queue<Combatant> unitLists;
	public bool isFirstTurn;
	private List<Combatant> playerList;
    private GameObject playerParty;
    public float timer;
	public int enemyPositionIndex;
	public int enemySelectedPositionIndex;
	public bool isPlayerSelectEnemy = false;
    private float time;
	public int playerPositionIndex;
	public bool enemyTurn = false;
    private bool playerSelectAttack = false;
    private bool playerAttack = false;
	private float timeToStartBattle;
	public bool callTurn;
	public bool isUnitAction = false;
//	private float sumExpCanGet = 0.0f;
	public Combatant currentUnit;
	public bool isSelectorSpawn = false;
	public bool[] isEnemyDead = new bool[6];
	//private Vector3 selectorPositionY = new Vector3(0, 3, 0);

	public void PressActionButton () {
		actionMenu.SetActive(true);
		attack_DefendMenu.SetActive(true);
	}

	public void PressAttackButton () {
		//choose enemy
		attack_DefendMenu.SetActive(false);
		actionMenu.SetActive(false);
		isSelectorSpawn = true;
        if (isPlayerSelectEnemy == false) {
            for (int i = 0; i < enemyPositionIndex; i++) {
                if (isEnemyDead[i] == false) {
					enemySelectedPositionIndex = i;
                    Instantiate(selector,
                                enemySpawnPositions[i].transform.position,
                                enemySpawnPositions[i].transform.rotation);
                    break;
                }
            }
        }
	}

	private void SpawnEnemy () {
		enemyPositionIndex = Random.Range(1, enemySpawnPositions.Length);
		int enemyDataIndex;
		for (int i = 0; i < enemyPositionIndex; i++) {
			enemyDataIndex = Random.Range(2, 3);
			enemyCombatant[i] = ORK.Combatants.Create(enemyDataIndex, enemyGroup);
			enemyCombatant[i].Init();
			enemyCombatant[i].Spawn(enemySpawnPositions[i].transform.position, 
			                        false, enemySpawnPositions[i].transform.rotation.y, 
			                        false, enemySpawnPositions[i].transform.localScale);
		}
	}

	private void SpawnPlayer () {
		//use index to spawn player
		playerCombatant[0] = ORK.Combatants.Create(0, playerGroup);
		playerCombatant[0].Init();
		playerCombatant[0].Spawn(playerSpawnPositions[0].transform.position,
								 false, playerSpawnPositions[0].transform.rotation.y,
								 false, playerSpawnPositions[0].transform.localScale);
		playerCombatant[1] = ORK.Combatants.Create(1, playerGroup);
		playerCombatant[1].Init();
		playerCombatant[1].Spawn(playerSpawnPositions[1].transform.position,
								 false, playerSpawnPositions[1].transform.rotation.y,
								 false, playerSpawnPositions[1].transform.localScale);
		playerCombatant[2] = ORK.Combatants.Create(4, playerGroup);
		playerCombatant[2].Init();
		playerCombatant[2].Spawn(playerSpawnPositions[2].transform.position,
												   false, playerSpawnPositions[2].transform.rotation.y,
												   false, playerSpawnPositions[2].transform.localScale);
		playerPositionIndex = 3;
	}

	void Awake() {
        if (instance == null) {
            instance = this;
        }
		timeToStartBattle = 2.5f;
		callTurn = false;
		enemyGroup = new Group(1);
		playerGroup = new Group(0);
		SpawnEnemy();
		SpawnPlayer();
		//if (GameManager.instance.level == Level.Passive) {
		//	timer = 30.0f;
		//}
		//if (GameManager.instance.level == Level.Normal) {
		//	timer = 20.0f;
		//}
		//if (GameManager.instance.level == Level.Aggressive) {
		//	timer = 10.0f;
		//}
		//if (GameManager.instance.level == Level.Nightmare) {
		//	timer = 5.0f;
		//}
		//set enemy is dead = false
		for (int i = 0; i < enemyPositionIndex; i++) {
			isEnemyDead[i] = false;
		}
		timer = ORK.Difficulties.GetBattleFactor() * 20.0f;
        time = timer;
		//Debug.Log(time);
        playerAttack = false;
		actionMenu.SetActive(false);
		attack_DefendMenu.SetActive(false);
	}

    void Start() {
		//player go first
		//Debug.Log(GameManager.instance.isEnemyAttackPlayer);
		//Debug.Log(GameManager.instance.isPlayerAttackEnemy);
		enemyList = new List<Combatant>();
		playerList = new List<Combatant>();
		unitStats = new List<Combatant>();
		//unitLists = new Queue<Combatant>();
		//unitTurn = new List<int>();
		//GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
		//foreach (GameObject playerUnit in playerUnits) {
		//    PlayerStat currentStat = playerUnit.GetComponent<PlayerStat>();
		//    currentStat.CalculateNextTurn(0);
		//    unitStats.Add(currentStat);
		//    //unitTurn.Add(currentStat.nextActTurn);
		//}
		//     GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
		//     foreach (GameObject enemy in enemyUnits) {
		//         PlayerStat currentStat = enemy.GetComponent<PlayerStat>();
		//         currentStat.CalculateNextTurn(0);
		//         unitStats.Add(currentStat);
		//sumExpCanGet = sumExpCanGet + currentStat.expPoints;
		//    //unitTurn.Add(currentStat.nextActTurn);
		//}
		//unitTurn.Sort();
		for (int i = 0; i < enemyPositionIndex; i++) {
			enemyList.Add(enemyCombatant[i]);
		}
		enemyList.Sort(delegate (Combatant x, Combatant y) {
			return y.Status[9].GetValue().CompareTo(x.Status[9].GetValue());
		});
		for (int i = 0; i < playerPositionIndex; i++ ){
			playerList.Add(playerCombatant[i]);
		}
		playerList.Sort(delegate (Combatant x, Combatant y) {
			return y.Status[9].GetValue().CompareTo(x.Status[9].GetValue());
		});
		for (int i = 0; i < playerPositionIndex; i++) {
            if (playerCombatant[i].GameObject.tag == "PlayerUnit") {
                unitStats.Add(playerCombatant[i]);
            }
        }
        for (int i = 0; i < enemyPositionIndex; i++) {
            if (enemyCombatant[i].GameObject.tag == "Enemy") {
                unitStats.Add(enemyCombatant[i]);
            }
        }
        unitStats.Sort(delegate (Combatant x, Combatant y) {
            return y.Status[9].GetValue().CompareTo(x.Status[9].GetValue());
        });
        //if player go first
		if (GameManager.instance.isPlayerAttackEnemy == true) {
			int j = 0;
            foreach (Combatant player in playerList) {
                listImageShowUnitTurn[j].sprite = player.GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
            }
            foreach (Combatant enemy in enemyList) {
                listImageShowUnitTurn[j].sprite = enemy.GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
            }
            int k = 0;
            while (j < 10) {
                if (k == unitStats.Count) {
                    k = 0;
                }
                Debug.Log(k);
                listImageShowUnitTurn[j].sprite = unitStats[k].GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
                k++;
            }
		}
        //if enemy go first 
		if (GameManager.instance.isEnemyAttackPlayer == true) {
			int j = 0;
			foreach (Combatant enemy in enemyList) {
				listImageShowUnitTurn[j].sprite = enemy.GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
			}
			foreach (Combatant player in playerList) {
				listImageShowUnitTurn[j].sprite = player.GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
			}
			int k = 0;
            while (j < 10) {
                if (k == unitStats.Count) {
                    k = 0;
                }
                listImageShowUnitTurn[j].sprite = unitStats[k].GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
                k++;
            }
		}
		//this.FristTurn();
	}

	public void FristTurn () {
		callTurn = true;
		isFirstTurn = true;
        //check is game over
		GameObject[] remainEnemyUnit = GameObject.FindGameObjectsWithTag("Enemy");
		if (remainEnemyUnit.Length == 0) {
			GameManager.instance.isPlayerAttackEnemy = false;
			GameManager.instance.isEnemyAttackPlayer = false;
			GameManager.instance.isBattleSceneAnimationLoaded = false;
			ScreenManager.Instance.TriggerLoadingFadeOut(1);
		}
		GameObject[] remainPlayerUnit = GameObject.FindGameObjectsWithTag("PlayerUnit");
        if (remainPlayerUnit.Length == 0) {
            //GameManager.instance.LoadGameMenu();
        }
        //enemy go first
		if (GameManager.instance.isEnemyAttackPlayer == true) {
			int j = 0;
			foreach (Combatant enemy in enemyList) {
				listImageShowUnitTurn[j].sprite = enemy.GameObject.GetComponent<SpriteRenderer>().sprite;
				j++;
			}
			foreach (Combatant player in playerList) {
				listImageShowUnitTurn[j].sprite = player.GameObject.GetComponent<SpriteRenderer>().sprite;
				j++;
			}
			int k = 0;
			while (j < 10) {
				if (k == unitStats.Count) {
					k = 0;
				}
				listImageShowUnitTurn[j].sprite = unitStats[k].GameObject.GetComponent<SpriteRenderer>().sprite;
				j++;
				k++;
			}
            //check player turn
			if (enemyList.Count == 0) {
				actionMenu.SetActive(true);
                //go to next turn
				if (playerList.Count == 0 && enemyList.Count == 0) {
					isFirstTurn = false;
					unitLists = new Queue<Combatant>(unitStats);
					this.nextTurn();
				}
				else {
					if (isUnitAction == false) {
						currentUnit = playerList[0];
					}
					//player turn
					enemyTurn = false;
					actionMenu.SetActive(true);
					if (currentUnit.GameObject.tag == "Player") {
						//spawm player selector 
                        if (isPlayerSelectEnemy == false) {
                            if (currentUnit.GameObject.transform.position == playerSpawnPositions[0].transform.position) {
                                Instantiate(playerSelector, playerSelectorPositions[0].transform.position,
                                            playerSelectorPositions[0].transform.rotation);
                            }
                            else if (currentUnit.GameObject.transform.position == playerSpawnPositions[1].transform.position){
                                Instantiate(playerSelector, playerSelectorPositions[1].transform.position,
                                            playerSelectorPositions[1].transform.rotation);
                            }
                            else if (currentUnit.GameObject.transform.position == playerSpawnPositions[2].transform.position){
                                Instantiate(playerSelector, playerSelectorPositions[2].transform.position,
                                            playerSelectorPositions[2].transform.rotation);
                            }
                        }
                        if (isPlayerSelectEnemy == true){
                            playerList.Remove(currentUnit);
                            currentUnit.GameObject.GetComponent<GetPlayerAction>().
                                       AttackTarget(enemyCombatant[enemySelectedPositionIndex].GameObject,
                                                    enemyCombatant[enemySelectedPositionIndex]);
                        }
					}
					else {
						playerList.Remove(currentUnit);
						this.FristTurn();
					}
				}
			}
			else {
				//enemy turn
				currentUnit = enemyList[0];
				enemyList.Remove(currentUnit);
				enemyTurn = true;
				currentUnit.GameObject.GetComponent<EnemyAction>().Action();
			}
		}

        //code for player go first
		if (GameManager.instance.isPlayerAttackEnemy == true) {
			int j = 0;
            foreach (Combatant player in playerList) {
                listImageShowUnitTurn[j].sprite = player.GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
            }
            foreach (Combatant enemy in enemyList) {
                listImageShowUnitTurn[j].sprite = enemy.GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
            }
            int k = 0;
            while (j < 10) {
                if (k == unitStats.Count) {
                    k = 0;
                }
                listImageShowUnitTurn[j].sprite = unitStats[k].GameObject.GetComponent<SpriteRenderer>().sprite;
                j++;
                k++;
            }
            //check enemy turn
            if (playerList.Count == 0) {
                actionMenu.SetActive(false);
                //go to next turn
                if (playerList.Count == 0 && enemyList.Count == 0) {
                    isFirstTurn = false;
                    unitLists = new Queue<Combatant>(unitStats);
                    this.nextTurn();
                }
                //enemy turn
                else {
                    currentUnit = enemyList[0];
					if (currentUnit.GameObject.tag == "Enemy") {
						enemyList.Remove(currentUnit);
                        enemyTurn = true;
                        currentUnit.GameObject.GetComponent<EnemyAction>().Action();
					}
					else {
						enemyList.Remove(currentUnit);
						this.FristTurn();
					}
                }
            }
            else {
                if (isUnitAction == false) {
                    currentUnit = playerList[0];
                }
                //player turn
                actionMenu.SetActive(true);
                enemyTurn = false;
                //spawn player selector
                if (isPlayerSelectEnemy == false) {
                    if (currentUnit.GameObject.transform.position == playerSpawnPositions[0].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[0].transform.position,
                                    playerSelectorPositions[0].transform.rotation);
                    }
                    else if (currentUnit.GameObject.transform.position == playerSpawnPositions[1].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[1].transform.position,
                                    playerSelectorPositions[1].transform.rotation);
                    }
                    else if (currentUnit.GameObject.transform.position == playerSpawnPositions[2].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[2].transform.position,
                                    playerSelectorPositions[2].transform.rotation);
                    }
                }
                if (isPlayerSelectEnemy == true) {
                    playerList.Remove(currentUnit);
                    currentUnit.GameObject.GetComponent<GetPlayerAction>().
                               AttackTarget(enemyCombatant[enemySelectedPositionIndex].GameObject,
                                            enemyCombatant[enemySelectedPositionIndex]);
                }
            }
		} 
	}

    void Update() {
		//Debug.Log(timer);
	
		if (enemyTurn == false && callTurn == true) {
			timerSlider.value = time/timer;
			time -= Time.deltaTime;
            //Debug.Log(time);
            if (time <= 0.0f) {
				time = timer;
				if (isFirstTurn == true) {
					this.FristTurn();
				}
				else {
					this.nextTurn();
				}
            }
        }
		else {
			time = timer;
		}
		if (isPlayerSelectEnemy == true) {
			time = timer;
		}
		if (callTurn == false) {
            timeToStartBattle -= Time.deltaTime;
			Debug.Log(timeToStartBattle);
        }
		if (timeToStartBattle <= 0 && callTurn == false) {
			timeToStartBattle = 0;
			this.FristTurn();
		}
    }

    public void nextTurn() {
		isFirstTurn = false;
        playerAttack = false;
		callTurn = true;
		if (isPlayerSelectEnemy == false && isSelectorSpawn == false && isUnitAction == false) {
			GameObject[] remainEnemyUnit = GameObject.FindGameObjectsWithTag("Enemy");
            if (remainEnemyUnit.Length == 0) {
				GameManager.instance.isEnemyAttackPlayer = false;
				GameManager.instance.isPlayerAttackEnemy = false;
				GameManager.instance.isBattleSceneAnimationLoaded = false;
				ScreenManager.Instance.TriggerLoadingFadeOut(1);
            }
            GameObject[] remainPlayerUnit = GameObject.FindGameObjectsWithTag("PlayerUnit");
            if (remainPlayerUnit.Length == 0) {
				//GameManager.instance.LoadGameMenu();
            }
		}
		int j = 0;
        int k = 0;
        while (j < 10) {
            if (k == unitStats.Count) {
                k = 0;
            }
			if (unitStats[k].GameObject.tag != "DeadUnit") {
				listImageShowUnitTurn[j].sprite = unitStats[k].GameObject.GetComponent<SpriteRenderer>().sprite;
			}
            j++;
            k++;
        }
		if (isPlayerSelectEnemy == false && isSelectorSpawn == false && isUnitAction == false) {
			//currentUnit = unitStats[0];
			currentUnit = unitLists.Dequeue();
		}
		if (currentUnit.GameObject.tag != "DeadUnit") {
			if (currentUnit.GameObject.tag == "PlayerUnit") {
				//player turn
				enemyTurn = false;
				//attack_DefendMenu.SetActive(false);
				//spawn player selector
				if (isPlayerSelectEnemy == false) {
                    if (currentUnit.GameObject.transform.position == playerSpawnPositions[0].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[0].transform.position,
                                    playerSelectorPositions[0].transform.rotation);
                    }
                    else if (currentUnit.GameObject.transform.position == playerSpawnPositions[1].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[1].transform.position,
                                    playerSelectorPositions[1].transform.rotation);
                    }
                    else if (currentUnit.GameObject.transform.position == playerSpawnPositions[2].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[2].transform.position,
                                    playerSelectorPositions[2].transform.rotation);
                    }
					actionMenu.SetActive(true);
                }
				if (isPlayerSelectEnemy == true && isSelectorSpawn == true) {
					unitStats.Remove(currentUnit);
					unitStats.Add(currentUnit);
					unitLists.Enqueue(currentUnit);
					currentUnit.GameObject.GetComponent<GetPlayerAction>().
								   AttackTarget(enemyCombatant[enemySelectedPositionIndex].GameObject,
													enemyCombatant[enemySelectedPositionIndex]);
					}
				}
			else {
				if (isSelectorSpawn == false && isUnitAction == false) {
					//enemy turn
					actionMenu.SetActive(false);
					attack_DefendMenu.SetActive(false);
					isUnitAction = true;
					enemyTurn = true;
					unitStats.Remove(currentUnit);
                    unitStats.Add(currentUnit);
                    unitLists.Enqueue(currentUnit);
                    currentUnit.GameObject.GetComponent<EnemyAction>().Action();
				}
			}
		}
		else {
			actionMenu.SetActive(false);
			attack_DefendMenu.SetActive(false);
			unitStats.Remove(currentUnit);
			unitLists.Enqueue(currentUnit);
			unitStats.Add(currentUnit);
			this.nextTurn();
		}
    }

    public bool isEnemyTurn() {
        return enemyTurn;
    }

    public void SetPlayerSelectAttack() {//player choose attack enemy 
        playerSelectAttack = true;
    }
}
