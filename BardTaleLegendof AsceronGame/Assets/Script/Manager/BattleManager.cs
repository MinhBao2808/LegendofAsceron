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
	public bool isUnitAction = false;
//	private float sumExpCanGet = 0.0f;
	public Combatant currentUnit;
	public bool isSelectorSpawn = false;
	public bool[] isEnemyDead = new bool[6];
	//private Vector3 selectorPositionY = new Vector3(0, 3, 0);

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
		//enemySelectedPositionIndex = 0;
        //int index = Random.Range(0, enemyEncouterPrefab.Length);
        //Instantiate(enemyEncouterPrefab[index], enemyEncouterPrefab[index].transform.position, enemyEncouterPrefab[index].transform.rotation);
	}

    void Start() {
		Debug.Log(GameManager.instance.isEnemyAttackPlayer);
		Debug.Log(GameManager.instance.isPlayerAttackEnemy);
        //this.playerParty = GameObject.Find("PlayerParty");
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
       // this.actionsMenu.SetActive(false);
  //      this.playerHealthName.SetActive(false);
		//this.playerAvatar.SetActive(false);
        //this.playerManaName.SetActive(false);
       // this.enemyUnitsMenu.SetActive(false);
		this.FristTurn();
	}

	public void FristTurn () {
		//if (enemyList.Count == 0) {
		//	GameManager.instance.LoadMapScene();
		//}
		//if (playerList.Count == 0) {
		//	GameManager.instance.LoadGameMenu();
		//}
		//Combatant currentUnit = playerList[0];
		//playerList.Remove(currentUnit);
		//if (currentUnit.Dead != true) {
		//	GameObject currentUnitObject = currentUnit.GameObject;
		//	Instantiate(selector, enemySpawnPositions[0].transform.position, enemySpawnPositions[0].transform.rotation);
		//}
        //check is game over
		GameObject[] remainEnemyUnit = GameObject.FindGameObjectsWithTag("Enemy");
		if (remainEnemyUnit.Length == 0) {
			GameManager.instance.LoadMapScene();
		}
		GameObject[] remainPlayerUnit = GameObject.FindGameObjectsWithTag("PlayerUnit");
        if (remainPlayerUnit.Length == 0) {
            GameManager.instance.LoadGameMenu();
        }
        
		isFirstTurn = true;
        //code for player go first
        //check enemy turn
		if (playerList.Count == 0) {
			//go to next turn
			if (playerList.Count == 0 && enemyList.Count == 0) {
				for (int i = 0; i < playerPositionIndex; i++) {
					unitStats.Add(playerCombatant[i]);
				}
				for (int i = 0; i < enemyPositionIndex; i++) {
					unitStats.Add(enemyCombatant[i]);
				}
				unitStats.Sort(delegate (Combatant x, Combatant y) {
                    return y.Status[9].GetValue().CompareTo(x.Status[9].GetValue());
                });
				//create queue for next turn
				isFirstTurn = false;
				unitLists = new Queue<Combatant>(unitStats);
				this.nextTurn();
			}
			//enemy turn
			else {
				currentUnit = enemyList[0];
                enemyList.Remove(currentUnit);
                enemyTurn = true;
                currentUnit.GameObject.GetComponent<EnemyAction>().Action();
			}
        }
		else {
			if (isUnitAction == false) {
				currentUnit = playerList[0];
			}
			Debug.Log(currentUnit.GameObject.name);
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
			//choose enemy
            if (isPlayerSelectEnemy == false) {
                for (int i = 0; i < enemyPositionIndex; i++) {
                    if (isEnemyDead[enemyPositionIndex] == false) {
                        Instantiate(selector, 
						            enemySpawnPositions[i].transform.position, 
						            enemySpawnPositions[i].transform.rotation);
                        break;
                    }
                }
            }
            //player turn
            if (isPlayerSelectEnemy == true) {
                enemyTurn = false;
                playerList.Remove(currentUnit);
                currentUnit.GameObject.GetComponent<GetPlayerAction>().
                           AttackTarget(enemyCombatant[enemySelectedPositionIndex].GameObject,
				                        enemyCombatant[enemySelectedPositionIndex]);
            }
		}
       
	}

    void Update() {
        if (enemyTurn == false && playerSelectAttack == false) {
			time -= Time.deltaTime;
            //Debug.Log(time);
            if (time <= 0.0f) {
				time = timer;
                this.nextTurn();
            }
        }
        else if (playerAttack == true && enemyTurn == false) {
            time = timer;
        }
    }

    public void SetPlayerInfoUI(PlayerStat currentPlayerStat) {
  //      playerHealthName.SetActive(true);
  //      playerManaName.SetActive(true);
		//playerAvatar.SetActive(true);
		//playerAvatarImage.overrideSprite = currentPlayerStat.playerAvatar;
        //playerHealthText.text = "" + currentPlayerStat.health;
        //playerManaText.text = "" + currentPlayerStat.mana;
    }

    public void SetActivePlayerInfoUI () {
        //playerHealthName.SetActive(false);
        //playerManaName.SetActive(false);
    }

    public void nextTurn() {
		isFirstTurn = false;
        //this.actionsMenu.SetActive(false);
  //      this.playerHealthName.SetActive(false);
		//this.playerAvatar.SetActive(false);
        //this.playerManaName.SetActive(false);
        //this.enemyUnitsMenu.SetActive(false);
        playerAttack = false;
		if (isPlayerSelectEnemy == false && isSelectorSpawn == false && isUnitAction == false) {
			GameObject[] remainEnemyUnit = GameObject.FindGameObjectsWithTag("Enemy");
            if (remainEnemyUnit.Length == 0) {
                //        GameObject[] playerObject = GameObject.FindGameObjectsWithTag("PlayerUnit");
                //        for (var i = 0; i < playerObject.Length; i++) {
                //if (playerObject[i].gameObject.name == "Player1") {
                //playerObject[i].gameObject.GetComponent<PlayerStat>().calculateExp(sumExpCanGet / playerObject.Length);
                //DataManager.instance.maxHealthPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().maxHealth;
                //DataManager.instance.currentExpPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().currentExp;
                //DataManager.instance.levelPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().playerLv;
                //DataManager.instance.maxManaPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().maxHealth;
                //               DataManager.instance.attackPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().attack;
                //               DataManager.instance.defensePlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().defense;
                //               DataManager.instance.healthPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().health;
                //               DataManager.instance.magicPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().magic;
                //               DataManager.instance.manaPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().mana;
                //               DataManager.instance.speedPlayer1 = playerObject[i].gameObject.GetComponent<PlayerStat>().speed;
                //           }
                //           else if(playerObject[i].gameObject.name == "Player2") {
                //playerObject[i].gameObject.GetComponent<PlayerStat>().calculateExp(sumExpCanGet / playerObject.Length);
                //DataManager.instance.maxHealthPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().maxHealth;
                //DataManager.instance.currentExpPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().currentExp;
                //DataManager.instance.levelPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().playerLv;
                //DataManager.instance.maxManaPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().maxMana;
                //        DataManager.instance.attackPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().attack;
                //        DataManager.instance.defensePlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().defense;
                //        DataManager.instance.healthPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().health;
                //        DataManager.instance.magicPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().magic;
                //        DataManager.instance.manaPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().mana;
                //        DataManager.instance.speedPlayer2 = playerObject[i].gameObject.GetComponent<PlayerStat>().speed;
                //    }
                //}
                GameManager.instance.LoadMapScene();
            }
            GameObject[] remainPlayerUnit = GameObject.FindGameObjectsWithTag("PlayerUnit");
            if (remainPlayerUnit.Length == 0) {
                GameManager.instance.LoadGameMenu();
            }
		}

		if (isPlayerSelectEnemy == false && isSelectorSpawn == false && isUnitAction == false) {
			//currentUnit = unitStats[0];
			//unitStats.Remove(currentUnit);
			currentUnit = unitLists.Dequeue();
		}
		Debug.Log(currentUnit.GameObject.name);
		if (currentUnit.GameObject.tag != "DeadUnit") {
			//GameObject currentUnit = currentUnitStat.gameObject;
			//unitStats.Sort();
			if (currentUnit.GameObject.tag == "PlayerUnit") {
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
				//choose enemy
				enemyTurn = false;
				if (isPlayerSelectEnemy == false && enemyTurn == false && isSelectorSpawn == false) {
					for (int i = 0; i < enemyPositionIndex; i++) {
						if (isEnemyDead[i] == false) {
							isSelectorSpawn = true;
							Instantiate(selector, enemySpawnPositions[i].transform.position,
						  enemySpawnPositions[i].transform.rotation);
							break;
						}
					}
				}
				//player turn
				if (isPlayerSelectEnemy == true && isSelectorSpawn == true) {
						//unitStats.Add(currentUnit);
						unitLists.Enqueue(currentUnit);
						currentUnit.GameObject.GetComponent<GetPlayerAction>().
								   AttackTarget(enemyCombatant[enemySelectedPositionIndex].GameObject,
													enemyCombatant[enemySelectedPositionIndex]);
					}
				}
			else {
				if (isSelectorSpawn == false && isUnitAction == false) {
					isUnitAction = true;
					enemyTurn = true;
                    //unitStats.Add(currentUnit);
                    unitLists.Enqueue(currentUnit);
                    currentUnit.GameObject.GetComponent<EnemyAction>().Action();
				}
			}
		}
		else {
			Debug.Log("a");
			unitLists.Enqueue(currentUnit);
			//unitStats.Add(currentUnit);
			this.nextTurn();
		}
    }

    public bool isEnemyTurn() {
        return enemyTurn;
    }

    public void SetPlayerSelectAttack() {//player choose attack enemy 
        playerSelectAttack = true;
    }

    public bool isPlayerSelectAttack() {
        return playerSelectAttack;
    }
    public void GetPlayerSelectAttack() {
        playerSelectAttack = false;
    }

    public bool isPlayerAttack() {
        return playerAttack;
    }
    public void SetPlayerAttack() {
        playerAttack = true;
    }
    public void GetPlayerAttack() {
        playerAttack = false;
    }
}
