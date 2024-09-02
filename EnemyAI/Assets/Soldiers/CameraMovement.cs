using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    const float Y_ANGLE_MIN = 0.0f;
    const float Y_ANGLE_MAX = 50.0f;
    Transform playerPos;
    Transform camTransform;
    Camera cam;
    float distance = 10;
    float currentX = 0.0f;
    float currentY = 0.0f;
    float sensitivityX = 4.0f;
    float sensitivityY = 5.0f;
    Vector3 lookatPos;

    void Start() {
        camTransform = transform;
        cam = Camera.main;
        playerPos = FindFirstObjectByType<PlayerMovement>().transform;
    }

    void Update() {
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY += -Input.GetAxis("Mouse Y") * sensitivityY;
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    void LateUpdate() {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = playerPos.position + rotation * dir;
        lookatPos = playerPos.position;
        lookatPos.y += 4.0f;
        camTransform.LookAt(lookatPos);
    }
}
