using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float sightRange = 15.0f;
    [SerializeField] float attackRange = 8.0f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;
    NavMeshAgent agent;
    Vector3 walkPoint = Vector3.zero;
    bool walkPointSet = false;
    bool playerInSightRange = false;
    bool playerInAttackRange = false;
    bool isAttacking = false;
    int enemyLife = 5;

    [SerializeField] GameObject Bullet = null;
    [SerializeField] Transform GunEndPoint;
    EnemySpawnerScript enemySpawner;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        enemySpawner = FindFirstObjectByType<EnemySpawnerScript>();
    }

    void Update() { 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position,attackRange, playerLayer);

        if (!playerInAttackRange && !playerInSightRange) {
            Patrolling();
        }
        if (playerInAttackRange && playerInSightRange) {
            AttackPlayer();
        }
        if (playerInAttackRange && !playerInSightRange) {
            ChasePlayer();
        } 
    }

    private void Patrolling() {
        agent.isStopped = false;
        SearchWalkPoint();
        if (walkPointSet) {
            agent.SetDestination(walkPoint);
        }

        Vector3 distaceToWalkPoint = transform.position - walkPoint;
        if (distaceToWalkPoint.magnitude < 0.5f) {
            walkPointSet = false;
        }
    }

    private void ChasePlayer() {
        if (!isAttacking) {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
        }
    }

    private void SearchWalkPoint() {
        float randomX = Random.Range(transform.position.x + 500, transform.position.x - 500);
        float randomZ = Random.Range(transform.position.z + 500, transform.position.z - 500);
        walkPoint = new Vector3(randomX, 0, randomZ);
        walkPointSet = Physics.Raycast(transform.position, Vector3.down, 2.0f, groundLayer);
    }

    private void AttackPlayer() {
        agent.isStopped = true;
        //Debug.Log("Attack Attack Attack....");

        if (!isAttacking) {
            transform.LookAt(playerTransform);
            GameObject SpawnedBullet = Instantiate(Bullet, GunEndPoint.position, GunEndPoint.rotation);
            SpawnedBullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000);
            isAttacking = true;
            Invoke("ResetAttack", 1);
        }
    }

    private void ResetAttack() {
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet") {
            enemyLife -= 1;
            Destroy(collision.gameObject);
            if(enemyLife < 0) {
                enemySpawner.EnemyDestroy(gameObject);
            }
        }
    }

    // Debugging
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
