using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ORKFramework;

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
	[SerializeField] private GameObject[] enemiesUnit;
	[SerializeField] private GameObject[] playerUnit;
	//private GameObject playerSelectorSpawn;
	[SerializeField] private GameObject[] playerSelectorPositions;
	private GameObject playerSelectorSpawned;
	//private GameObject[] enemyGameObject = new GameObject[6];
	//private GameObject[] playerGameObject = new GameObject[3];
	public List<GameObject> unitStats;
	public List<GameObject> enemyList;
	public Queue<GameObject> unitLists;
	public bool isFirstTurn;
	public List<GameObject> playerList;
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
	public bool isTurnSkip;
	public bool callTurn;
	public bool isUnitAction = false;
//	private float sumExpCanGet = 0.0f;
	public GameObject currentUnit;
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
		//enemyPositionIndex = Random.Range(1, enemySpawnPositions.Length);
		//int enemyDataIndex;
		//for (int i = 0; i < enemyPositionIndex; i++) {
		//	enemyDataIndex = Random.Range(2, 3);
		//	enemyGameObject[i] = ORK.GameObjects.Create(enemyDataIndex, enemyGroup);
		//	enemyGameObject[i].Init();
		//	enemyGameObject[i].Spawn(enemySpawnPositions[i].transform.position, 
		//	                        false, enemySpawnPositions[i].transform.rotation.y, 
		//	                        false, enemySpawnPositions[i].transform.localScale);
		//}
		enemyPositionIndex = Random.Range(1, enemySpawnPositions.Length);
		int enemyDataIndex;
		for (int i = 0; i < enemyPositionIndex; i++) {
			enemyDataIndex = Random.Range(0, 1);
			enemiesUnit[enemyDataIndex].GetComponent<EnemyStat>().Init(enemyDataIndex);
			Instantiate(enemiesUnit[enemyDataIndex], enemySpawnPositions[i].transform.position, enemySpawnPositions[i].transform.rotation);
		}
	}

	private void SpawnPlayer () {
		//use index to spawn player
		//playerGameObject[0] = ORK.GameObjects.Create(0, playerGroup);
		//playerGameObject[0].Init();
		//playerGameObject[0].Spawn(playerSpawnPositions[0].transform.position,
		//						 false, playerSpawnPositions[0].transform.rotation.y,
		//						 false, playerSpawnPositions[0].transform.localScale);
		//playerGameObject[1] = ORK.GameObjects.Create(1, playerGroup);
		//playerGameObject[1].Init();
		//playerGameObject[1].Spawn(playerSpawnPositions[1].transform.position,
		//						 false, playerSpawnPositions[1].transform.rotation.y,
		//						 false, playerSpawnPositions[1].transform.localScale);
		//playerGameObject[2] = ORK.GameObjects.Create(4, playerGroup);
		//playerGameObject[2].Init();
		//playerGameObject[2].Spawn(playerSpawnPositions[2].transform.position,
		//										   false, playerSpawnPositions[2].transform.rotation.y,
		//										   false, playerSpawnPositions[2].transform.localScale);
		//playerPositionIndex = 3;
		playerUnit[0].GetComponent<PlayerStat>().Init(0);
		Instantiate(playerUnit[0], playerSpawnPositions[0].transform.position, playerSpawnPositions[0].transform.rotation);
		playerPositionIndex = 1;
	}

	void Awake() {
        if (instance == null) {
            instance = this;
        }
		//playerUnit[0].GetComponent<PlayerStat>().Init(0);
		timeToStartBattle = 2.5f;
		callTurn = false;
		isTurnSkip = false;
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
		//player go firstckEnemy);
		enemyList = new List<GameObject>();
		playerList = new List<GameObject>();
		unitStats = new List<GameObject>();
		for (int i = 0; i < 2; i++) {
			enemyList.Add(enemiesUnit[i]);
		}
		enemyList.Sort(delegate (GameObject x, GameObject y) {
			return y.GetComponent<EnemyStat>().enemy.baseStat.dexterity
				    .CompareTo(x.GetComponent<EnemyStat>().enemy.baseStat.dexterity);
		});
		for (int i = 0; i < playerPositionIndex; i++ ){
			playerList.Add(playerUnit[i]);
		}
		playerList.Sort(delegate (GameObject x, GameObject y) {
			return y.GetComponent<PlayerStat>().player.baseStat.dexterity.
				    CompareTo(x.GetComponent<PlayerStat>().player.baseStat.dexterity);
		});
		for (int i = 0; i < playerPositionIndex; i++) {
			if (playerUnit[i].tag == "PlayerUnit") {
				unitStats.Add(playerUnit[i]);
            }
        }
        for (int i = 0; i < 2; i++) {
			if (enemiesUnit[i].tag == "Enemy") {
				unitStats.Add(enemiesUnit[i]);
            }
        }
        unitStats.Sort(delegate (GameObject x, GameObject y) {
			if (x.tag == "PlayerUnit" && y.tag == "PlayerUnit") {
				return y.GetComponent<PlayerStat>().player.baseStat.dexterity
					    .CompareTo(x.GetComponent<PlayerStat>().player.baseStat.dexterity);
			}
			else if (x.tag == "PlayerUnit" && y.tag == "Enemy") {
				return y.GetComponent<EnemyStat>().enemy.baseStat.dexterity.
					    CompareTo(x.GetComponent<PlayerStat>().player.baseStat.dexterity);
			}
			else if (x.tag == "Enemy" && y.tag == "PlayerUnit") {
				return y.GetComponent<PlayerStat>().player.baseStat.dexterity.
						CompareTo(x.GetComponent<EnemyStat>().enemy.baseStat.dexterity);
			}
			return y.GetComponent<EnemyStat>().enemy.baseStat.dexterity
                    .CompareTo(x.GetComponent<EnemyStat>().enemy.baseStat.dexterity);
        });
        //if player go first
		if (GameManager.instance.isPlayerAttackEnemy == true) {
			int j = 0;
            foreach (GameObject player in playerList) {
                listImageShowUnitTurn[j].sprite = player.GetComponent<SpriteRenderer>().sprite;
                j++;
            }
            foreach (GameObject enemy in enemyList) {
                listImageShowUnitTurn[j].sprite = enemy.GetComponent<SpriteRenderer>().sprite;
                j++;
            }
            int k = 0;
            while (j < 10) {
                if (k == unitStats.Count) {
                    k = 0;
                }
                Debug.Log(k);
                listImageShowUnitTurn[j].sprite = unitStats[k].GetComponent<SpriteRenderer>().sprite;
                j++;
                k++;
            }
		}
        //if enemy go first 
		if (GameManager.instance.isEnemyAttackPlayer == true) {
			int j = 0;
			foreach (GameObject enemy in enemyList) {
				listImageShowUnitTurn[j].sprite = enemy.GetComponent<SpriteRenderer>().sprite;
                j++;
			}
			foreach (GameObject player in playerList) {
				listImageShowUnitTurn[j].sprite = player.GetComponent<SpriteRenderer>().sprite;
                j++;
			}
			int k = 0;
            while (j < 10) {
                if (k == unitStats.Count) {
                    k = 0;
                }
                listImageShowUnitTurn[j].sprite = unitStats[k].GetComponent<SpriteRenderer>().sprite;
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
			ScreenManager.Instance.TriggerLoadingFadeOut("M0002",false);
		}
		GameObject[] remainPlayerUnit = GameObject.FindGameObjectsWithTag("PlayerUnit");
        if (remainPlayerUnit.Length == 0) {
            //GameManager.instance.LoadGameMenu();
        }
        //enemy go first
		if (GameManager.instance.isEnemyAttackPlayer == true) {
			int j = 0;
			foreach (GameObject enemy in enemyList) {
				listImageShowUnitTurn[j].sprite = enemy.GetComponent<SpriteRenderer>().sprite;
				j++;
			}
			foreach (GameObject player in playerList) {
				listImageShowUnitTurn[j].sprite = player.GetComponent<SpriteRenderer>().sprite;
				j++;
			}
			int k = 0;
			while (j < 10) {
				if (k == unitStats.Count) {
					k = 0;
				}
				listImageShowUnitTurn[j].sprite = unitStats[k].GetComponent<SpriteRenderer>().sprite;
				j++;
				k++;
			}
            //check player turn
			if (enemyList.Count == 0) {
				actionMenu.SetActive(true);
                //go to next turn
				if (playerList.Count == 0 && enemyList.Count == 0) {
					isFirstTurn = false;
					unitLists = new Queue<GameObject>(unitStats);
					this.nextTurn();
				}
				else {
					if (isUnitAction == false) {
						currentUnit = playerList[0];
					}
					//player turn
					enemyTurn = false;
					actionMenu.SetActive(true);
					if (currentUnit.tag == "Player") {
						//spawm player selector 
                        if (isPlayerSelectEnemy == false) {
                            if (currentUnit.transform.position == playerSpawnPositions[0].transform.position) {
                                Instantiate(playerSelector, playerSelectorPositions[0].transform.position,
                                            playerSelectorPositions[0].transform.rotation);
                            }
                            else if (currentUnit.transform.position == playerSpawnPositions[1].transform.position){
                                Instantiate(playerSelector, playerSelectorPositions[1].transform.position,
                                            playerSelectorPositions[1].transform.rotation);
                            }
                            else if (currentUnit.transform.position == playerSpawnPositions[2].transform.position){
                                Instantiate(playerSelector, playerSelectorPositions[2].transform.position,
                                            playerSelectorPositions[2].transform.rotation);
                            }
                        }
                        if (isPlayerSelectEnemy == true){
                            playerList.Remove(currentUnit);
                            //currentUnit.GetComponent<GetPlayerAction>().
                                       //AttackTarget(enemyGameObject[enemySelectedPositionIndex].GameObject,
                                                    //enemyGameObject[enemySelectedPositionIndex]);
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
				currentUnit.GetComponent<EnemyAction>().Action();
			}
		}

        //code for player go first
		if (GameManager.instance.isPlayerAttackEnemy == true) {
			int j = 0;
            foreach (GameObject player in playerList) {
                listImageShowUnitTurn[j].sprite = player.GetComponent<SpriteRenderer>().sprite;
                j++;
            }
            foreach (GameObject enemy in enemyList) {
                listImageShowUnitTurn[j].sprite = enemy.GetComponent<SpriteRenderer>().sprite;
                j++;
            }
            int k = 0;
            while (j < 10) {
                if (k == unitStats.Count) {
                    k = 0;
                }
                listImageShowUnitTurn[j].sprite = unitStats[k].GetComponent<SpriteRenderer>().sprite;
                j++;
                k++;
            }
            //check enemy turn
            if (playerList.Count == 0) {
                actionMenu.SetActive(false);
                //go to next turn
                if (playerList.Count == 0 && enemyList.Count == 0) {
                    isFirstTurn = false;
                    unitLists = new Queue<GameObject>(unitStats);
                    this.nextTurn();
                }
                //enemy turn
                else {
                    currentUnit = enemyList[0];
					if (currentUnit.tag == "Enemy") {
						enemyList.Remove(currentUnit);
                        enemyTurn = true;
                        currentUnit.GetComponent<EnemyAction>().Action();
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
                    if (currentUnit.transform.position == playerSpawnPositions[0].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[0].transform.position,
                                    playerSelectorPositions[0].transform.rotation);
                    }
                    else if (currentUnit.transform.position == playerSpawnPositions[1].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[1].transform.position,
                                    playerSelectorPositions[1].transform.rotation);
                    }
                    else if (currentUnit.transform.position == playerSpawnPositions[2].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[2].transform.position,
                                    playerSelectorPositions[2].transform.rotation);
                    }
                }
                if (isPlayerSelectEnemy == true) {
                    playerList.Remove(currentUnit);
                    //currentUnit.GetComponent<GetPlayerAction>().
                               //AttackTarget(enemyGameObject[enemySelectedPositionIndex].GameObject,
                                            //enemyGameObject[enemySelectedPositionIndex]);
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
				isTurnSkip = true;
				time = timer;
				if (isFirstTurn == true) {
					playerList.Remove(currentUnit);
					this.FristTurn();
				}
				else {
					unitStats.Remove(currentUnit);
					unitStats.Add(currentUnit);
					unitLists.Enqueue(currentUnit);
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
				ScreenManager.Instance.TriggerLoadingFadeOut("M0002",false);
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
			listImageShowUnitTurn[j].sprite = unitStats[k].GetComponent<SpriteRenderer>().sprite;
            j++;
            k++;
        }
		if (isPlayerSelectEnemy == false && isSelectorSpawn == false && isUnitAction == false) {
			//currentUnit = unitStats[0];
			currentUnit = unitLists.Dequeue();
		}
		if (currentUnit.tag != "DeadUnit") {
			if (currentUnit.tag == "PlayerUnit") {
				//player turn
				enemyTurn = false;
				//attack_DefendMenu.SetActive(false);
				//spawn player selector
				if (isPlayerSelectEnemy == false) {
                    if (currentUnit.transform.position == playerSpawnPositions[0].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[0].transform.position,
                                    playerSelectorPositions[0].transform.rotation);
                    }
                    else if (currentUnit.transform.position == playerSpawnPositions[1].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[1].transform.position,
                                    playerSelectorPositions[1].transform.rotation);
                    }
                    else if (currentUnit.transform.position == playerSpawnPositions[2].transform.position) {
                        Instantiate(playerSelector, playerSelectorPositions[2].transform.position,
                                    playerSelectorPositions[2].transform.rotation);
                    }
					actionMenu.SetActive(true);
                }
				if (isPlayerSelectEnemy == true && isSelectorSpawn == true) {
					unitStats.Remove(currentUnit);
					unitStats.Add(currentUnit);
					unitLists.Enqueue(currentUnit);
					//currentUnit.GetComponent<GetPlayerAction>().
								   //AttackTarget(enemyGameObject[enemySelectedPositionIndex].GameObject,
													//enemyGameObject[enemySelectedPositionIndex]);
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
                    currentUnit.GetComponent<EnemyAction>().Action();
				}
			}
		}
		else {
			actionMenu.SetActive(false);
			attack_DefendMenu.SetActive(false);
			unitLists.Enqueue(currentUnit);
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
