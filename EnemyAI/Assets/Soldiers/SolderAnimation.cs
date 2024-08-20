using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolderAnimation : MonoBehaviour
{
    Animator animator;
    PlayerMovement player;
    int isRunningHash;


    void Start() {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("IsRunning"); // generates parameter id for string
        player = GetComponentInParent<PlayerMovement>();
    }

    void Update() {
        bool isRunning = animator.GetBool(isRunningHash);

        if (player.direction.magnitude > 0.1f) {
            animator.SetBool("IsRunning", true);
        }
        if (player.direction.magnitude < 0.1f) {
            animator.SetBool("IsRunning", false);
        }
        if (player.fire) {
            animator.SetTrigger("Shoot");
        }
    }

}
