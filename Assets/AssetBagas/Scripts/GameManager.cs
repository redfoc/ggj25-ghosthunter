using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] enemies;          // Variasi prefab enemy
    public List<GameObject> enemiesList; 
    public Transform[] spawnPoints;       // Tiga posisi spawn (posisi berbeda)
    public float spawnDelay = 2f;         // Jeda antar spawn

    private bool isSpawning = false;      // Apakah sedang dalam proses spawning

    [Header("Max Spawn Counter")]
    public int basicEnemyMaxSpawnCount = 5;
    public int buffGhostMaxSpawnCount = 3;
    public int speedGhostMaxSpawnCount = 2;

    [Header("Counter")]
    public int basicEnemySpawnCount = 0;
    public int buffGhostSpawnCount = 0;
    public int speedGhostSpawnCount = 0;

    [Header("Enemy Kill Count")]
    public int basicEnemyKillCount = 0;
    public int buffGhostKillCount = 0;
    public int speedGhostKillCount = 0;

    public int totalEnemyKill = 0;
    public int remainingEnemyToKill = 0;

    public bool isGameComplete = false;
    public bool isGameover = false;

    public int enemyInScene = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (enemiesList == null)
            enemiesList = new List<GameObject>(enemies);
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         print("Masuk");
    //         // Mulai spawning saat player masuk ke trigger
    //         StartSpawning();
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         print("Keluar");
    //         // Hentikan spawning saat player keluar dari trigger
    //         StopSpawning();
    //     }
    // }
    private void Update() {
        // debug f12 to print 
        if (Input.GetKeyDown(KeyCode.F12)){
            foreach(GameObject a in enemiesList){
                Debug.Log(a.name);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
            enemiesList.Remove(enemies[0]);
        if (Input.GetKeyDown(KeyCode.O))
            enemiesList.Remove(enemies[1]);
        if (Input.GetKeyDown(KeyCode.P))
            enemiesList.Remove(enemies[2]);
    }
    private void FixedUpdate() {
        // count remaining enemy to kill and total current total kills
        totalEnemyKill = basicEnemyKillCount + buffGhostKillCount + speedGhostKillCount;
        remainingEnemyToKill = (basicEnemyMaxSpawnCount + buffGhostMaxSpawnCount + speedGhostMaxSpawnCount) - totalEnemyKill;


        // LEVEL CLEARED if remaningEnemyToKill is 0
        if(!isGameComplete){
            if (enemyInScene <= 0){
                if(basicEnemyKillCount >= basicEnemyMaxSpawnCount && buffGhostKillCount >= buffGhostMaxSpawnCount && speedGhostKillCount >= speedGhostMaxSpawnCount){
                    isGameComplete = true;
                    Debug.Log("Game Completed");
                    UIScript.instance.ShowPopupGameCompleted();
                }
            }
        }

        UIScript.instance.UpdateTextCounterGhost(basicEnemyKillCount+"/"+basicEnemyMaxSpawnCount, buffGhostKillCount+"/"+buffGhostMaxSpawnCount, speedGhostKillCount+"/"+speedGhostMaxSpawnCount);
    }
    // Fungsi untuk memulai spawning enemy
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }

    // Coroutine untuk spawning enemy dengan jeda
    private IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            // Pilih spawn point secara acak
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            // Pilih enemy berdasarkan spawn rate
            GameObject enemyToSpawn = GetEnemyBySpawnRate();

            // 
            if (basicEnemySpawnCount >= basicEnemyMaxSpawnCount){
                enemiesList.Remove(enemies[0]);
            } else if (buffGhostSpawnCount >= buffGhostMaxSpawnCount) {
                // if (enemyToSpawn.GetComponent<Enemy>().enemyType == Enemy.EnemyType.Buff)
                //     enemyToSpawn = enemies[2];
                enemiesList.Remove(enemies[1]);
            } else if (speedGhostSpawnCount >= speedGhostMaxSpawnCount) {
                // if (enemyToSpawn.GetComponent<Enemy>().enemyType == Enemy.EnemyType.Speedy)
                //     enemyToSpawn = enemies[0];
                enemiesList.Remove(enemies[2]);
            }
            if (enemyToSpawn != null)
            {   
                // check tipe enemy yg mo dispawn, habis itu tambahin ke counter
                switch (enemyToSpawn.GetComponent<Enemy>().enemyType)
                {
                    case Enemy.EnemyType.Basic:
                    basicEnemySpawnCount++;
                    break;
                    case Enemy.EnemyType.Buff:
                    buffGhostSpawnCount++;
                    break;
                    case Enemy.EnemyType.Speedy:
                    speedGhostSpawnCount++;
                    break;
                    default:
                    break;
                }
                // Spawn enemy di posisi yang dipilih
                Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
            }

            // Tunggu sebelum spawn berikutnya
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Fungsi untuk menghentikan spawning
    public void StopSpawning()
    {
        isSpawning = false;
    }

    // Pilih enemy berdasarkan spawn rate
    private GameObject GetEnemyBySpawnRate()
    {
        float totalRate = 0f;

        // Hitung total spawn rate
        foreach (GameObject enemy in enemiesList)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                totalRate += enemyScript.spawnRate;
            }
        }

        float randomValue = Random.Range(0, totalRate); // Nilai acak dari 0 ke total spawn rate
        float currentRate = 0f;

        // Pilih enemy berdasarkan spawn rate
        foreach (GameObject enemy in enemiesList)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                currentRate += enemyScript.spawnRate;

                if (randomValue <= currentRate)
                {
                    return enemy;
                }
            }
        }

        return null;
    }

    public void EnemyKilled(string type = ""){
        enemyInScene--;
    }
}
