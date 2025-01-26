using UnityEngine;

public class CursorMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float fixedHeight = 0f;
    private Camera mainCamera;
    private Plane groundPlane;
    private Animator anim;

    void Start()
    {
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, fixedHeight);
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
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
            anim.SetTrigger("Walking");
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}