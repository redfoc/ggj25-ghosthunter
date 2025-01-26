using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Basic,
        Buff,
        Speedy
    }
    public GameManager gameManager;  // reference ke gameManager
    public string gameManagerTag;    // Tag si game manager -- biasanya "GameController"
    public EnemyType enemyType;      // Tipe enemy (Basic, Buff, Speedy)
    public int health;               // Jumlah health enemy
    public int damage;               // Damage yang diberikan ke player
    public float speed;              // Kecepatan enemy
    public int dropMoney;            // Uang yang dijatuhkan saat mati
    public float spawnRate;          // Spawn rate untuk tipe enemy

    private Transform player;        // Referensi ke posisi player
    private NavMeshAgent agent;      // NavMeshAgent dari enemy

    void Start()
    {
        if (gameManager == null){
            gameManager = GameObject.FindGameObjectWithTag(gameManagerTag).GetComponent<GameManager>();
        }

        // Mendapatkan komponen NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Mencari referensi player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.Log("No GameObject with tag 'Player' found in the scene!");
        }

        // Set kecepatan agent berdasarkan atribut speed
        agent.speed = speed;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position); // Kejar player
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
            GameManager.instance.EnemyKilled();

            Destroy(gameObject); // Hancurkan enemy setelah menyerang
        } else if (other.CompareTag("Bullet"))
        {
            GameManager.instance.EnemyKilled();
            CountAsKill();

            GameGeneralLogic.instance.AddCoin(10);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
    }

    void CountAsKill(){
        // tambahin kill count ke gamemanager selama masih ada slotnya

        if (enemyType == EnemyType.Basic){
            if (gameManager.basicEnemyKillCount < gameManager.basicEnemyMaxSpawnCount){
                gameManager.basicEnemyKillCount++;
            }
        } else if (enemyType == EnemyType.Buff){
            if (gameManager.buffGhostKillCount < gameManager.buffGhostMaxSpawnCount)
                gameManager.buffGhostKillCount++;
        } else if (enemyType == EnemyType.Speedy){
            if (gameManager.speedGhostKillCount < gameManager.speedGhostMaxSpawnCount)
                gameManager.speedGhostKillCount++;
        }
    }

    public void TakeDamage(int amount)
    {
        AudioManager.instance.PlaySFX(4);
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Tambahkan drop money ke player
        //GameObject.FindObjectOfType<GameManager>().AddMoney(dropMoney);

        
        Destroy(gameObject);
    }
}
