using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // Jangan lupa untuk mengimpor NavMesh

public class Enemy : MonoBehaviour
{
    private Transform player;        // Referensi ke posisi player
    private NavMeshAgent agent;      // NavMeshAgent dari enemy
    public int damage;               // Damage yang diberikan ke player

    // Start is called before the first frame update
    void Start()
    {
        // Mendapatkan komponen NavMeshAgent dari enemy
        agent = GetComponent<NavMeshAgent>();

        // Mencari objek bertag "Player" di scene
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform; // Menyimpan transform player
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Set tujuan (destination) dari agent ke posisi player
        agent.SetDestination(player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jika terkena collider milik player, berikan damage dan hancurkan enemy
        if (other.CompareTag("Player"))
        {
            // Pastikan player memiliki komponen "Player" dengan fungsi TakeDamage
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }

            // Hancurkan enemy setelah menyerang
            Destroy(gameObject);
        }
    }
}
