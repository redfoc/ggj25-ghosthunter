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

            // Pilih enemy secara acak
            int enemyIndex = Random.Range(0, enemies.Length);
            GameObject enemyToSpawn = enemies[enemyIndex];

            // Spawn enemy di posisi yang dipilih
            Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);

            // Tunggu sebelum spawn berikutnya
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Fungsi untuk menghentikan spawning
    public void StopSpawning()
    {
        isSpawning = false;
        StopCoroutine(SpawnEnemies());
    }
}
