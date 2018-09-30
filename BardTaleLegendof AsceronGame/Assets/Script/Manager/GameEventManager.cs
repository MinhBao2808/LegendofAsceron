using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour {

    public string[] chapters = { "0", "1", "2" };
    public int currentChapter = 1;
    private static GameEventManager _instance;

    public static GameEventManager Instance
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

    // Update is called once per frame
    void Update () {
		
	}
}
