using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    UI_CharacterItem characterItem_01;
    [SerializeField]
    UI_CharacterItem characterItem_02;
    [SerializeField]
    UI_CharacterItem characterItem_03;
    [SerializeField]
    TextMeshProUGUI playtimeText;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        pauseMenu.SetActive(false);
        characterItem_01.gameObject.SetActive(false);
        characterItem_02.gameObject.SetActive(false);
        characterItem_03.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        playtimeText.text = "Play Time: " + PlayerManager.Instance.PlayTime;
        if(hInput.GetButtonUp("Back") && SceneManager.GetActiveScene().name != "M0000-Menu")
        {
            characterItem_01.gameObject.SetActive(!pauseMenu.activeSelf);
            characterItem_01.UpdateImage();
            if (PlayerManager.Instance.PartyMemberID.Count > 1)
            {
                characterItem_02.gameObject.SetActive(!pauseMenu.activeSelf);
                characterItem_02.UpdateImage();
            }
            else 
            {
                characterItem_02.gameObject.SetActive(false);
            }
            if(PlayerManager.Instance.PartyMemberID.Count > 2)
            {
                characterItem_03.gameObject.SetActive(!pauseMenu.activeSelf);
                characterItem_03.UpdateImage();
            }
            else 
            {
                characterItem_03.gameObject.SetActive(false);
            }
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
	}
}
