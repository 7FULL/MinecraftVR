using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBlock : MonoBehaviour
{
    private World world;

    public BlockType blockType;

    public ChunkRenderer chunkRenderer;
    
    private void Awake()
    {
        world = FindObjectOfType<World>();
    }

    private void OnCollisionEnter(Collision other)
    {
        world.SetBlockInt(transform.position,blockType ,chunkRenderer);
        
        Destroy(this.gameObject);
    }
}
