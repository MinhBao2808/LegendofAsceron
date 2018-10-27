﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MovingObject {
	[SerializeField] private GameObject[] checkpoints;
	[SerializeField] private float rayLength;
	[SerializeField] private float timeToFollowPlayer;
	[SerializeField] private LayerMask layer;
	[SerializeField] private Light spotLight;
	[SerializeField] private float enemyViewDistance;
	[SerializeField] private LayerMask viewMask;
	//private bool isEnemyAttackPlayer = false;
	private bool avoiding = false;
	private Animator animator;
	private float navigationTime = 0;
	float viewAngle;
	private GameObject player;
	private bool enemySeePlayer = false;
	private Color originalSpotLightColor;
	private int checkpoint = 0;
	private Transform enemy;
	private float currentSpeed;
	private float currentTimeEnemyFollowPlayer;
	private Vector3 enemyTarget;

	//private void Awake() {
	//	foreach(int i in GameManager.instance.enemyInMapIndex) {
	//		if (MapManager.instance.enemyInMap[i].name == this.gameObject.name) {
	//			Destroy(gameObject);
	//		}
	//	}
	//}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		currentTimeEnemyFollowPlayer = timeToFollowPlayer;
		enemy = GetComponent<Transform>();
		viewAngle = spotLight.spotAngle;
		originalSpotLightColor = spotLight.color;
		player = GameObject.FindWithTag("Player").gameObject;
	}

	private bool EnemyCanSeePlayer() {
		if (Vector3.Distance(transform.position, player.transform.position) < enemyViewDistance) {
			Vector3 distanceToPlayer = (player.transform.position - transform.position).normalized;
			float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, distanceToPlayer);
			if (angleBetweenEnemyAndPlayer < viewAngle / 2f) {
				if (!Physics.Linecast(transform.position,player.transform.position,viewMask)) {
					enemySeePlayer = true;
					return true;
				}
			}
		}
		return false;
	}

	private void ObjectAvoidance(Vector3 target) {
		var dir = (target - enemy.position).normalized;
		RaycastHit hit;
		avoiding = false;
		if (Physics.Raycast(transform.position,transform.forward,out hit,rayLength)) {
			if (hit.transform != transform) {
				if (hit.collider.CompareTag("Object")) {
					Debug.DrawLine(transform.position, hit.point, Color.red);
                    dir += hit.normal * 20;
                    avoiding = true;
				}
			}
		}
		var leftRay = transform.position;
		var rightRay = transform.position;
		leftRay.x -= 2;
		rightRay.x += 2;
		if (Physics.Raycast(leftRay,transform.forward,out hit,rayLength)) {
			if (hit.transform != transform) {
				if (hit.collider.CompareTag("Object")) {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    dir += hit.normal * 20;
                    avoiding = true;
                }
			}
		}
		if (Physics.Raycast(rightRay, transform.forward, out hit, rayLength)) {
			if (hit.transform != transform) {
				if (hit.collider.CompareTag("Object")) {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    dir -= hit.normal * 20;
                    avoiding = true;
                }
			}
		}
		var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
		transform.position += transform.forward * walkSpeed * Time.deltaTime;
	}

	// Update is called once per frame
	void Update () {
		if (EnemyCanSeePlayer()) {
			spotLight.color = Color.red;
		}
		else {
			spotLight.color = originalSpotLightColor;
		}
		if(GameManager.instance.isEnemyAttackPlayer == true && GameManager.instance.isPlayerAttackEnemy == false) {
			animator.Play("Attack");
		}
		else {
			if (GameManager.instance.isPlayerAttackEnemy == true) {
				Destroy(this.gameObject);
				//GameManager.instance.GoToBattle();
				ScreenManager.Instance.TriggerBattleFadeOut();
			}
			if (enemySeePlayer == true && currentTimeEnemyFollowPlayer >= 0) {
                currentTimeEnemyFollowPlayer -= Time.deltaTime;
                currentSpeed = sprintSpeed;
                animator.SetBool("isWalk", false);
                enemyTarget = player.transform.position;
            }
            else {
                currentTimeEnemyFollowPlayer = timeToFollowPlayer;
                currentSpeed = walkSpeed;
                enemyTarget = checkpoints[checkpoint].transform.position;
                animator.SetBool("isWalk", true);
                enemySeePlayer = false;
            }
			ObjectAvoidance(enemyTarget);
        }
	}

	public void attackEvent() {
		
		GameManager.instance.isEnemyAttackPlayer = true;
		GameManager.instance.isPlayerAttackEnemy = false;
		//for (int i = 0; i < MapManager.instance.enemyInMap.Length; i++) {
		//	if (MapManager.instance.enemyInMap[i].name == this.gameObject.name) {
		//		Debug.Log(i);
		//		GameManager.instance.enemyInMapIndex.Add(i);
		//		break;
		//	}
		//}

		Destroy(this.gameObject);
		//GameManager.instance.GoToBattle();
		//ScreenManager.Instance.TriggerBattleFadeOut();
	}

	//private void OnTriggerEnter(Collider collision) {
	//	if (collision.gameObject.tag == "Player") {
	//		//enemySeePlayer = true;
	//		DataManager.instance.listEnemyDefeatedPosition.Enqueue(enemyStartPosition);
	//           Destroy(this.gameObject);
	//           GameManager.instance.GoToBattle();
	//	}
	//}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			if (GameManager.instance.isPlayerAttackEnemy == false && enemySeePlayer == true) {
				GameManager.instance.isEnemyAttackPlayer = true;
			}
			//else {
			//	GameManager.instance.isPlayerAttackEnemy = true;
			//}
		}
	}

	private void OnDestroy() {
		ScreenManager.Instance.TriggerBattleFadeOut();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Checkpoint") {
			if (checkpoint == 1) {
				checkpoint = 0;
			}
			else {
				checkpoint++;
			}
		}
	}
}
