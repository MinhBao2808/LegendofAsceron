using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovingObject {
    public static PlayerMovement instance = null;
	[SerializeField] private GameObject StatPanel;
	private Rigidbody rigid;
	private Animation animation;
	private CharacterController characterController;
	[SerializeField] private Transform cameraT;
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
    private GameManager gameManager = new GameManager();
    
    
	private void Awake() {
        if (instance == null) {
            instance = this;
        }
        //gameObject.transform.position = DataManager.instance.playerPosition;
	}

	private void Start() {
		targetRotation = transform.rotation;
		rigid = GetComponent<Rigidbody>();
		animation = GetComponent<Animation>();
		//animation.Play("idle");
		characterController = GetComponent<CharacterController>();
		forwardInput = turnInput = 0;
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
		if (Input.GetKey("a")) {
			animation.Play("Attack");
		}
		else {
			if (Input.GetKey(KeyCode.LeftShift)) {
                currentSpeed = sprintSpeed;
            }
            else {
                currentSpeed = walkSpeed;
            }
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 inputDir = input.normalized;
            if (inputDir != Vector2.zero) {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
                                                                           targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                Vector3 velocity = transform.forward * currentSpeed;
                if (currentSpeed == sprintSpeed) {
                    animation.Play("Run");
                }
                else {
                    animation.Play("Walk");
                }
                characterController.Move(velocity * Time.deltaTime);
            }
            else {
				animation.PlayQueued("idle",QueueMode.PlayNow);
            }
		}


		//currentSpeed = Mathf.SmoothDamp(currentSpeed, speed * inputDir.magnitude, ref speedSmoothVelocity, speedSmoothTime);

        
		//currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
		if (Input.GetKeyDown("e")) {
			StatPanel.SetActive(true);
        }

		if (Input.GetKeyDown("escape")) {
			StatPanel.SetActive(false);
        }
	}

	//private void FixedUpdate() {
	//	Run();
	//}

	//void Run() {
	//	if (Mathf.Abs(forwardInput) > inputDelay) {
	//		rigid.velocity = transform.forward * forwardInput * forwardVel;
	//		animator.SetBool("Moving", true);
	//	}
	//	else {
	//		rigid.velocity = Vector3.zero;
	//		animator.SetBool("Moving", false);
	//	}
	//}

	//void Turn() {
	//	if (Mathf.Abs(turnInput) >inputDelay) {
	//		targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
	//	}
	//	transform.rotation = targetRotation;
	//}

	public Vector2 ReturnPlayerPosition() {
        Vector2 playerPosition = transform.position;
        return playerPosition;
    }
}
