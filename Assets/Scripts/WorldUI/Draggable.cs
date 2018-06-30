using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform reParent;
    public Transform newParent = null;
    public enum Slot {Character, RHand, LHand, Armor, Trinket, Item};
    public Slot slotType;

    public void Awake()
    {
        reParent = this.transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        
        //Unparent when dragging object
        Debug.Log("Current Parent: " + reParent);
        this.transform.SetParent(this.transform.root);
        newParent = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Turn off raycast
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        //Move object
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");

        //Reparent after dropping
        if (newParent != null)
        {
            this.transform.SetParent(newParent);
        }
        else
        {
            this.transform.SetParent(reParent);
        }

        //Turn on raycast
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}