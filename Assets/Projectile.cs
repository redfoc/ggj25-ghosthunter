using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
  public float damage = 10f;
  
  void OnTriggerEnter(Collider other)
  {
      // Check if hit NavMesh
      NavMeshHit hit;
      if (NavMesh.SamplePosition(other.transform.position, out hit, 0.1f, NavMesh.AllAreas))
      {
          Destroy(gameObject);
          return;
      }

      // Handle other collisions
      IDamageable target = other.GetComponent<IDamageable>();
      if (target != null)
      {
          target.TakeDamage(damage);
      }
      
      Destroy(gameObject);
  }
}

// Optional: Interface for damageable objects
public interface IDamageable
{
    void TakeDamage(float damage);
}