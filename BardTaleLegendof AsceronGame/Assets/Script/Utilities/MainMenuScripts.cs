using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.UI;

public class MainMenuScripts : MonoBehaviour {
    public Button continueButton;

    private void Start()
    {
        if(SaveLoadManager.Instance.LoadAllSavefile() == null)
        {
            continueButton.gameObject.SetActive(false);
        }
        else
        {
            continueButton.gameObject.SetActive(true);
        }
    }

=======
using ORKFramework;

public class MainMenuScripts : MonoBehaviour {
>>>>>>> 837ac71b9e536431bec2c512bedd9543ba17495d
    public void QuitGame()
    {
        Application.Quit();
    }
}
