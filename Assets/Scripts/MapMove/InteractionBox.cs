using UnityEngine;
using System.Collections.Generic;

public class InteractionBox : MonoBehaviour
{
    private List<IInteractable> interactablesInRange = new List<IInteractable>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            InteractWithNearest();
    }

    private void InteractWithNearest()
    {
        IInteractable nearest = null;
        float minDist = float.MaxValue;

        foreach (IInteractable interactable in interactablesInRange)
        {
            MonoBehaviour mb = interactable as MonoBehaviour;
            if (mb == null) continue;
            float dist = Vector2.Distance(transform.position, mb.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = interactable;
            }
        }

        nearest?.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && !interactablesInRange.Contains(interactable))
            interactablesInRange.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            interactablesInRange.Remove(interactable);
    }
}
