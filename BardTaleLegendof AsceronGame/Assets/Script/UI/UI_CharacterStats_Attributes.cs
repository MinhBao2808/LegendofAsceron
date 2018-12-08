using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterStats_Attributes : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI currentValue;
    [SerializeField]
    private TextMeshProUGUI changedValue;
    [SerializeField]
    private Button button;

    int add;
    
    void Awake()
    {
        add = 0;
        currentValue = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        changedValue = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        button = transform.GetChild(3).GetComponent<Button>();
        currentValue.text = "";
        changedValue.text = "";
    }

    public void OnShow(int value, int attribute)
    {
        currentValue.text = value.ToString();
        if (attribute == 0)
        {
            changedValue.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
        else
        {
            changedValue.gameObject.SetActive(true);
            button.gameObject.SetActive(true);
            changedValue.text = "";
        }
    }

    public void RefreshAll()
    {
        add = 0;
        changedValue.text = "";
    }

    public void OnButtonClick(ref int attributes)
    {
        add++;
        attributes--;
        changedValue.text = "+" + add;
    }

    public int GetAddValue()
    {
        return add;
    }
}
