using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private static PlayerManager _instance;

    public int PlayTime { get; set; }
    public int Difficulty { get; set; }
    public int Currency { get; set; }

    private float timeTick = 0f;
    private const float timeInterval = 1f;

    public static PlayerManager Instance
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

    private void Initialize(int diff)
    {
        PlayTime = 0;
        Difficulty = diff;
        Currency = 0;
    }

    // Update is called once per frame
    void Update () {
		if (timeTick < timeInterval)
        {
            timeTick += Time.deltaTime;
        }
        else
        {
            timeTick = 0;
            PlayTime += 1;
        }
	}
}