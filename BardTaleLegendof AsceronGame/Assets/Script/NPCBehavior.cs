using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCBehavior : MonoBehaviour {

    [SerializeField]
    string id;
    [SerializeField]
    string selectionId;

    private void OnMouseEnter()
    {
        OnInteract();
    }

    public void OnInteract()
    {
        InteractController.Instance.OnShowSelection(selectionId);
    }
}
