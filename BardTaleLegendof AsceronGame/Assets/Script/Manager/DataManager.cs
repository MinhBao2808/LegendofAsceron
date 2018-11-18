using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

using JsonDataClasses;

public class DataManager : MonoBehaviour
{
    public List<EnemyJson> EnemyList { get; private set; }
    public List<CharacterJson> CharacterList { get; private set; }
    public List<WeaponJson> WeaponList { get; private set; }
    public List<ArmorJson> ArmorList { get; private set; }
    public List<UsableJson> UsableList { get; private set; }

    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            Init();
            DontDestroyOnLoad(this);
        }
    }

    void Init()
    {
        EnemyList = new List<EnemyJson>();
        CharacterList = new List<CharacterJson>();
        WeaponList = new List<WeaponJson>();
        ArmorList = new List<ArmorJson>();
        UsableList = new List<UsableJson>();

        var enemyJson = Resources.Load<TextAsset>("JSON/enemies");
        EnemyJson[] tempEnemyList = JsonConvert.DeserializeObject<EnemyJson[]>(enemyJson.text);
        EnemyList.AddRange(tempEnemyList);

        var characterJson = Resources.Load<TextAsset>("JSON/characters");
        CharacterJson[] tempCharacterList = JsonConvert.DeserializeObject<CharacterJson[]>(characterJson.text);
        CharacterList.AddRange(tempCharacterList);

        var weaponJson = Resources.Load<TextAsset>("JSON/weapons");
        WeaponJson[] tempWeaponList = JsonConvert.DeserializeObject<WeaponJson[]>(weaponJson.text);
        WeaponList.AddRange(tempWeaponList);

        for (int i = 0; i < WeaponList.Count; i++)
        {
            WeaponList[i].GenerateDurability();
        }

        var armorJson = Resources.Load<TextAsset>("JSON/armors");
        ArmorJson[] tempArmorList = JsonConvert.DeserializeObject<ArmorJson[]>(armorJson.text);
        ArmorList.AddRange(tempArmorList);

        for (int i = 0; i < ArmorList.Count; i++)
        {
            ArmorList[i].GenerateDurability();
        }

        var usableJson = Resources.Load<TextAsset>("JSON/usables");
        UsableJson[] tempUsableList = JsonConvert.DeserializeObject<UsableJson[]>(usableJson.text);
        UsableList.AddRange(tempUsableList);
    }

    public WeaponJson SearchWeaponID (string id)
    {
        return WeaponList.Find((obj) => obj.id.Equals(id));
    }

    public ArmorJson SearchArmorID(string id)
    {
        return ArmorList.Find((obj) => obj.id.Equals(id));
    }

    public UsableJson SearchUsableID(string id)
    {
        return UsableList.Find((obj) => obj.id.Equals(id));
    }
}
