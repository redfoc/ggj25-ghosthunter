using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 5f;
    public Material vignetteMaterial;
    
    private Vector3 originalPosition;
    private float shakeIntensity = 0.2f;
    private float shakeDuration = 0.5f;
    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (vignetteMaterial != null)
        {
            Graphics.Blit(source, destination, vignetteMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        
        transform.LookAt(target);
    }

    public void StartShake()
    {
        StartCoroutine(ShakeCamera());
        StartCoroutine(VignetteEffect());
    }

    private IEnumerator ShakeCamera()
    {
        originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-2f, 2f) * shakeIntensity;
            float y = Random.Range(-2f, 2f) * shakeIntensity;
            
            transform.localPosition = new Vector3(
                originalPosition.x + x,
                originalPosition.y + y,
                originalPosition.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    private IEnumerator VignetteEffect()
    {
        if (vignetteMaterial == null) yield break;
        
        float elapsed = 0f;
        float maxIntensity = 0.5f;

        while (elapsed < shakeDuration)
        {
            vignetteMaterial.SetFloat("_VignetteIntensity", Mathf.Lerp(0f, maxIntensity, elapsed / shakeDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        while (vignetteMaterial.GetFloat("_VignetteIntensity") > 0)
        {
            float currentIntensity = vignetteMaterial.GetFloat("_VignetteIntensity");
            vignetteMaterial.SetFloat("_VignetteIntensity", Mathf.Lerp(currentIntensity, 0f, Time.deltaTime * 2f));
            yield return null;
        }
    }
}