using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;          // Variasi prefab enemy
    public Transform[] spawnPoints;       // Tiga posisi spawn (posisi berbeda)
    public float spawnDelay = 2f;         // Jeda antar spawn

    private bool isSpawning = false;      // Apakah sedang dalam proses spawning

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Masuk");
            // Mulai spawning saat player masuk ke trigger
            StartSpawning();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Keluar");
            // Hentikan spawning saat player keluar dari trigger
            StopSpawning();
        }
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

            if (enemyToSpawn != null)
            {
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
        foreach (GameObject enemy in enemies)
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
        foreach (GameObject enemy in enemies)
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
}
