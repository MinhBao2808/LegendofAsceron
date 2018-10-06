using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public enum Level {
	Passive,
	Normal,
	Aggressive,
	Nightmare
}

public class GameManager:MonoBehaviour {
    public static GameManager instance = null;
	[SerializeField] private GameObject enemyPrefab;
	public int index;//store index of save file
    private Vector3 currentPlayerPosition = new Vector3();
	private bool isSceneMenu;

	//check is player on map scene attack enemy 
	public bool isPlayerAttackEnemy;
	//check is enemy on map scene attack player
	public bool isEnemyAttackPlayer;


    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
		index = PlayerPrefs.GetInt("c");
		isSceneMenu = true;
    }

	private void Start() {
		SceneManager.sceneLoaded += OnSceneLoaded;
		//StartCoroutine(StartRoutine());
	}

	public void GoToBattle () {
        currentPlayerPosition = PlayerMovement.instance.ReturnPlayerPosition();
        //DataManager.instance.playerPosition = currentPlayerPosition;
		//battleMusic.SetActive(true);
		SceneManager.LoadScene("2 BattleScene");
    }
    

	public void QuitGame() {
		Application.Quit();
	}

	public void LoadMapScene() {
		//battleMusic.SetActive(false);
		SceneManager.LoadScene(1);
	}
   
    public void LoadGameMenu() {//go to game menu
        SceneManager.LoadScene(0);
    }

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (scene.name == "1 MapScene") {
			isSceneMenu = false;
			SpawnEnemy();
			//SceneManager.sceneLoaded -= OnSceneLoaded;
        }
		if (scene.name == "2 BattleScene") {
			isSceneMenu = false;
		}
    }

	private void SpawnEnemy () {
		GameObject[] spawnPointObject = GameObject.FindGameObjectsWithTag("SpawnPoint");
		for (int i = 0; i < spawnPointObject.Length; i++) {
			//Debug.Log(randomPosition);
			var enemySpawn = Instantiate(enemyPrefab, spawnPointObject[i].transform.position, spawnPointObject[i].transform.rotation);
		}
	}

	public void PlayerGoToNewMap() {
		//DataManager.instance.listEnemyDefeatedPosition.Clear();
	}
}
