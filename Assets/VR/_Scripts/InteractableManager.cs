using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private static CraftingManager cf;
    public CraftingManager cfAux;
    
    public static InteractableManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        cf = cfAux;
        
        cf.inventario.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Interact(Item item, Vector3Int posicion, RaycastHit hit)
    {
        PlayerController3D player = GameManager.instance.player.GetComponent<PlayerController3D>();
        switch (item.BlockType)
        {
            case BlockType.MESA_DE_CRAFTEO:
                cf.craftingTable.gameObject.SetActive(true);
                cf.gameObject.SetActive(false);
                cf.inventario.gameObject.SetActive(true);
                
                player.stop();
                break;
            //El filete de momento es un bloque de adoquin por que todavia no lo hemos metido xDD
            case BlockType.BLOQUE_ADOQUIN:
                bool x = player.eat(1);
                if (x)
                {
                    player.usedItem();
                }
                break;
            case BlockType.TNT:
                StartCoroutine(explosion(posicion,hit));
                GameManager.instance.world.SetBlockInt(hit, BlockType.TNT_ACTIVE);
                break;
            default:
                Debug.Log("No coincidio con nada el item: "+ item);
                break;
        }
    }

    IEnumerator explosion(Vector3Int posicion, RaycastHit hit)
    {
        yield return new WaitForSecondsRealtime(4);
        CrearExplosion(posicion, hit);
        GameManager.instance.world.SetBlockInt(hit, BlockType.AIR);
    }
    
    private static void CrearExplosion(Vector3Int posicion, RaycastHit hit2) {
        Debug.Log("Explosion creada");
        
        // Crea una esfera de colisión en la posición dada con el radio especificado
        Collider[] colliders = Physics.OverlapSphere(posicion, 5);
        
        // Recorre todos los colliders y aplica una fuerza explosiva a cada objeto con Rigidbody
        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            Entity entity = hit.GetComponent<Entity>();
            
            if (entity != null)
            {
                entity.takeDamage(8);
                entity.randomDirection();
            }

            if (rb != null) {
                rb.AddExplosionForce(25, posicion, 5, 2f, ForceMode.Impulse);
            }
            else
            {
                Rigidbody rb2 = hit.GetComponentInParent<Rigidbody>();
                
                if (rb2 != null) {
                    rb2.AddExplosionForce(50, posicion, 5, 2f, ForceMode.Impulse);

                    hit.gameObject.GetComponentInParent<PlayerController3D>().receiveExplosion();
                }
            }
        }
    }
}
