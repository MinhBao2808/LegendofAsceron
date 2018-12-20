using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour {

    [SerializeField]
    Button button;
    [SerializeField]
    Image image;

    string itemId;

    private void Awake()
    {
        button.onClick.AddListener(()=>ShopManager.Instance.OnClickItem());
    }

    public void OnShow(Sprite img, string id)
    {
        image.sprite = img;
        itemId = id;
    }

    public void OnMouseEnter()
    {
        ShopManager.Instance.OnHoverItem(itemId);
    }

    public void OnMouseExit()
    {
        ShopManager.Instance.OnHoverOut();
    }
}
