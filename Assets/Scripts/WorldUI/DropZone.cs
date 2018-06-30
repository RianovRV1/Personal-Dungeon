using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Draggable.Slot SlotType;
    public void OnPointerEnter(PointerEventData eventdata)
    {
        //Debug.Log(this.gameObject.name + "Entered");
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        //Debug.Log(this.gameObject.name + "Exited");
    }

    public void OnDrop(PointerEventData eventdata)
    {
        Debug.Log(this.gameObject.name + " joined: " + eventdata.pointerDrag.name);

        //Make sure something is dragging then set new parent to drop onto
        Draggable currentObject = eventdata.pointerDrag.GetComponent<Draggable>();
        if (currentObject != null)
        {
            if (SlotType == currentObject.slotType)
            {
                currentObject.newParent = this.transform;
            }
        }

    }
}