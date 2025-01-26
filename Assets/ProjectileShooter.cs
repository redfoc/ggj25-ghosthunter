using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public float shootCooldown = 0.5f;
    public Transform shootPointFront;
    public Transform shootPointBack;

    public float spreadAngle = 15f;

    private float nextShootTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextShootTime)
        {
            ShootByLevel();
            nextShootTime = Time.time + shootCooldown;
        }
    }

    void ShootByLevel()
    {
        switch (GameGeneralLogic.instance.shooterLevel)
        {
            case 1:
                ShootSingle();
                break;
            case 2:
                ShootCone();
                break;
            case 3:
                ShootConeAndBack();
                break;
        }
    }

    void ShootSingle()
    {
        SpawnProjectile(transform.rotation);
    }

    void ShootCone()
    {
        SpawnProjectile(transform.rotation);
        SpawnProjectile(transform.rotation * Quaternion.Euler(0, -spreadAngle, 0));
        SpawnProjectile(transform.rotation * Quaternion.Euler(0, spreadAngle, 0));
    }

    void ShootConeAndBack()
    {
        ShootCone();
        SpawnProjectile(transform.rotation * Quaternion.Euler(0, 180, 0), false);
    }

    void SpawnProjectile(Quaternion rotation, bool isFront = true)
    {
        Vector3 shootPosition = shootPointFront != null ? shootPointFront.position : transform.position;
        if (!isFront)
        {
            shootPosition = shootPointBack != null ? shootPointBack.position : transform.position;
        }

        // Only keep Y rotation
        Quaternion yRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        GameObject projectile = Instantiate(projectilePrefab, shootPosition, yRotation);
        AudioManager.instance.PlaySFX(2);
        // Set initial scale
        projectile.transform.localScale = Vector3.one * 0.3f;

        // Scale up animation
        LeanTween.scale(projectile, Vector3.one * 0.8f, 0.8f).setEase(LeanTweenType.easeOutQuad);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Collider collider = projectile.GetComponent<Collider>();

        if (rb != null && collider != null)
        {
            collider.enabled = false;
            rb.velocity = projectile.transform.forward * projectileSpeed;
            LeanTween.delayedCall(0.2f, () =>
            {
                collider.enabled = true;
            });

            Destroy(projectile, 3f);
        }
    }
}