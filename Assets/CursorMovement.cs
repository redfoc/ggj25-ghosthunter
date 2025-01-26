using UnityEngine;

public class CursorMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float fixedHeight = 0f;
    public float minDistance = 1f;
    private Camera mainCamera;
    private Plane groundPlane;
    private bool isRotating = false;
    private ProjectileShooter shooter;
    private Animator anim;

    void Start()
    {
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, fixedHeight);
        shooter = GetComponent<ProjectileShooter>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(GameManager.instance.isGameComplete) return;
        
        if (Input.GetMouseButtonDown(1) && !isRotating)
        {
            isRotating = true;
            LeanTween.rotateY(gameObject, transform.eulerAngles.y + 180f, 0.2f)
            .setOnComplete(() =>
            {
                shooter.ShootByLevel();
                LeanTween.delayedCall(.5f, () =>
                {
                    LeanTween.rotateY(gameObject, transform.eulerAngles.y + 180f, 0.2f)
                        .setOnComplete(() =>
                        {
                            isRotating = false;
                        });
                });
            });
            return;
        }

        if (!isRotating)
        {
            HandleNormalMovement();
        }
    }

    void HandleNormalMovement()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        if (groundPlane.Raycast(ray, out enter))
        {
            Vector3 targetPosition = ray.GetPoint(enter);
            
            // Rotate towards target
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Move towards target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}