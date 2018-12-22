using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JsonDataClasses;

public class SkillPanelItem : MonoBehaviour {

    [SerializeField]
    Button button;
    [SerializeField]
    Image icon;
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI descriptionText;

    private string skillId;
    public UI_Character_Skill parent;

    public void OnShow(string skillid, int availableSkillPoint)
    {
        skillId = skillid;
        SkillJson skill = DataManager.Instance.SearchSkillID(skillid);
        if (skill != null)
        {
            icon.sprite = Resources.Load<Sprite>(skill.imgPath);
            nameText.text = skill.name;
            descriptionText.text = skill.tooltips;
        }
        button.interactable = (availableSkillPoint > 0) ? true : false;
    }
    
    public void OnClick()
    {
        parent.OnClickItem(skillId);
    }
}
