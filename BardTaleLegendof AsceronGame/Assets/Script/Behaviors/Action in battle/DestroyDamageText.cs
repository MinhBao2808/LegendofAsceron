using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDamageText : MonoBehaviour {
    [SerializeField] private float destroyTime;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, this.destroyTime);
	}
}
