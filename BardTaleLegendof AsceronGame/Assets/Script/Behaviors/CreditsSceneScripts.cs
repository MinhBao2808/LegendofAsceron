using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSceneScripts : MonoBehaviour {

    public GameObject optionsMenu;

    public void OnFinishAnimation()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
