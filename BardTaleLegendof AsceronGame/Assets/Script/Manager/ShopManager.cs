using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JsonDataClasses;

public class ShopManager : MonoBehaviour {

    public static ShopManager Instance { get; private set; }

    [SerializeField]
    ShopItem shopItemPrefs;
    [SerializeField]
    GameObject viewPortContent;
    [SerializeField]
    GameObject confirmPanel;
    [SerializeField]
    Button yesButton;
    [SerializeField]
    Button noButton;
    [SerializeField]
    GameObject OnHoverDetailPanel;
    [SerializeField]
    TextMeshProUGUI itemName;
    [SerializeField]
    TextMeshProUGUI itemStat;
    [SerializeField]
    TextMeshProUGUI itemDescription;
    [SerializeField]
    TextMeshProUGUI itemPrice;

    private string itemId;
    private ItemJson itemJson;
    private List<ShopItem> shopItems;
    private bool isItemClicked;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        shopItems = new List<ShopItem>();
        for (int i = 0; i < 30; i++)
        {
            ShopItem temp = Instantiate(shopItemPrefs);
            shopItems.Add(temp);
        }
    }

    public void OnShow(string[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            shopItems[i].transform.parent = viewPortContent.transform;
            ItemJson item;
            if (items[i].ToLower().Contains("iw"))
            {
                WeaponJson weapon = DataManager.Instance.SearchWeaponID(items[i]);
                item = weapon;
            }
            else if (items[i].ToLower().Contains("ia"))
            {
                ArmorJson armor = DataManager.Instance.SearchArmorID(items[i]);
                item = armor;
            }
            else
            {
                item = DataManager.Instance.SearchUsableID(items[i]);
            }
            Sprite sprite = Resources.Load<Sprite>(item.imgPath);
            shopItems[i].OnShow(sprite, item.id);
        }
    }

    public void OnClose()
    {

    }

    public void OnHoverItem(string itemid)
    {
        if (!isItemClicked)
        {
            OnHoverDetailPanel.SetActive(true);
            ItemJson item;
            if (itemid.Contains("iw"))
            {
                WeaponJson weapon = DataManager.Instance.SearchWeaponID(itemid);
                item = weapon;
                itemStat.text = "Require level: " + item.lvlRequirement + "\n" +
                    "Type: " + item.type.ToString() + "\n" +
                    "PATK: " + weapon.patk + "\n" +
                    "MATK: " + weapon.matk;
            }
            else if (itemid.Contains("ia"))
            {
                ArmorJson armor = DataManager.Instance.SearchArmorID(itemid);
                item = armor;
                itemStat.text = "Require level: " + item.lvlRequirement + "\n" +
                    "Type: " + item.type.ToString() + "\n" +
                    "PATK: " + armor.pdef + "\n" +
                    "MATK: " + armor.mdef;
            }
            else
            {
                item = DataManager.Instance.SearchUsableID(itemid);
                itemStat.text = "";
            }
            itemDescription.text = item.tooltips;
            itemName.text = item.name;
            itemPrice.text = item.gold + " Gold";
            itemId = itemid;
            itemJson = item;
        }
    }

    public void OnHoverOut()
    {
        OnHoverDetailPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    public void OnClickItem()
    {
        isItemClicked = false;
        confirmPanel.SetActive(true);
        isItemClicked = true;
    }

    public void OnYes()
    {
       if (PlayerManager.Instance.Currency >= itemJson.gold)
       {
            PlayerManager.Instance.Currency -= itemJson.gold;
            if (itemJson.id.Contains("iw"))
            {
                PlayerManager.Instance.AddItem("weapon", itemJson.id, 1);
            }
            else if (itemJson.id.Contains("ia"))
            {
                PlayerManager.Instance.AddItem("armor", itemJson.id, 1);
            }
            else
            {
                PlayerManager.Instance.AddItem("usable", itemJson.id, 1);
            }
       }
    }

    public void OnNo()
    {
        confirmPanel.SetActive(false);
        OnHoverDetailPanel.SetActive(false);
        isItemClicked = false;
    }
}
