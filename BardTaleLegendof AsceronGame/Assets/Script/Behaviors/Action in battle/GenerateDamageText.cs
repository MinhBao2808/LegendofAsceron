using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ORKFramework;
using ORKFramework.Behaviours;

public class GenerateDamageText : MonoBehaviour {
	[SerializeField] private GameObject damageText;
	[SerializeField] private float damageTextOffset;
	private GameObject damageTextObject;
	private int maxHealth;
	private int currentHealth;

	public void ReceiveDamage(int damage) {
		CombatantComponent combatantComponent = gameObject.GetComponent<CombatantComponent>();
        Combatant unit = combatantComponent.combatant;
		currentHealth = unit.Status[1].GetValue();
		currentHealth -= damage;
		unit.Status[1].InitValue(currentHealth);
        GameObject canvas = GameObject.Find("Canvas");
        damageTextObject = Instantiate(this.damageText) as GameObject;
		damageTextObject.transform.SetParent(canvas.transform, false);
        damageTextObject.GetComponent<Text>().text = "" + damage;

		damageTextObject.transform.position = new Vector3(transform.position.x + damageTextOffset,
		                                                  transform.position.y + 1.0f,transform.position.z);
		if (currentHealth <= 0) {
			if (this.gameObject.tag == "Enemy") {
				BattleManager.instance.enemyList.Remove(unit);
				this.gameObject.tag = "DeadUnit";
				BattleManager.instance.isEnemyDead[BattleManager.instance.enemySelectedPositionIndex] = true;
				this.gameObject.SetActive(false);
			}
			else {
				this.gameObject.tag = "DeadUnit";
			}
        }
    }
}
