using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    public bool isGrounded = false;

    private PlayerController3D player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController3D>();
        isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
        if (player.explosionCurrent)
        {
            player.ableToMove = true;
            player.explosionCurrent = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }

    private void OnTriggerStay(Collider other)
    {
        isGrounded = true;
        
        if (player.explosionCurrent)
        {
            player.ableToMove = true;
            player.explosionCurrent = false;
        }
    }
}
