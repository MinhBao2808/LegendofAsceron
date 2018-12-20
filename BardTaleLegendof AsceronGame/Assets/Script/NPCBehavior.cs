using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCBehavior : MonoBehaviour {

    [SerializeField]
    string id;
    [SerializeField]
    string selectionId;

    public void OnMouseDown()
    {
        float distance = Vector3.Distance(PlayerManager.Instance.player.transform.position, transform.position);
        Debug.Log(distance);
        if (distance <= 1.5f)
        {
            OnInteract();
        }
    }

    public void OnInteract()
    {
        InteractController.Instance.OnShowSelection(selectionId);
    }
}
