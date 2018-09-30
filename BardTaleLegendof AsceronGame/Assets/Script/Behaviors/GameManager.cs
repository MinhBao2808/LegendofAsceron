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
	//[SerializeField] private AudioClip battleMusic;
	[SerializeField] private GameObject battleMusic;
	[SerializeField] private GameObject gameLevelPanel;
	[SerializeField] private GameObject menuPanel;
	[SerializeField] private GameObject creditPanel;
	[SerializeField] private GameObject settingPanel;
	[SerializeField] private Animator creditAnimator;
	[SerializeField] private Dropdown dropdownResolution;
	[SerializeField] private Button changeFullScreenToWindows;
	[SerializeField] private GameObject loadFileSavePanel;
	[SerializeField] private GameObject loadButtonPrefabs;
    private int countPlayerMove = 0;
	public int[] arrayOfSave = new int[10];
	public int index;//store index of save file
    private Vector3 currentPlayerPosition = new Vector3();
	private bool isSceneMenu;
	public Level level;
	// Fixed aspect ratio parameters
    static public bool FixedAspectRatio = true;
    static public float TargetAspectRatio = 16 / 9f;

    // Windowed aspect ratio when FixedAspectRatio is false
    static public float WindowedAspectRatio = 16f / 9f;

    // List of horizontal resolutions to include
    int[] resolutions = new int[] { 600, 800, 1024, 1280, 1400, 1600, 1920 };

    public Resolution DisplayResolution;
    public List<Vector2> WindowedResolutions, FullscreenResolutions;

    int currWindowedRes, currFullscreenRes;


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
		StartCoroutine(StartRoutine());
	}

	public void GoToBattle () {
        currentPlayerPosition = PlayerMovement.instance.ReturnPlayerPosition();
        //DataManager.instance.playerPosition = currentPlayerPosition;
		battleMusic.SetActive(true);
		SceneManager.LoadScene("2 BattleScene");
    }

	public void LoadDifficultys() {//go to map 
								   //SceneManager.LoadScene(1);
		menuPanel.SetActive(false);
		creditPanel.SetActive(false);
		settingPanel.SetActive(false);
		gameLevelPanel.SetActive(true);
		loadFileSavePanel.SetActive(false);
		//battleMusic.SetActive(false);
	}

	public void LoadCreditPanel() {
		creditPanel.SetActive(true);
		gameLevelPanel.SetActive(false);
		menuPanel.SetActive(false);
		settingPanel.SetActive(false);
		loadFileSavePanel.SetActive(false);
		creditAnimator.Play("creditScoller");
	}

	public void LoadSettingPanel() {
		settingPanel.SetActive(true);
		gameLevelPanel.SetActive(false);
		menuPanel.SetActive(false);
		loadFileSavePanel.SetActive(false);
		creditPanel.SetActive(false);
	}

	public void LoadMenuPanel() {
		creditPanel.SetActive(false);
		gameLevelPanel.SetActive(false);
		settingPanel.SetActive(false);
		loadFileSavePanel.SetActive(false);
		menuPanel.SetActive(true);
	}

	public void LoadLoadGamePanel() {
		loadFileSavePanel.SetActive(true);
		menuPanel.SetActive(false);
		settingPanel.SetActive(false);
		creditPanel.SetActive(false);
		gameLevelPanel.SetActive(false);
		//SaveLoad.LoadMultipleFiles(loadButtonPrefabs,arrayOfSave);
	}

	public void ChangeResolution(int index) {
		//Debug.Log(index);
		SetResolution(index, Screen.fullScreen);
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void LoadMapScene() {
		battleMusic.SetActive(false);
		SceneManager.LoadScene(1);
	}

	//public void ChoosePassiveDifficulty() {//choose passive difficultys
	//	level = Level.Passive;
	//	DataManager.instance.gameLevel = level;
	//	SceneManager.LoadScene(1);
	//}

	//public void ChooseNormalDifficulty() {//choose normal difficulty
	//	level = Level.Normal;
	//	DataManager.instance.gameLevel = level;
	//	SceneManager.LoadScene(1);
	//}

	//public void ChooseAggressiveDifficulty() {//choose aggressive difficulty
	//	level = Level.Aggressive;
	//	DataManager.instance.gameLevel = level;
	//	SceneManager.LoadScene(1);
	//}

	//public void ChooseNightmareDifficulty () {//choose nightmare difficulty
	//	level = Level.Nightmare;
	//	DataManager.instance.gameLevel = level;
	//	SceneManager.LoadScene(1);
	//}
   
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

    //setting resolution

	private IEnumerator StartRoutine() {
        if (Application.platform == RuntimePlatform.OSXPlayer) {
            DisplayResolution = Screen.currentResolution;
        }
        else {
            if (Screen.fullScreen) {
                Resolution r = Screen.currentResolution;
                Screen.fullScreen = false;

                yield return null;
                yield return null;

                DisplayResolution = Screen.currentResolution;

                Screen.SetResolution(r.width, r.height, true);

                yield return null;
            }
            else {
                DisplayResolution = Screen.currentResolution;
            }
        }

        InitResolutions();
		//foreach (Vector2 r in WindowedResolutions) {
		//	Debug.Log(r.x + "x" + r.y);
		//}
    }

    private void InitResolutions() {
        float screenAspect = (float)DisplayResolution.width / DisplayResolution.height;

        WindowedResolutions = new List<Vector2>();
        FullscreenResolutions = new List<Vector2>();

        foreach (int w in resolutions) {
            if (w < DisplayResolution.width) {
                // Adding resolution only if it's 20% smaller than the screen
                if (w < DisplayResolution.width * 0.8f) {
                    Vector2 windowedResolution = new Vector2(w, Mathf.Round(w / (FixedAspectRatio ? TargetAspectRatio : WindowedAspectRatio)));
                    if (windowedResolution.y < DisplayResolution.height * 0.8f)
                        WindowedResolutions.Add(windowedResolution);

                    FullscreenResolutions.Add(new Vector2(w, Mathf.Round(w / screenAspect)));
                }
            }
        }

        // Adding fullscreen native resolution
        FullscreenResolutions.Add(new Vector2(DisplayResolution.width, DisplayResolution.height));

        // Adding half fullscreen native resolution
        Vector2 halfNative = new Vector2(DisplayResolution.width * 0.5f, DisplayResolution.height * 0.5f);
        if (halfNative.x > resolutions[0] && FullscreenResolutions.IndexOf(halfNative) == -1)
            FullscreenResolutions.Add(halfNative);

        FullscreenResolutions = FullscreenResolutions.OrderBy(resolution => resolution.x).ToList();

        bool found = false;

        if (Screen.fullScreen) {
            currWindowedRes = WindowedResolutions.Count - 1;

            for (int i = 0; i < FullscreenResolutions.Count; i++) {
                if (FullscreenResolutions[i].x == Screen.width && FullscreenResolutions[i].y == Screen.height) {
                    currFullscreenRes = i;
                    found = true;
                    break;
                }
            }

			if (!found) {
				SetResolution(FullscreenResolutions.Count - 1, true);
			}
			dropdownResolution.value = currFullscreenRes;
			changeFullScreenToWindows.GetComponentsInChildren<Text>()[0].text = "Windowed";
        }
        else {
            currFullscreenRes = FullscreenResolutions.Count - 1;

            for (int i = 0; i < WindowedResolutions.Count; i++) {
                if (WindowedResolutions[i].x == Screen.width && WindowedResolutions[i].y == Screen.height) {
                    found = true;
                    currWindowedRes = i;
                    break;
                }
            }

			if (!found) {
				SetResolution(WindowedResolutions.Count - 1, false);
			}
			dropdownResolution.value = currWindowedRes;
			changeFullScreenToWindows.GetComponentsInChildren<Text>()[0].text = "Full screen";
        }
    }

	public void SetResolution(int index, bool fullscreen) {
        Vector2 r = new Vector2();
        if (fullscreen) {
            currFullscreenRes = index;
            r = FullscreenResolutions[currFullscreenRes];
        }
        else {
            currWindowedRes = index;
            r = WindowedResolutions[currWindowedRes];
        }

        bool fullscreen2windowed = Screen.fullScreen & !fullscreen;

        //Debug.Log("Setting resolution to " + (int)r.x + "x" + (int)r.y);
        Screen.SetResolution((int)r.x, (int)r.y, fullscreen);

        // On OSX the application will pass from fullscreen to windowed with an animated transition of a couple of seconds.
        // After this transition, the first time you exit fullscreen you have to call SetResolution again to ensure that the window is resized correctly.
        if (Application.platform == RuntimePlatform.OSXPlayer) {
            // Ensure that there is no SetResolutionAfterResize coroutine running and waiting for screen size changes
            StopAllCoroutines();
            // Resize the window again after the end of the resize transition
            if (fullscreen2windowed) StartCoroutine(SetResolutionAfterResize(r));
        }
    }

    private IEnumerator SetResolutionAfterResize(Vector2 r) {
        int maxTime = 5; // Max wait for the end of the resize transition
        float time = Time.time;
        // Skipping a couple of frames during which the screen size will change
        yield return null;
        yield return null;
        int lastW = Screen.width;
        int lastH = Screen.height;
        // Waiting for another screen size change at the end of the transition animation
        while (Time.time - time < maxTime) {
            if (lastW != Screen.width || lastH != Screen.height) {
				Debug.Log("Resize! " + Screen.width + "x" + Screen.height);
                Screen.SetResolution((int)r.x, (int)r.y, Screen.fullScreen);
                yield break;
            }

            yield return null;
        }
    }

    public void ToggleFullscreen() {
        SetResolution(Screen.fullScreen ? currWindowedRes : currFullscreenRes,!Screen.fullScreen);
    }

	//private void OnGUI() {
	//	if (isSceneMenu == true)
	//	{
	//		//Debug.Log("a");
	//		if (Screen.fullScreen)
	//		{
	//			changeFullScreenToWindows.GetComponentsInChildren<Text>()[0].text = "Windowed";
	//		}
	//		else
	//		{
	//			changeFullScreenToWindows.GetComponentsInChildren<Text>()[0].text = "Full screen";
	//		}
	//	}
	//}
}
