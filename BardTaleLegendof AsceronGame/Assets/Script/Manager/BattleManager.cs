﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
	[SerializeField] private Button[] itemButton;
	[SerializeField] private Button[] skillButton;
	[SerializeField] private GameObject gameOverPanel;
	[SerializeField] private GameObject victoryPanel;
	[SerializeField] private GameObject player1LvPanel;
	[SerializeField] private GameObject player2LvPanel;
	[SerializeField] private GameObject player3LvPanel;
	[SerializeField] private GameObject larsSkillPanel;
	//private GameObject playerSelectorSpawn;
	[SerializeField] private GameObject[] playerSelectorPositions;
	[SerializeField] private GameObject itemPanel;
	private GameObject playerSelectorSpawned;
	public bool isGameOver;
	public bool isVictory;
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
	private int sumGilPoint;
	private int sumExpPoint;
	private int expPlayerCanGet;
	private float expModifier;
	private bool isPlayerPressSkillButton = false;
	private int playerSkillIndex;
	private bool isPlayerUseItem;
	int count = 0;

	public bool IsPlayerUseItem
	{
		get
		{
			return isPlayerUseItem;
		}

		set
		{
			isPlayerUseItem = value;
		}
	}

	//private Vector3 selectorPositionY = new Vector3(0, 3, 0);

	public void PressActionButton () {
		actionMenu.SetActive(true);
		attack_DefendMenu.SetActive(true);
	}

	public void PressAttackButton () {
		//choose enemy
		attack_DefendMenu.SetActive(false);
		isPlayerPressSkillButton = false;
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

	public void PressSkillPanelButton () {
		attack_DefendMenu.SetActive(false);
        actionMenu.SetActive(true);
		larsSkillPanel.SetActive(true);
		for (int i = 0; i < currentUnit.GetComponent<PlayerStat>().player.info.skills.Length;i++) {
			//get name of skill and add to all button;
			skillButton[i].GetComponentInChildren<Text>().text = currentUnit.GetComponent<PlayerStat>().player.info.skills[i];
		}
	}

	public void PressSkillButton (int index) {
		isSelectorSpawn = true;
		if (currentUnit.transform.position == playerUnit[0].transform.position) {
			larsSkillPanel.SetActive(false);
        }
		isPlayerPressSkillButton = true;
		playerSkillIndex = index;
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
		if (playerUnit[0].GetComponent<PlayerStat>().player.level <= 10) {
			enemyPositionIndex = Random.Range(1, 3);
		}
		else {
			enemyPositionIndex = Random.Range(1, enemySpawnPositions.Length);
		}
		int enemyDataIndex;
		for (int i = 0; i < enemyPositionIndex; i++) {
			enemyDataIndex = Random.Range(0, 1);
			enemiesUnit[i].GetComponent<EnemyStat>().Init(enemyDataIndex);
			//enemiesUnit[i].GetComponent<EnemyStat>().enemy.stats = enemiesUnit[i].GetComponent<EnemyStat>().enemy.growthStat;
			enemiesUnit[i] =  Instantiate(enemiesUnit[i], enemySpawnPositions[i].transform.position, 
			            enemySpawnPositions[i].transform.rotation);
		}
	}

	public void PressUseItemButton () {
		itemPanel.SetActive(true);
		attack_DefendMenu.SetActive(false);
		larsSkillPanel.SetActive(false);
		if (DataManager.Instance.UsableList.Count > 0) {
			for (int i = 0; i < DataManager.Instance.UsableList.Count; i++) {
				//get item from data
				itemButton[i].GetComponentInChildren<Text>().text = "" + DataManager.Instance.UsableList[i].name;
				itemButton[i].GetComponentInChildren<Image>().sprite = Resources.Load(DataManager.Instance.UsableList[i].imgPath) as Sprite;
            }
		}
	}

	public void PressUseItem(int index) {
		if (currentUnit.gameObject.tag == "PlayerUnit") {
			if (DataManager.Instance.UsableList[index].amount > 0) {
				if (DataManager.Instance.UsableList[index].regenType == JsonDataClasses.UsableRegenType.HP) {
					float currentHp = currentUnit.GetComponent<PlayerStat>().player.battleStats.hp;
					currentHp += DataManager.Instance.UsableList[index].amount;
					if (currentHp >= currentUnit.GetComponent<PlayerStat>().player.battleStats.maxHp) {
						currentHp = currentUnit.GetComponent<PlayerStat>().player.battleStats.maxHp;
					}
					currentUnit.GetComponent<GenerateDamageText>().ReceiveDamage(DataManager.Instance.UsableList[index].amount);
					currentUnit.GetComponent<PlayerStat>().player.SetHp(currentHp);
				}
				if (DataManager.Instance.UsableList[index].regenType==JsonDataClasses.UsableRegenType.MP) {
					float currentMp = currentUnit.GetComponent<PlayerStat>().player.battleStats.mp;
					currentMp += PlayerManager.Instance.Usables[index].amount;
					if (currentMp >= currentUnit.GetComponent<PlayerStat>().player.battleStats.maxMp) {
						currentMp = currentUnit.GetComponent<PlayerStat>().player.battleStats.maxMp;
					}
					currentUnit.GetComponent<GenerateDamageText>().ReceiveDamage(DataManager.Instance.UsableList[index].amount);
					currentUnit.GetComponent<PlayerStat>().player.SetMp(currentMp);
				}
				//if (PlayerManager.Instance.Usables[index].regenType == JsonDataClasses.UsableRegenType.Synergy) {
				//	float currentSynergy = currentUnit.GetComponent<PlayerStat>().pla
				//}
			}
			isPlayerUseItem = true;
			itemPanel.SetActive(false);
            if (isFirstTurn == true)
            {
                playerList.Remove(currentUnit);
                //this.FristTurn();
            }
            else
            {
                unitStats.Remove(currentUnit);
                unitStats.Add(currentUnit);
                unitLists.Enqueue(currentUnit);
                //this.nextTurn();
            }
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
		playerUnit[0] = Instantiate(playerUnit[0], 
		                            playerSpawnPositions[0].transform.position, 
		                            playerSpawnPositions[0].transform.rotation);
		playerPositionIndex = 1;
	}

	void Awake() {
        if (instance == null) {
            instance = this;
        }
		isGameOver = false;
		isVictory = false;
		//playerUnit[0].GetComponent<PlayerStat>().Init(0);
		timeToStartBattle = 2.5f;
		callTurn = false;
		isTurnSkip = false;
		isPlayerUseItem = false;
		SpawnPlayer();
		SpawnEnemy();
		sumGilPoint = 0;
		sumExpPoint = 0;
		expPlayerCanGet = 0;
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
		switch (PlayerManager.Instance.Difficulty) {
			case 0: timer = 20.0f;
				expModifier = 1.25f;
				break;
			case 1: timer = 15.0f;
				expModifier = 1.0f;
				break;
			case 2: timer = 10.0f;
				expModifier = 0.75f;
				break;
			case 3: timer = 5.0f;
				expModifier = 0.5f;
				break;
			default: break;
		}
		//timer = 
		//timer = ORK.Difficulties.GetBattleFactor() * 20.0f;
        time = timer;
		//Debug.Log(time);
        playerAttack = false;
		actionMenu.SetActive(false);
		attack_DefendMenu.SetActive(false);
		larsSkillPanel.SetActive(false);
		itemPanel.SetActive(false);
	}

    void Start() {
		//player go firstckEnemy);
		enemyList = new List<GameObject>();
		playerList = new List<GameObject>();
		unitStats = new List<GameObject>();
		for (int i = 0; i < enemyPositionIndex; i++) {
			enemyList.Add(enemiesUnit[i]);
		}
		enemyList.Sort(delegate (GameObject x, GameObject y) {
			return y.GetComponent<EnemyStat>().enemy.stats.dexterity
				    .CompareTo(x.GetComponent<EnemyStat>().enemy.stats.dexterity);
		});
		for (int i = 0; i < playerPositionIndex; i++ ){
			playerList.Add(playerUnit[i]);
		}
		playerList.Sort(delegate (GameObject x, GameObject y) {
			return y.GetComponent<PlayerStat>().player.battleStats.spd.
				    CompareTo(x.GetComponent<PlayerStat>().player.battleStats.spd);
		});
		for (int i = 0; i < playerPositionIndex; i++) {
			if (playerUnit[i].tag == "PlayerUnit") {
				unitStats.Add(playerUnit[i]);
            }
        }
		for (int i = 0; i < enemyPositionIndex; i++) {
			if (enemiesUnit[i].tag == "Enemy") {
				unitStats.Add(enemiesUnit[i]);
            }
        }
        unitStats.Sort(delegate (GameObject x, GameObject y) {
			if (x.tag == "PlayerUnit" && y.tag == "PlayerUnit") {
				return y.GetComponent<PlayerStat>().player.battleStats.spd
					    .CompareTo(x.GetComponent<PlayerStat>().player.battleStats.spd);
			}
			else if (x.tag == "PlayerUnit" && y.tag == "Enemy") {
				return y.GetComponent<EnemyStat>().enemy.stats.dexterity.
					    CompareTo(x.GetComponent<PlayerStat>().player.battleStats.spd);
			}
			else if (x.tag == "Enemy" && y.tag == "PlayerUnit") {
				return y.GetComponent<PlayerStat>().player.battleStats.spd.
					    CompareTo(x.GetComponent<EnemyStat>().enemy.stats.dexterity);
			}
			return y.GetComponent<EnemyStat>().enemy.stats.dexterity
				    .CompareTo(x.GetComponent<EnemyStat>().enemy.stats.dexterity);
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
			AudioManager.Instance.ChangeBgm(AudioManager.Instance.battleBgms[5]);
			GameManager.instance.isPlayerAttackEnemy = false;
			GameManager.instance.isEnemyAttackPlayer = false;
			GameManager.instance.isBattleSceneAnimationLoaded = false;
			isVictory = true;
			victoryPanel.SetActive(true);
			for (int i = 0; i < enemyPositionIndex; i++) {
				sumGilPoint = sumGilPoint + enemiesUnit[i].GetComponent<EnemyStat>().enemy.baseReward.gold;
				sumExpPoint = sumExpPoint + enemiesUnit[i].GetComponent<EnemyStat>().enemy.baseReward.exp;
			}
			UpdateVictoryPanelUI.instance.UpdateGilPoint(sumGilPoint);
			UpdateVictoryPanelUI.instance.UpdateExpPoint(sumExpPoint);
			PlayerManager.Instance.Currency += sumGilPoint;
			//expPlayerCanGet = sumExpPoint / playerPositionIndex;
			expPlayerCanGet = (sumExpPoint / playerPositionIndex) * (int)expModifier;
			if (playerPositionIndex == 1) {
				player1LvPanel.SetActive(true);
                player2LvPanel.SetActive(false);
                player3LvPanel.SetActive(false);
				playerUnit[0].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
				UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[0].GetComponent<PlayerStat>().player.info.name,
									 playerUnit[0].GetComponent<PlayerStat>().player.level,
									 playerUnit[0].GetComponent<PlayerStat>().player.experience,
				                                              Expression.GetExpExpression(playerUnit[0].GetComponent<PlayerStat>().player.level + 1));
				PlayerManager.Instance.Characters[0] = playerUnit[0].GetComponent<PlayerStat>().player;
				Debug.Log(PlayerManager.Instance.Characters[0].level);
			}
			if (playerPositionIndex == 2) {
				player1LvPanel.SetActive(true);
				player2LvPanel.SetActive(true);
                player3LvPanel.SetActive(false);
				playerUnit[0].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
				playerUnit[1].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
				UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[0].GetComponent<PlayerStat>().player.info.name,
                                     playerUnit[0].GetComponent<PlayerStat>().player.level,
                                     playerUnit[0].GetComponent<PlayerStat>().player.experience,
                                                              Expression.GetExpExpression(playerUnit[0].GetComponent<PlayerStat>().player.level + 1));
				UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[1].GetComponent<PlayerStat>().player.info.name,
                                     playerUnit[1].GetComponent<PlayerStat>().player.level,
                                     playerUnit[1].GetComponent<PlayerStat>().player.experience,
                                                              Expression.GetExpExpression(playerUnit[1].GetComponent<PlayerStat>().player.level + 1));
				PlayerManager.Instance.Characters[0] = playerUnit[0].GetComponent<PlayerStat>().player;
				PlayerManager.Instance.Characters[1] = playerUnit[1].GetComponent<PlayerStat>().player;
			}
			if (playerPositionIndex == 3) {
				player1LvPanel.SetActive(true);
				player2LvPanel.SetActive(true);
				player3LvPanel.SetActive(true);
				playerUnit[0].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
				playerUnit[1].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
				playerUnit[2].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
				UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[0].GetComponent<PlayerStat>().player.info.name,
                                     playerUnit[0].GetComponent<PlayerStat>().player.level,
                                     playerUnit[0].GetComponent<PlayerStat>().player.experience,
                                                              Expression.GetExpExpression(playerUnit[0].GetComponent<PlayerStat>().player.level + 1));
				UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[1].GetComponent<PlayerStat>().player.info.name,
                                     playerUnit[1].GetComponent<PlayerStat>().player.level,
                                     playerUnit[1].GetComponent<PlayerStat>().player.experience,
                                                              Expression.GetExpExpression(playerUnit[1].GetComponent<PlayerStat>().player.level + 1));
				UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[2].GetComponent<PlayerStat>().player.info.name,
                                     playerUnit[2].GetComponent<PlayerStat>().player.level,
                                     playerUnit[2].GetComponent<PlayerStat>().player.experience,
                                                              Expression.GetExpExpression(playerUnit[2].GetComponent<PlayerStat>().player.level + 1));
				PlayerManager.Instance.Characters[0] = playerUnit[0].GetComponent<PlayerStat>().player;
				PlayerManager.Instance.Characters[1] = playerUnit[1].GetComponent<PlayerStat>().player;
				PlayerManager.Instance.Characters[3] = playerUnit[3].GetComponent<PlayerStat>().player;
			}
			//ScreenManager.Instance.TriggerLoadingFadeOut("M0002",false);
		}
		GameObject[] remainPlayerUnit = GameObject.FindGameObjectsWithTag("PlayerUnit");
        if (remainPlayerUnit.Length == 0) {
            //GameManager.instance.LoadGameMenu();
			isGameOver = true;
            gameOverPanel.SetActive(true);
        }
        //enemy go first
		if (GameManager.instance.isEnemyAttackPlayer == true) {
			count++;
			Debug.Log(count);
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
					if (currentUnit.tag == "PlayerUnit") {
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
							if (isPlayerPressSkillButton == true) {
								currentUnit.GetComponent<GetPlayerAction>().
								           PerformSkill(playerSkillIndex, enemiesUnit[enemySelectedPositionIndex]);
							}
							else {
								currentUnit.GetComponent<GetPlayerAction>().
                                       AttackTarget(enemiesUnit[enemySelectedPositionIndex]);
							}
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
				enemyTurn = true;
				currentUnit = enemyList[0];
				enemyList.Remove(currentUnit);
				currentUnit.GetComponent<EnemyActionBattle>().Action();
			}
		}

        //code for player go first
		if (GameManager.instance.isPlayerAttackEnemy == true) {
			Debug.Log("b");
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
						currentUnit.GetComponent<EnemyActionBattle>().Action();
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
					if (isPlayerSelectEnemy == true) {
                        playerList.Remove(currentUnit);
                        if (isPlayerPressSkillButton == true) {
                            currentUnit.GetComponent<GetPlayerAction>().
							           PerformSkill(playerSkillIndex, enemiesUnit[enemySelectedPositionIndex]);
                        }
                        else {
                            currentUnit.GetComponent<GetPlayerAction>().
                                   AttackTarget(enemiesUnit[enemySelectedPositionIndex]);
                        }
                    }
                }
            }
		} 
	}

    void Update() {
		//Debug.Log(timer);
		timerSlider.value = time / timer;
		if (enemyTurn == false && callTurn == true) {
			time -= Time.deltaTime;
            Debug.Log(time);
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
        }
		if (timeToStartBattle <= 0 && callTurn == false) {
			timeToStartBattle = 0;
			this.FristTurn();
		}
		if (isGameOver == true) {
			if (Input.GetKeyDown(KeyCode.Return)) {
                SceneManager.LoadScene(0);
            }
		}
		if (isVictory == true) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				ScreenManager.Instance.TriggerLoadingFadeOut("M0002", false);
			}
        }
    }

    public void nextTurn() {
		isFirstTurn = false;
        playerAttack = false;
		callTurn = true;
		Debug.Log("c");
		if (isPlayerSelectEnemy == false && isSelectorSpawn == false && isUnitAction == false) {
			GameObject[] remainEnemyUnit = GameObject.FindGameObjectsWithTag("Enemy");
            if (remainEnemyUnit.Length == 0) {
				AudioManager.Instance.ChangeBgm(AudioManager.Instance.battleBgms[5]);
				GameManager.instance.isEnemyAttackPlayer = false;
				GameManager.instance.isPlayerAttackEnemy = false;
				GameManager.instance.isBattleSceneAnimationLoaded = false;
				isVictory = true;
				victoryPanel.SetActive(true);
				for (int i = 0; i < enemyPositionIndex; i++) {
                    sumGilPoint = sumGilPoint + enemiesUnit[i].GetComponent<EnemyStat>().enemy.baseReward.gold;
                    sumExpPoint = sumExpPoint + enemiesUnit[i].GetComponent<EnemyStat>().enemy.baseReward.exp;
                }
				UpdateVictoryPanelUI.instance.UpdateGilPoint(sumGilPoint);
                UpdateVictoryPanelUI.instance.UpdateExpPoint(sumExpPoint);
				PlayerManager.Instance.Currency += sumGilPoint;
				expPlayerCanGet = (sumExpPoint / playerPositionIndex) * (int)expModifier;
                if (playerPositionIndex == 1) {
					player1LvPanel.SetActive(true);
					player2LvPanel.SetActive(false);
					player3LvPanel.SetActive(false);
                    playerUnit[0].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
					UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[0].GetComponent<PlayerStat>().player.info.name,
                                         playerUnit[0].GetComponent<PlayerStat>().player.level,
                                         playerUnit[0].GetComponent<PlayerStat>().player.experience,
                                                                  Expression.GetExpExpression(playerUnit[0].GetComponent<PlayerStat>().player.level + 1));
					PlayerManager.Instance.Characters[0] = playerUnit[0].GetComponent<PlayerStat>().player;
                }
				if (playerPositionIndex == 2) {
					player1LvPanel.SetActive(true);
                    player2LvPanel.SetActive(true);
                    player3LvPanel.SetActive(false);
                    playerUnit[0].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
                    playerUnit[1].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
					UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[0].GetComponent<PlayerStat>().player.info.name,
                                         playerUnit[0].GetComponent<PlayerStat>().player.level,
                                         playerUnit[0].GetComponent<PlayerStat>().player.experience,
                                                                  Expression.GetExpExpression(playerUnit[0].GetComponent<PlayerStat>().player.level + 1));
					UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[1].GetComponent<PlayerStat>().player.info.name,
                                         playerUnit[1].GetComponent<PlayerStat>().player.level,
                                         playerUnit[1].GetComponent<PlayerStat>().player.experience,
                                                                  Expression.GetExpExpression(playerUnit[1].GetComponent<PlayerStat>().player.level + 1));
					PlayerManager.Instance.Characters[0] = playerUnit[0].GetComponent<PlayerStat>().player;
					PlayerManager.Instance.Characters[1] = playerUnit[1].GetComponent<PlayerStat>().player;
                }
				if (playerPositionIndex == 3) {
					player1LvPanel.SetActive(true);
					player2LvPanel.SetActive(true);
                    player3LvPanel.SetActive(true);
                    playerUnit[0].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
                    playerUnit[1].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
                    playerUnit[2].GetComponent<PlayerStat>().player.AddExperience(expPlayerCanGet);
					UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[0].GetComponent<PlayerStat>().player.info.name,
                                         playerUnit[0].GetComponent<PlayerStat>().player.level,
                                         playerUnit[0].GetComponent<PlayerStat>().player.experience,
                                                                  Expression.GetExpExpression(playerUnit[0].GetComponent<PlayerStat>().player.level + 1));
					UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[1].GetComponent<PlayerStat>().player.info.name,
                                         playerUnit[1].GetComponent<PlayerStat>().player.level,
                                         playerUnit[1].GetComponent<PlayerStat>().player.experience,
                                                                  Expression.GetExpExpression(playerUnit[1].GetComponent<PlayerStat>().player.level + 1));
					UpdateVictoryPanelUI.instance.UpdatePLayer1Lv(playerUnit[2].GetComponent<PlayerStat>().player.info.name,
                                         playerUnit[2].GetComponent<PlayerStat>().player.level,
                                         playerUnit[2].GetComponent<PlayerStat>().player.experience,
                                                                  Expression.GetExpExpression(playerUnit[2].GetComponent<PlayerStat>().player.level + 1));
					PlayerManager.Instance.Characters[0] = playerUnit[0].GetComponent<PlayerStat>().player;
					PlayerManager.Instance.Characters[1] = playerUnit[1].GetComponent<PlayerStat>().player;
					PlayerManager.Instance.Characters[2] = playerUnit[2].GetComponent<PlayerStat>().player;
                }
				//ScreenManager.Instance.TriggerLoadingFadeOut("M0002",false);
            }
            GameObject[] remainPlayerUnit = GameObject.FindGameObjectsWithTag("PlayerUnit");
            if (remainPlayerUnit.Length == 0) {
				isGameOver = true;
				gameOverPanel.SetActive(true);
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
					if (isPlayerSelectEnemy == true) {
                        playerList.Remove(currentUnit);
                        if (isPlayerPressSkillButton == true) {
                            currentUnit.GetComponent<GetPlayerAction>().
							           PerformSkill(playerSkillIndex, enemiesUnit[enemySelectedPositionIndex]);
                        }
                        else {
                            currentUnit.GetComponent<GetPlayerAction>().
                                   AttackTarget(enemiesUnit[enemySelectedPositionIndex]);
                        }
                    }
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
					currentUnit.GetComponent<EnemyActionBattle>().Action();
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
