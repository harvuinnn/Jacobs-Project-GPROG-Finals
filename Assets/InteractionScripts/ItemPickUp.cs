using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Item picked up!");
    }
}
