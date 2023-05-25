using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropableItem : MonoBehaviour
{
    public Item Item;
    public int itemMuch;

    private Rigidbody rb;

    public ChunkRenderer chunkRenderer;

    private Vector3 stoppedPosition = new Vector3(0,-11111111111111,0);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject,10000);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Picked");
            other.gameObject.GetComponentInParent<PlayerController3D>().inventory.añadirItem(Item,itemMuch,chunkRenderer);
            other.gameObject.GetComponentInParent<PlayerController3D>().updateItems();
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (rb.velocity.magnitude == 0)
        {
            stoppedPosition = transform.position;
        }

        //Debug.Log(GameManager.instance.world.GetBlock(transform.position,chunkRenderer));
        
        if (stoppedPosition.y- 0.3f > transform.position.y && GameManager.instance.world.GetBlock(transform.position,chunkRenderer) != BlockType.AIR)
        {
            transform.position = new Vector3(transform.position.x,stoppedPosition.y + 0.3f,transform.position.z);
            stoppedPosition = transform.position;
        }
    }
}
