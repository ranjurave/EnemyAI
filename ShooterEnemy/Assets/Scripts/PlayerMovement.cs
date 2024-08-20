using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] Transform cam;
    [SerializeField] Transform gunEndPoint;
    [SerializeField] GameObject bullet;
    float speed = 6f;
    float turnSmoothVelocity;

    bool isGrounded;
    float groundCheckDistance = 0.1f ;
    [SerializeField] LayerMask groundLayer;
    float gravity = -9.8f;
    float jumpHeight = 1.0f;
    Vector3 velocity = Vector3.zero;

    void Start() {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude > 0) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            Vector3 moveDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        //Jump Code............
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayer);
        if(isGrounded && velocity.y < 0f) {
            velocity.y = -2.0f;
        }
        if(Input.GetKey(KeyCode.Space) && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //Jump Code End...........

        if (Input.GetMouseButtonDown(0)) {
            GameObject bulletSpawned = Instantiate(bullet, gunEndPoint.position, gunEndPoint.rotation);
            bulletSpawned.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000);
        }
    }
}
