using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour {

    public Animator anim;

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

    public void TriggerLoadingFadeOut(int screenIndex)
    {
        anim.SetTrigger("LoadingFadeOut");
        level = screenIndex;
    }

    void TriggerIdle()
    {
        anim.SetTrigger("Idle");
    }

    public void TriggerBattleFadeOut()
    {
        anim.SetTrigger("BattleFadeOut");
        level = 2;
    }

    void OnLoadComplete(string trigger)
    {
        StartCoroutine(LoadAsynchronously(level, trigger));
    }

    IEnumerator LoadAsynchronously(int sceneIndex, string trigger)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            yield return null;
        }
        anim.SetTrigger(trigger);
        AudioManager.Instance.ChangeBgm(AudioManager.Instance.normalBgms[1]);
    }
    
}
