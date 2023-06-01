using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Data/Item")]
public class Item : ScriptableObject,Interactable
{
    public string name;

    public Sprite sprite;

    public int maxStack;

    public BlockType BlockType;

    public bool isPlacable = true;

    public bool interactable = false;

    public bool tool = false;

    public int durability = 10;

    public Tool toolType;
    public void interact(Vector3Int posicion = new Vector3Int(), RaycastHit hit = default)
    {
        InteractableManager.instance.Interact(this,posicion, hit);
        //Debug.Log(this.name);
    }

    public override string ToString()
    {
        return "Nombre: "+name+"/n"+
               "Sprite: "+sprite.name+"/n"+
               "MaxStack: "+maxStack+"/n"+
               "BlockType: "+BlockType+"/n"+
               "IsPlacable: "+isPlacable+"/n"+
               "Interactable: "+interactable+"/n";
    }
}
