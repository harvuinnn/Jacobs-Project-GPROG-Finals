using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactRange = 3;
    private Transform highlight;
    private RaycastHit raycastHit;
    private GameObject[] interactablePos;

    void Start()
    {
        interactablePos = GameObject.FindGameObjectsWithTag("Interactable");
    }

    void Update()
    {
        if (IsPlayerNearInteractable())
        {
            PerformRaycast();
        }
        else
        {
            HighlightObject(null);
        }
    }

    private bool IsPlayerNearInteractable()
    {
        foreach (GameObject interactableObject in interactablePos)
        {
            float distanceFromInteractable = Vector3.Distance(transform.position, interactableObject.transform.position);
            if (distanceFromInteractable <= interactRange)
            {
                return true;
            }
        }
        return false;
    }

    private void PerformRaycast()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactRange, Color.red, 1.0f);

        if (Physics.Raycast(ray, out raycastHit, interactRange))
        {
            if (raycastHit.collider.CompareTag("Interactable"))
            {
                Transform hitTransform = raycastHit.transform;

                HighlightObject(hitTransform);

                if (highlight == hitTransform && Input.GetKeyDown(KeyCode.E))
                {
                    if (hitTransform.gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Interact();
                    }
                }
            }
        }
        else
        {
            HighlightObject(null);
        }
    }

    public void HighlightObject(Transform newHighlight)
    {
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
        }

        if (newHighlight != null)
        {
            highlight = newHighlight;

            if (highlight.CompareTag("Interactable"))
            {
                Outline outline = highlight.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
                else
                {
                    outline = highlight.gameObject.AddComponent<Outline>();
                    outline.OutlineColor = Color.yellow;
                    outline.OutlineWidth = 7.0f;
                    outline.enabled = true;
                }
            }
        }
        else
        {
            highlight = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerCamera.transform.position, interactRange);
    }
}
