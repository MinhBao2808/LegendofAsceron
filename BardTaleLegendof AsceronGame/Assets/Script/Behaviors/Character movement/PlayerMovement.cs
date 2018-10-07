using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovingObject {
    public static PlayerMovement instance = null;
	private Rigidbody rigid;
	private Animation ani;
	private CharacterController characterController;
	//[SerializeField] private Transform cameraT;
	[SerializeField] private float inputDelay = 0.1f;
	[SerializeField] private float forwardVel = 12;
	[SerializeField] private float rotateVel = 100;
	private Quaternion targetRotation;
	private float forwardInput, turnInput; 
	[SerializeField] private float speed;
	[SerializeField] private float rotateSpeed = 3.0f;
	[SerializeField] private float turnSmoothTime;
	private float turnSmoothVelocity;
	public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
	private bool isPlayerPressAttack;
    
	private void Awake() {
        if (instance == null) {
            instance = this;
        }
        //gameObject.transform.position = DataManager.instance.playerPosition;
	}

	private void Start() {
		targetRotation = transform.rotation;
		rigid = GetComponent<Rigidbody>();
		ani = GetComponent<Animation>();
		//animation.Play("idle");
		characterController = GetComponent<CharacterController>();
		forwardInput = turnInput = 0;
		isPlayerPressAttack = false;
	}

	public Quaternion TargetRotation {
		get {
			return targetRotation;
		}
	}

	void GetInput () {
		forwardInput = Input.GetAxis("Vertical");
		turnInput = Input.GetAxis("Horizontal");
	}

	void Update() {
		//GetInput();
		//Turn();
		if (GameManager.instance.isEnemyAttackPlayer == false) {
			if (Input.GetKey("a")) {
				isPlayerPressAttack = true;
				ani.Play("Attack");
			}
			else {
				if (Input.GetKey(KeyCode.LeftShift)) {
					currentSpeed = sprintSpeed;
				}
				else {
					currentSpeed = walkSpeed;
				}
				isPlayerPressAttack = false;
				Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
				Vector2 inputDir = input.normalized;
				if (inputDir != Vector2.zero) {
					float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
					transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
																			   targetRotation, ref turnSmoothVelocity, turnSmoothTime);
					Vector3 velocity = transform.forward * currentSpeed;
					if (currentSpeed == sprintSpeed) {
						ani.Play("Run");
					}
					else {
						ani.Play("Walk");
					}
					characterController.Move(velocity * Time.deltaTime);
				}
				else {
					ani.PlayQueued("idle", QueueMode.PlayNow);
				}
			}
		}
		else {
			ani.PlayQueued("idle", QueueMode.PlayNow);
		}
	}

	public Vector2 ReturnPlayerPosition() {
        Vector2 playerPosition = transform.position;
        return playerPosition;
    }

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Enemy" && isPlayerPressAttack == true) {
			GameManager.instance.isPlayerAttackEnemy = true;
		}
	}
}
