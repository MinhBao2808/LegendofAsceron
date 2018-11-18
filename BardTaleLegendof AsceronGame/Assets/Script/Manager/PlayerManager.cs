using System;
using System.Collections.Generic;
using UnityEngine;

using JsonDataClasses;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private static PlayerManager _instance;

    public int PlayTime { get; set; }
    public int Difficulty { get; set; }
    public List<PlayerWeapon> Weapons { get; private set; }
    public List<PlayerArmor> Armors { get; private set; }
    public List<UsableJson> Usables { get; private set; }
    public int Currency { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    //Rotation as Euler Angles
    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }
    public string CurrentSceneID { get; set; }
    public List<PlayerCharacter> Characters { get; private set; }

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

    public void Initialize(int diff)
    {
        PlayTime = 0;
        Difficulty = diff;
        Weapons = new List<PlayerWeapon>();
        Armors = new List<PlayerArmor>();
        Currency = 0;
        Characters = new List<PlayerCharacter>
        {
            new PlayerCharacter(DataManager.Instance.CharacterList[0], 1)
        };
        PosX = 0;
        PosY = 0;
        PosZ = 0;
        RotX = 0;
        RotY = 0;
        RotZ = 0;
    }

    public void AddCharacter(CharacterJson character, int level)
    {
        Characters.Add(new PlayerCharacter(character, level));
    }

    public void AddItem(string type, string id)
    {
        if (type.ToLower() == "usable")
        {
            Usables.Add(DataManager.Instance.SearchUsableID(id));
        }
        else if (type.ToLower() == "weapon")
        {
            Weapons.Add(new PlayerWeapon(id));
        }
        else if (type.ToLower() == "armor")
        {
            Armors.Add(new PlayerArmor(id));
        }
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
        Weapons = data.weapons;
        Armors = data.armors;
        Usables = data.usables;
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
    public List<PlayerWeapon> weapons;
    public List<PlayerArmor> armors;
    public List<UsableJson> usables;
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
        currency = PlayerManager.Instance.Currency;
        characters = PlayerManager.Instance.Characters;
        weapons = PlayerManager.Instance.Weapons;
        armors = PlayerManager.Instance.Armors;
        usables = PlayerManager.Instance.Usables;
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
public class PlayerCharacter
{
    public int level;
    public int experience;
    public int availableAttributes;
    public int availableSkillPoints;
    public PlayerCharBattleStat battleStats;
    public UnitJson info;
    public PlayerWeapon weapon;
    public List<PlayerArmor> armors;

    public PlayerCharacter(CharacterJson json, int targetLevel)
    {
        info = new UnitJson
        {
            id = json.id,
            name = json.name,
            description = json.description,
            race = json.race,
            type = json.type,
            skills = json.skills,
            totalSkills = json.totalSkills,
            stats = new UnitStatJson()
        };

        if (!String.IsNullOrEmpty(json.weaponID))
        {
            weapon = new PlayerWeapon(json.weaponID);
        }

        if (json.armorIDs.Length > 0)
        {
            armors = new List<PlayerArmor>();
            for (int i = 0; i < json.armorIDs.Length; i++)
            {
                armors.Add(new PlayerArmor(json.armorIDs[i]));
            }
        }

        SetStatByLevel(targetLevel, json);

        availableAttributes = 0;
        availableSkillPoints = 0;

        CalculateBattleStat();
        RefreshStatus();
    }

    public void AddExperience(int exp)
    {
        experience += exp;
        int nextLevelExp = Expression.GetExpExpression(level + 1);
        if (experience >= nextLevelExp)
        {
            experience = experience - nextLevelExp;
            IncreaseLevel(1);
            CalculateBattleStat();
            RefreshStatus();
        }
    }

    public void RefreshStatus()
    {
        battleStats.hp = battleStats.maxHp;
        battleStats.mp = battleStats.maxMp;
    }

    public void SetHp(float targetHp)
    {
        battleStats.hp = (targetHp >= battleStats.maxHp) ? battleStats.maxHp : targetHp;
    }

    public void SetMp(float targetMp)
    {
        battleStats.mp = (targetMp >= battleStats.maxMp) ? battleStats.maxMp : targetMp;
    }

    void AddWeaponStat()
    {
        if (weapon.durability > 0)
        {

            battleStats.maxHp += weapon.weapon.stats.vitality * GameConfigs.HP_PER_VIT;
            battleStats.maxMp += weapon.weapon.stats.wisdom * GameConfigs.MP_PER_WIS;

            battleStats.eva += weapon.weapon.stats.dexterity * GameConfigs.EVA_PER_DEX;
            battleStats.crit += weapon.weapon.stats.dexterity * GameConfigs.CRIT_PER_DEX;
            battleStats.spd += weapon.weapon.stats.dexterity * GameConfigs.SPD_PER_DEX;

            battleStats.patk += weapon.weapon.patk + weapon.weapon.stats.strength * GameConfigs.PATK_PER_STR;
            battleStats.matk += weapon.weapon.matk + weapon.weapon.stats.intelligence * GameConfigs.MATK_PER_INT;

            battleStats.pdef += weapon.weapon.stats.endurance * GameConfigs.PDEF_PER_END;
            battleStats.mdef += weapon.weapon.stats.wisdom * GameConfigs.MDEF_PER_WIS;

            battleStats.fireRes += weapon.weapon.stats.fireRes;
            battleStats.lightningRes += weapon.weapon.stats.lightningRes;
            battleStats.iceRes += weapon.weapon.stats.iceRes;
            battleStats.holyRes += weapon.weapon.stats.holyRes;
            battleStats.darkRes += weapon.weapon.stats.darkRes;

            if (weapon.weapon.modification != null)
            {
                battleStats.maxHp += weapon.weapon.modification.hp;
                battleStats.maxMp += weapon.weapon.modification.mp;

                battleStats.eva += weapon.weapon.modification.evasion;
                battleStats.crit += weapon.weapon.modification.crit;
                battleStats.spd += weapon.weapon.modification.speed;

                battleStats.patk += weapon.weapon.modification.patk;
                battleStats.matk += weapon.weapon.modification.matk;

                battleStats.pdef += weapon.weapon.modification.pdef;
                battleStats.mdef += weapon.weapon.modification.mdef;
            }
        }
    }

    void AddArmorStat(PlayerArmor piece)
    {
        if (piece.durability > 0)
        {
            battleStats.maxHp += piece.armor.stats.vitality * GameConfigs.HP_PER_VIT;
            battleStats.maxMp += piece.armor.stats.wisdom * GameConfigs.MP_PER_WIS;

            battleStats.eva += piece.armor.stats.dexterity * GameConfigs.EVA_PER_DEX;
            battleStats.crit += piece.armor.stats.dexterity * GameConfigs.CRIT_PER_DEX;
            battleStats.spd += piece.armor.stats.dexterity * GameConfigs.SPD_PER_DEX;

            battleStats.patk += piece.armor.stats.strength * GameConfigs.PATK_PER_STR;
            battleStats.matk += piece.armor.stats.intelligence * GameConfigs.MATK_PER_INT;

            battleStats.pdef += piece.armor.pdef + piece.armor.stats.endurance * GameConfigs.PDEF_PER_END;
            battleStats.mdef += piece.armor.mdef + piece.armor.stats.wisdom * GameConfigs.MDEF_PER_WIS;

            battleStats.fireRes += piece.armor.stats.fireRes;
            battleStats.lightningRes += piece.armor.stats.lightningRes;
            battleStats.iceRes += piece.armor.stats.iceRes;
            battleStats.holyRes += piece.armor.stats.holyRes;
            battleStats.darkRes += piece.armor.stats.darkRes;

            if (piece.armor.modification != null)
            {
                battleStats.maxHp += piece.armor.modification.hp;
                battleStats.maxMp += piece.armor.modification.mp;

                battleStats.eva += piece.armor.modification.evasion;
                battleStats.crit += piece.armor.modification.crit;
                battleStats.spd += piece.armor.modification.speed;

                battleStats.patk += piece.armor.modification.patk;
                battleStats.matk += piece.armor.modification.matk;

                battleStats.pdef += piece.armor.modification.pdef;
                battleStats.mdef += piece.armor.modification.mdef;
            }
        }
    }

    public void CalculateBattleStat()
    {
        battleStats = new PlayerCharBattleStat();

        Debug.Log(info.stats);

        battleStats.maxHp += info.stats.hp + info.stats.vitality * GameConfigs.HP_PER_VIT;
        battleStats.maxMp += info.stats.mp + info.stats.wisdom * GameConfigs.MP_PER_WIS;

        battleStats.eva += info.stats.dexterity * GameConfigs.EVA_PER_DEX;
        battleStats.crit += info.stats.dexterity * GameConfigs.CRIT_PER_DEX;
        battleStats.spd += info.stats.dexterity * GameConfigs.SPD_PER_DEX;

        battleStats.patk += info.stats.strength * GameConfigs.PATK_PER_STR;
        battleStats.matk += info.stats.intelligence * GameConfigs.MATK_PER_INT;

        battleStats.pdef += info.stats.endurance * GameConfigs.PDEF_PER_END;
        battleStats.mdef += info.stats.wisdom * GameConfigs.MDEF_PER_WIS;

        if (weapon != null)
        {
            AddWeaponStat();
        }

        if (armors != null)
        {
            for (int i = 0; i < armors.Count && armors[i].armor != null; i++)
            {
                AddArmorStat(armors[i]);
            }
        }

        battleStats.eva = (battleStats.eva <= GameConfigs.MAX_EVA_PERCENTAGE) ?
            battleStats.eva : GameConfigs.MAX_EVA_PERCENTAGE;
        battleStats.crit = (battleStats.crit <= GameConfigs.MAX_CRIT_PERCENTAGE) ?
            battleStats.crit : GameConfigs.MAX_CRIT_PERCENTAGE;
    }

    public void IncreaseAttributes(int amount)
    {
        availableAttributes += amount;
    }

    public void IncreaseSkillPoints(int amount)
    {
        availableSkillPoints += amount;
    }

    public void IncreaseLevel(int increaseAmount)
    {
        level += increaseAmount;
        IncreaseAttributes(GameConfigs.ATTRIBUTES_PER_LVL * increaseAmount);
        IncreaseSkillPoints(GameConfigs.SKILLPOINTS_PER_LVL * increaseAmount);
    }

    public void SetStatByLevel(int lvl, CharacterJson character)
    {
        if (lvl > 0)
        {
            level = lvl;
            experience = 0;
            info.stats.hp = character.stats.hp + (character.growthStat.hp * (lvl - 1));
            info.stats.mp = character.stats.mp + (character.growthStat.mp * (lvl - 1));
            info.stats.strength = character.stats.strength + (character.growthStat.strength * (lvl - 1));
            info.stats.dexterity = character.stats.dexterity + (character.growthStat.dexterity * (lvl - 1));
            info.stats.intelligence = character.stats.intelligence + (character.growthStat.intelligence * (lvl - 1));
            info.stats.vitality = character.stats.vitality + (character.growthStat.vitality * (lvl - 1));
            info.stats.wisdom = character.stats.wisdom + (character.growthStat.wisdom * (lvl - 1));
            info.stats.endurance = character.stats.endurance + (character.growthStat.endurance * (lvl - 1));
        }
    }
}

[Serializable]
public class PlayerWeapon 
{
    public WeaponJson weapon;
    public int durability;

    public PlayerWeapon(string weaponID)
    {
        weapon = DataManager.Instance.SearchWeaponID(weaponID);
        weapon.GenerateDurability();
        durability = weapon.MaxDurability;
    }
}

[Serializable]
public class PlayerArmor
{
    public ArmorJson armor;
    public int durability;

    public PlayerArmor(string armorID)
    {
        armor = DataManager.Instance.SearchArmorID(armorID);
        armor.GenerateDurability();
        durability = armor.MaxDurability;
    }
}

[Serializable]
public class PlayerUsable
{
    public UsableJson usable;
    public int amount;
}

[Serializable]
public class PlayerCharBattleStat
{
    public float maxHp;
    public float maxMp;
    public float hp;
    public float mp;
    public float patk;
    public float matk;
    public float pdef;
    public float mdef;
    public float spd;
    public float eva;
    public float crit;
    public float fireRes;
    public float lightningRes;
    public float iceRes;
    public float holyRes;
    public float darkRes;

    public PlayerCharBattleStat()
    {
        maxHp = 0;
        maxMp = 0;
        hp = 0;
        mp = 0;
        patk = 0;
        matk = 0;
        pdef = 0;
        mdef = 0;
        spd = 0;
        eva = 0;
        crit = 0;
        fireRes = 0;
        lightningRes = 0;
        iceRes = 0;
        holyRes = 0;
        darkRes = 0;
    }
}