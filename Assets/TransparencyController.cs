using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player;
    public float rayOffset = 1f;
    private Material material;
    private float normalOpacity = 1f;
    private float transparentOpacity = 0.5f;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = (player.position - rayStart).normalized;
        Ray ray = new Ray(rayStart, rayDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == player)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, transparentOpacity);
            }
            else
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, normalOpacity);
            }
        }
    }
}