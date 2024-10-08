
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private CharacterController controller;
    private float speed = 6f;
    private float turnSmoothVelocity;
    Transform thirdPersonCamera;
    Camera playerCamera;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public bool fire;
    //Jump variable
    private bool isGrounded;
    [SerializeField] float groundedCheckDistance = 0.1f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] Transform gunPoint;
    Vector3 velocity;

    void Start() {
        controller = GetComponent<CharacterController>();
        thirdPersonCamera = FindFirstObjectByType<Camera>().transform;
        playerCamera = FindFirstObjectByType<Camera>();
    }
    void Update() {

        fire = Input.GetMouseButton(0);

        if(fire) {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                Debug.Log(hit.transform.name);
            }
            //Ray ray = new Ray(gunPoint.position, transform.forward);
            //if (Physics.Raycast(ray, out hit)) {
            //    Debug.Log(hit.transform.name);
            //}
            //Debug.DrawRay(gunPoint.position, Vector3.forward, Color.red );  
        }

        //Movement Code
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + thirdPersonCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            Vector3 moveDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        //Jump Code
        isGrounded = Physics.CheckSphere(transform.position, groundedCheckDistance, groundMask);
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (isGrounded) {
            if (Input.GetKey(KeyCode.Space)) {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * -gravity);
            }
        }
    }
}
