using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public void interact(Vector3Int posicion = new Vector3Int(), RaycastHit hit = default);
}
