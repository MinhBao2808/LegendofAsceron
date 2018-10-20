using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour {

    public Animator anim;
    public Image loadingScreenArtwork;
    public Sprite[] artworkSprites;

    private bool isLoading = false;

    private float countTimer = 5;
    private float intervalTime = 5;

    private static ScreenManager _instance;
    private int level;

    public static ScreenManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        if (isLoading)
        {
            if (countTimer < intervalTime)
            {
                countTimer += Time.deltaTime;
            }
            else
            {
                countTimer = 0;
                intervalTime = Random.Range(5f, 10f);
                int randomSpriteIndex = Random.Range(0, artworkSprites.Length);
                loadingScreenArtwork.sprite = artworkSprites[randomSpriteIndex];
            }
        }
    }

    public void TriggerLoadingFadeOut(int screenIndex)
    {
        anim.SetTrigger("LoadingFadeOut");
        level = screenIndex;
        isLoading = true;
    }

    void TriggerIdle()
    {
        anim.SetTrigger("Idle");
        isLoading = false;
    }

    public void TriggerBattleFadeOut()
    {
        anim.SetTrigger("BattleFadeOut");
        level = 2;
    }

    void OnLoadComplete(string trigger)
    {
		Debug.Log(trigger);
        StartCoroutine(LoadAsynchronously(level, trigger));
    }

    IEnumerator LoadAsynchronously(int sceneIndex, string trigger)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            yield return null;
        }
		anim.SetTrigger(trigger);
        if (level == 2)
        {
            AudioManager.Instance.ChangeBgm(AudioManager.Instance.battleBgms[0]);
        }
        else
        {
            AudioManager.Instance.ChangeBgm(AudioManager.Instance.normalBgms[1]);
        }
    }
    
}
