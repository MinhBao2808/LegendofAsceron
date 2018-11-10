using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateDamageText : MonoBehaviour {
	[SerializeField] private GameObject damageText;
	[SerializeField] private float damageTextOffset;
	private GameObject damageTextObject;
	private float maxHealth;
	private float currentHealth;

	public void ReceiveDamage(float damage) {
		if (gameObject.tag == "PlayerUnit") {
			currentHealth = this.gameObject.GetComponent<PlayerStat>().player.baseStat.hp;
			currentHealth -= damage;
			this.gameObject.GetComponent<PlayerStat>().player.baseStat.hp = currentHealth;
		}
		else {
			currentHealth = this.gameObject.GetComponent<EnemyStat>().enemy.baseStat.hp;
			currentHealth -= damage;
			this.gameObject.GetComponent<EnemyStat>().enemy.baseStat.hp = currentHealth;
		}

        GameObject canvas = GameObject.Find("Canvas");
        damageTextObject = Instantiate(this.damageText) as GameObject;
		damageTextObject.transform.SetParent(canvas.transform, false);
        damageTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = "" + damage;

		damageTextObject.transform.position = new Vector3(transform.position.x + damageTextOffset,
		                                                  transform.position.y + 1.0f,transform.position.z);
		if (currentHealth <= 0.0f) {
			if (this.gameObject.tag == "Enemy") {
				//BattleManager.instance.enemyList.Remove(unit);
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
