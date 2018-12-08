using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterStats_Equipments : MonoBehaviour {

    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI equipmentName;

    // Use this for initialization
    void Start () {
        image = transform.GetChild(0).GetComponent<Image>();
        equipmentName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
	
    public void OnShowWeapon(PlayerWeapon weapon)
    {
        if (weapon != null)
        {
            if (weapon.weapon != null)
            {
                image.gameObject.SetActive(true);
                image.sprite = Resources.Load<Sprite>(weapon.weapon.imgPath);
                image.preserveAspect = true;
                equipmentName.text = weapon.weapon.name;
            }
            else
            {
                image.gameObject.SetActive(false);
                equipmentName.text = "None";
            }
        }
        else
        {
            image.gameObject.SetActive(false);
            equipmentName.text = "None";
        }
    }

    public void OnShowArmor(PlayerArmor armor)
    {
        if (armor != null)
        {
            if (armor.armor != null)
            {
                image.gameObject.SetActive(true);
                image.sprite = Resources.Load<Sprite>(armor.armor.imgPath);
                image.preserveAspect = true;
                equipmentName.text = armor.armor.name;
            }
            else
            {
                image.gameObject.SetActive(false);
                equipmentName.text = "None";
            }
        }
        else
        {
            image.gameObject.SetActive(false);
            equipmentName.text = "None";
        }
    }
}
