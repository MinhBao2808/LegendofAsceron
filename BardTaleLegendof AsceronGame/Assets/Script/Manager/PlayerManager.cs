using System;
using System.Collections.Generic;
using UnityEngine;

using JsonDataClasses;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private static PlayerManager _instance;

    public int PlayTime { get; set; }
    public int Difficulty { get; set; }
    public List<ItemJson> Inventory { get; set; }
    public int Currency { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    //Rotation as Euler Angles
    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }
    public string CurrentSceneID { get; set; }

    private List<PlayerCharacter> characters;
    public List<PlayerCharacter> Characters
    {
        get { return characters; }
        set { characters = value; }
    }

    public GameObject player;
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
            StartCoroutine(UpdatePlayerValue());
            DontDestroyOnLoad(this);
        }
    }

    IEnumerator UpdatePlayerValue()
    {
        while(true)
        {
            PlayTime += 1;
            PosX = player.transform.position.x;
            PosY = player.transform.position.y;
            PosZ = player.transform.position.z;
            RotX = player.transform.rotation.eulerAngles.x;
            RotY = player.transform.rotation.eulerAngles.y;
            RotZ = player.transform.rotation.eulerAngles.z;
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    /*
    void Update () {
		if (timeTick < timeInterval)
        {
            timeTick += Time.deltaTime;
        }
        else
        {
            timeTick = 0;
            PlayTime += 1;
            PosX = player.transform.position.x;
            PosY = player.transform.position.y;
            PosZ = player.transform.position.z;
            RotX = player.transform.rotation.eulerAngles.x;
            RotY = player.transform.rotation.eulerAngles.y;
            RotZ = player.transform.rotation.eulerAngles.z;
        }
    }*/

    public void Initialize(int diff)
    {
        PlayTime = 0;
        Difficulty = diff;
        Inventory = new List<ItemJson>();
        Currency = 0;
        Characters = new List<PlayerCharacter>
        {
            new PlayerCharacter(DataManager.Instance.characterList[0])
        };
        PosX = 0;
        PosY = 0;
        PosZ = 0;
        RotX = 0;
        RotY = 0;
        RotZ = 0;
    }

    public void AddCharacter(CharacterJson character)
    {
        characters.Add(new PlayerCharacter(character));
    }

    public void SetPlayerPos()
    {
        Debug.Log(new Vector3(PosX, PosY, PosZ));
        player.transform.position = new Vector3(PosX,PosY,PosZ);
        player.transform.rotation = Quaternion.Euler(RotX,RotY,RotZ);
    }

    public void SetPlayerData(PlayerData data)
    {
        Difficulty = data.difficulty;
        Currency = data.currency;
        PlayTime = data.playTime;
        Characters = data.characters;
        Inventory = data.inventory;
        PosX = data.posX;
        PosY = data.posY;
        PosZ = data.posZ;
        RotX = data.rotX;
        RotY = data.rotY;
        RotZ = data.rotZ;
        CurrentSceneID = data.sceneID;
    }

    public void UpdateCurrentSceneID(string scene)
    {
        CurrentSceneID = scene;
    }
}

[Serializable]
public class PlayerData
{
    public int playTime;
    public int difficulty;
    public List<PlayerCharacter> characters;
    public List<ItemJson> inventory;
    public int currency;
    public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;
    public string sceneID;

    public PlayerData()
    {
        playTime = PlayerManager.Instance.PlayTime;
        difficulty = PlayerManager.Instance.Difficulty;
        inventory = PlayerManager.Instance.Inventory;
        currency = PlayerManager.Instance.Currency;
        characters = PlayerManager.Instance.Characters;
        posX = PlayerManager.Instance.PosX;
        posY = PlayerManager.Instance.PosY;
        posZ = PlayerManager.Instance.PosZ;
        rotX = PlayerManager.Instance.RotX;
        rotY = PlayerManager.Instance.RotY;
        rotZ = PlayerManager.Instance.RotZ;
        sceneID = PlayerManager.Instance.CurrentSceneID;
    }
}

[Serializable]
public class PlayerCharacter : CharacterJson
{
    public int level;
    public int experience;
    public int availableAttributes;
    public int availableSkillPoints;

    public PlayerCharacter(CharacterJson json)
    {
        id = json.id;
        name = json.name;
        accessories = json.accessories;
        description = json.description;
        race = json.race;
        type = json.type;
        baseStat = json.baseStat;
        skills = json.skills;
        totalSkills = json.totalSkills;
        growthStat = json.growthStat;
        level = 1;
        experience = 0;
        availableAttributes = 0;
        availableSkillPoints = 0;
    }

    public void AddExperience(int exp)
    {
        experience += exp;
        int nextLevelExp = Expression.GetExpExpression(level + 1);
        if (experience >= nextLevelExp)
        {
            experience = experience - nextLevelExp;
            IncreaseLevel();
            IncreaseAttributes();
            IncreaseSkillPoints();
        }
    }

    public void IncreaseAttributes()
    {
        availableAttributes += 6;
    }

    public void IncreaseSkillPoints()
    {
        availableSkillPoints += 1;
    }

    public void IncreaseLevel()
    {
        level++;
    }

    public void SetLevel(int lvl)
    {
        level = lvl;
        experience = 0;
    }
}