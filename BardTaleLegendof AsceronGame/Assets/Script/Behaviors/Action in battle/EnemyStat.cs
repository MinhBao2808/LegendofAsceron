using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour {
	public EnemyUnit enemy;

	public void Init (int index) {
		enemy = new EnemyUnit(DataManager.Instance.EnemyList[index]);
	}
}
