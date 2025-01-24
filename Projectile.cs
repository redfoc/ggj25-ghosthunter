using UnityEngine;
public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    
    void OnCollisionEnter(Collision collision)
    {
        // Handle collision (e.g., damage enemies, explode, etc.)
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
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