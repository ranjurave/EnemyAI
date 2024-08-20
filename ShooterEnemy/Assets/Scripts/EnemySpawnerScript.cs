using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour {

    [SerializeField] EnemyAIScript enemySpawned;
    EnemyAIScript[] enemyList = new EnemyAIScript[2];

    void Start()
    {
        for (int i = 0; i < 2; i++) {
            enemyList[i] = enemySpawned;
        }

        foreach(EnemyAIScript enemy in enemyList) {
            Debug.Log(enemy);
            float posX = Random.Range(-20, 20);
            float posY = Random.Range(-20, 20);
            Vector3 spawnPoint = new Vector3(posX, 0, posY);
            Vector3 spawnRotation = new Vector3(0, Random.Range(0,360), 0);
            Instantiate(enemy, spawnPoint, Quaternion.Euler(spawnRotation)); 
        }
    }

    public void EnemyDestroy(GameObject enemyToDestroy) {
        enemyToDestroy.gameObject.SetActive(false);
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy() {
        yield return new WaitForSeconds(5);
        Instantiate(enemySpawned, Vector3.zero, Quaternion.Euler(Vector3.zero));
    }
}
