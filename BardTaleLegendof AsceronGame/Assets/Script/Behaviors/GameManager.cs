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
	public List<int> enemyInMapIndex;
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
		enemyInMapIndex = new List<int>();
    }
}
