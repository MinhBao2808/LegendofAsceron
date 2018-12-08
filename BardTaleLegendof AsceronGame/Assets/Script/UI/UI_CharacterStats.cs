using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterStats : MonoBehaviour {

    #region Info
    [SerializeField]
    private Image face;
    [SerializeField]
    private TextMeshProUGUI charName;
    [SerializeField]
    private Slider hp;
    [SerializeField]
    private Slider mp;
    [SerializeField]
    private Slider exp;
    [SerializeField]
    private TextMeshProUGUI level;
    [SerializeField]
    private TextMeshProUGUI toNextLevel;
    #endregion

    #region Attributes
    [SerializeField]
    private UI_CharacterStats_Attributes str;
    [SerializeField]
    private UI_CharacterStats_Attributes dex;
    [SerializeField]
    private UI_CharacterStats_Attributes intel;
    [SerializeField]
    private UI_CharacterStats_Attributes vit;
    [SerializeField]
    private UI_CharacterStats_Attributes end;
    [SerializeField]
    private UI_CharacterStats_Attributes wis;
    [SerializeField]
    private TextMeshProUGUI availableText;
    [SerializeField]
    private TextMeshProUGUI points;
    [SerializeField]
    private Button confirm;
    #endregion
    string charId;
    PlayerCharacter character;

	// Use this for initialization
	void Start () {
        character = null;
        gameObject.SetActive(false);
	}
	
    public void Unhide(string id)
    {
        charId = id;
        SetCharacter(PlayerManager.Instance.SearchCharacterById(id));
        if (character != null)
        {
            UpdateInfo();
            UpdateAttributes();
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Character not found!");
        }
    }

    void SetCharacter(PlayerCharacter target)
    {
        character = target;
    }

    void UpdateInfo()
    {
        face.sprite = Resources.Load<Sprite>(character.info.faceImgPath);
        charName.text = character.info.name;
        hp.value = character.battleStats.hp / character.battleStats.maxHp;
        mp.value = character.battleStats.mp / character.battleStats.maxMp;
        int nextLevelExp = Expression.GetExpExpression(character.level + 1);
        exp.value = character.experience / nextLevelExp;
        toNextLevel.text = "To Next Level: " + (nextLevelExp - character.experience);
    }

    public void PreviewAttribute(string type, int amount)
    {

    }

    void UpdateAttributes()
    {
        int pos = PlayerManager.Instance.ReturnCharacterPosById(charId);
        if (pos != -1)
        {
            PlayerManager.Instance.Characters[pos] = character;
        }
        else
        {
            Debug.LogError("Character with Id " + charId + " is not found in Player Character List.");
        }
    }
}
