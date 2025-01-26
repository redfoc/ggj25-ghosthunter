using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public float spawnDelay = 3f;
    public int maxEnemiesInScene = 6;
    public float minDistanceFromPlayer = 10f;

    [Header("Enemy Spawn Limits")]
    public int maxBasicEnemies = 15;
    public int maxBuffEnemies = 5;
    public int maxSpeedyEnemies = 5;

    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    public List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> availableEnemies;
    private List<Transform> spawnPoints = new List<Transform>();
    private Transform playerTransform;
    private bool isSpawning;

    private void Start()
    {
        InitializeSpawner();
        StartSpawning();
    }

    private void InitializeSpawner()
    {
        // Initialize lists
        availableEnemies = new List<GameObject>(enemyPrefabs);
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

        // Find player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private IEnumerator SpawnEnemies()
    {
        while (isSpawning && !IsLevelComplete())
        {
            if (GameManager.instance.enemyInScene < maxEnemiesInScene)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = GetValidSpawnPoint();
        Debug.Log("A");
        if (spawnPoint == null) return;
        Debug.Log("B");
        GameObject enemyToSpawn = GetEnemyToSpawn();
        if (enemyToSpawn == null) return;
        Debug.Log("C");
        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
        enemies.Add(spawnedEnemy);
    }

    private Transform GetValidSpawnPoint()
    {
        var validPoints = spawnPoints.FindAll(point =>
            Vector3.Distance(point.position, playerTransform.position) >= minDistanceFromPlayer);

        return validPoints.Count > 0 ? validPoints[Random.Range(0, validPoints.Count)] : null;
    }

    private GameObject GetEnemyToSpawn()
    {
        List<int> enemyTypes = new List<int>();
        if(GameManager.instance.basicEnemyKillCount < GameManager.instance.basicEnemyMaxSpawnCount){
            enemyTypes.Add(1);
        }
        if(GameManager.instance.buffGhostKillCount < GameManager.instance.buffGhostMaxSpawnCount){
            enemyTypes.Add(2);
        }
        if(GameManager.instance.speedGhostKillCount < GameManager.instance.speedGhostMaxSpawnCount){
            enemyTypes.Add(3);
        }
        if(enemyTypes.Count == 0){
            return null;
        }
        //
        int selectedType = Random.Range(1, enemyTypes.Count+1);
        if(selectedType == 1){
            GameManager.instance.basicEnemySpawnCount++;
            GameManager.instance.enemyInScene++;
            return enemyPrefab1;
        }
        if(selectedType == 2){
            GameManager.instance.buffGhostSpawnCount++;
            GameManager.instance.enemyInScene++;
            return enemyPrefab2;
        }
        if(selectedType == 3){
            GameManager.instance.speedGhostSpawnCount++;
            GameManager.instance.enemyInScene++;
            return enemyPrefab3;
        }
        return null;
        //
        // float totalRate = 0;
        // foreach (GameObject enemy in availableEnemies)
        // {
        //     Enemy script = enemy.GetComponent<Enemy>();
        //     if (script != null)
        //         totalRate += script.spawnRate;
        // }

        // if (totalRate <= 0) return null;

        // float random = Random.Range(0, totalRate);
        // float currentRate = 0;

        // foreach (GameObject enemy in availableEnemies)
        // {
        //     Enemy script = enemy.GetComponent<Enemy>();
        //     if (script != null)
        //     {
        //         currentRate += script.spawnRate;
        //         if (random <= currentRate)
        //             return enemy;
        //     }
        // }
        // return null;
    }

    private bool IsLevelComplete()
    {
        return false;
    }

    private void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }
}