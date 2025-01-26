using UnityEngine;

public class OverrideHideSpecificObject : MonoBehaviour
{
   public GameObject hidePrefab;
   public GameObject playerObject;
   private MeshRenderer meshRenderer;

   void Start()
   {
       meshRenderer = hidePrefab.GetComponent<MeshRenderer>();
   }

   void OnTriggerEnter(Collider other)
   {
       if (other.gameObject == playerObject)
       {
           meshRenderer.enabled = false;
       }
   }

   void OnTriggerExit(Collider other) 
   {
       if (other.gameObject == playerObject)
       {
           meshRenderer.enabled = true;
       }
   }
}