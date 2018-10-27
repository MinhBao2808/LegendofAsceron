using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

using JsonDataClasses;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    public List<EnemyJson> enemyList = new List<EnemyJson>();
    public List<CharacterJson> characterList = new List<CharacterJson>();

    public static DataManager Instance
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
            Init();
            DontDestroyOnLoad(this);
        }
    }

    void Init()
    {
        var enemyJson = Resources.Load<TextAsset>("JSON/enemies");
        EnemyJson[] tempEnemyList = JsonConvert.DeserializeObject<EnemyJson[]>(enemyJson.text);
        enemyList.AddRange(tempEnemyList);
        var characterJson = Resources.Load<TextAsset>("JSON/characters");
        CharacterJson[] tempCharacterList = JsonConvert.DeserializeObject<CharacterJson[]>(characterJson.text);
        characterList.AddRange(tempCharacterList);
    }
}
