using System.Collections;
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
	[SerializeField] private float frontSideSensorPosition = 0.2f;
	[SerializeField] private float frontSensorAngle = 30f;
	private bool avoiding = false;
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

	// Use this for initialization
	void Start () {
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
		if (enemySeePlayer == true && currentTimeEnemyFollowPlayer >= 0) {
			currentTimeEnemyFollowPlayer -= Time.deltaTime;
			Debug.Log(currentTimeEnemyFollowPlayer);
			currentSpeed = sprintSpeed;
			enemyTarget = player.transform.position;
		}
		else {
			currentTimeEnemyFollowPlayer = timeToFollowPlayer;
			currentSpeed = walkSpeed;
			enemyTarget = checkpoints[checkpoint].transform.position;
			enemySeePlayer = false;
		}
		ObjectAvoidance(enemyTarget);
		//if (avoiding == false) {
		//	Vector3 lookAt = enemyTarget;
		//	lookAt.y = enemyTarget.y;
		//	enemy.LookAt(lookAt);
		//	//navigationTime += Time.deltaTime;
		//	enemy.position = Vector3.MoveTowards(enemy.position,
		//	                                     enemyTarget,
		//	                                     currentSpeed * Time.deltaTime);
		//}
		//var target = player.transform.position;
		//if (enemySeePlayer == true && timeToFollowPlayer >=0) {
		//	//move to player
		//	Vector3 lookAt = player.transform.position;
  //          lookAt.y = transform.position.y;
  //          transform.LookAt(lookAt);
		//	transform.position = Vector3.MoveTowards(transform.position, target, sprintSpeed * Time.deltaTime);
		//}
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
			Destroy(this.gameObject);
			//GameManager.instance.GoToBattle();
			ScreenManager.Instance.TriggerBattleFadeOut();
		}
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
