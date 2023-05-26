using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface Slot: IDropHandler
{
    public void OnDrop(PointerEventData eventData);

    public void clear();

    public void implementDrag(DragDropItem dragItem);
}
