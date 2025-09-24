using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public Transform rayOrigin;
    public float distance;
    public LayerMask interactableLayers;

    public RaycastHit hit;

    private void Update()
    {
        Interact();
    }

    private void Interact()
    {
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, distance, interactableLayers) && InputController.Instance.PickUpItem())
        {
            hit.collider.GetComponent<IInteractable>().Interacting();
            Debug.Log(hit.collider.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * distance);
    }
}
