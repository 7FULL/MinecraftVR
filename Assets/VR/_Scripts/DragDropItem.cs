using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 *
 *
 *El objeto tiene que ser hijo dle canvas si no las escalas no funcionan
 * 
 */
public class DragDropItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Canvas canvas;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 startedPosition;

    public Item item;
    public int itemMuch;

    public bool isOutput = false;

    public CraftingManager cf;

    public TMP_Text texto;

    public Image imagen;

    public bool lastWasCraftingSlot = false;
    
    public bool lastWasInventorySlot = false;

    public CraftingSlot lastCraftingSlot;
    
    public InventorySlots lastInventorySlot;
    
    public GameObject dropableItem;
    
    public GameObject dragDropItem;

    private PointerEventData PointerEventData = null;

    public ChunkRenderer chunkRenderer;

    private PlayerController3D player;
    
    private ControlesVR _controlesVR;
    
    private bool usarVR = false;
    private bool cooldownUsarVR = false;

    private void Awake()
    {
        _controlesVR = new ControlesVR();

        _controlesVR.VR.Usar.started += u => usarVR = true;
        _controlesVR.VR.Usar.canceled += u => usarVR = false;

        _controlesVR.Enable();
    }
    
    IEnumerator cooldownVRUsar()
    {
        yield return new WaitForSecondsRealtime(.1f);
        cooldownUsarVR = false;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        imagen.sprite = item.sprite;

        cf = GameManager.instance.player.GetComponent<PlayerController3D>().craftingManager;
        
        //Utilizar esto en escena de pruebas
        //cf = FindObjectOfType<CraftingManager>();
        
        canvas = cf.getCanvas();

        player = GameManager.instance.player.GetComponent<PlayerController3D>();
    }

    public void inicializarFoto()
    {
        imagen.sprite = item.sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PointerEventData = eventData;
        
        transform.SetAsLastSibling();
        
        //canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        resetStartPosition();
        
        if (isOutput)
        {
            cf.outputDone();
            isOutput = false;
            cf.update();
        }

        if (lastWasCraftingSlot)
        {
            lastCraftingSlot.clear();

            cf.update();
        }
        
        if (lastWasInventorySlot)
        {
            lastInventorySlot.clear();
        }
        
        player.updateItems();
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        PointerEventData = null;
        //canvasGroup.alpha = 1;

        hacerCosasDeObjeto(eventData,this);
        
        canvasGroup.blocksRaycasts = true;
        
        player.updateItems();
        
        cf.update();
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(1) || usarVR )&& PointerEventData != null && !cooldownUsarVR)
        {
            cooldownUsarVR = true;
            
            StartCoroutine(cooldownVRUsar());
            
            if (PointerEventData.pointerEnter.name == "Panel")
            {
                //Debug.Log("0");

                dropearSoloUno();
                
                GameManager.instance.player.GetComponent<PlayerController3D>().updateItems();
            }
            else if (PointerEventData.pointerEnter.transform.parent.GetComponent<Slot>() != null)
            {
                //Debug.Log("1");
                
                GameObject aux = Instantiate(dragDropItem, this.transform.parent.transform);

                DragDropItem dragDrop= aux.GetComponent<DragDropItem>();

                dragDrop.rectTransform.anchoredPosition = PointerEventData.pointerEnter.transform.parent.GetComponent<RectTransform>().anchoredPosition;

                dragDrop.item = item;
                dragDrop.actualizarCantidad(1);
                dragDrop.inicializarFoto();
                
                PointerEventData.pointerEnter.transform.parent.GetComponent<Slot>().implementDrag(dragDrop);
                
                restarCantidad(1);
                
                //bool x = hacerCosasDeObjeto(PointerEventData,dragDrop);

                transform.SetAsLastSibling();

                /*if (x)
                {
                    Debug.Log("Destruido");
                    Destroy(aux);
                }*/
                
                aux.GetComponent<CanvasGroup>().blocksRaycasts = true;
                
                Type t = PointerEventData.pointerEnter.transform.parent.GetComponent<Slot>().GetType();
                
                if (t.Equals(typeof(CraftingSlot)))
                {
                    if (!PointerEventData.pointerEnter.transform.parent.GetComponent<CraftingSlot>().isOutput)
                    {
                        //Debug.Log("a");
                        
                        dragDrop.lastWasCraftingSlot = true;
                    
                        dragDrop.lastCraftingSlot = PointerEventData.pointerEnter.transform.parent.GetComponent<CraftingSlot>();
                    }
                    else
                    {
                        rectTransform.anchoredPosition = startedPosition;

                        if (dragDrop.lastWasCraftingSlot)
                        {
                            dragDrop.lastCraftingSlot.asign(this);
                        }
                    }
                    
                    cf.update();
                }

                if (t.Equals(typeof(InventorySlots)))
                {
                    dragDrop.lastWasInventorySlot = true;
                    
                    dragDrop.lastInventorySlot = PointerEventData.pointerEnter.transform.parent.GetComponent<InventorySlots>();

                    PointerEventData.pointerEnter.transform.parent.GetComponent<InventorySlots>().item = dragDrop;
                }
                
                cf.update();
                
                GameManager.instance.player.GetComponent<PlayerController3D>().updateItems();

                //GameManager.instance.player.GetComponent<PlayerController3D>().updateHandSlots();
            }
            else if(PointerEventData.pointerEnter.transform.parent.GetComponent<DragDropItem>() != null)
            {
                //Debug.Log(PointerEventData.pointerEnter.transform.parent.name);
                
                GameObject aux = Instantiate(dragDropItem, this.transform.parent.transform);

                DragDropItem dragDrop= aux.GetComponent<DragDropItem>();

                dragDrop.item = item;
                dragDrop.actualizarCantidad(1);
                dragDrop.inicializarFoto();
                
                restarCantidad(1);
                
                hacerCosasDeObjeto(PointerEventData,dragDrop);

                aux.GetComponent<CanvasGroup>().blocksRaycasts = true;
                
                transform.SetAsLastSibling();

                Destroy(aux);
                
                cf.update();
                
                GameManager.instance.player.GetComponent<PlayerController3D>().updateItems();
            }
        }
    }

    public bool hacerCosasDeObjeto(PointerEventData eventData,DragDropItem dropItem)
    {
        bool aux = false;
        
        Slot slot = eventData.pointerEnter.transform.parent.GetComponent<Slot>();
            if (slot == null)
            {
                DragDropItem otherItem = eventData.pointerEnter.transform.parent.GetComponent<DragDropItem>();

                if ((otherItem == null || otherItem.isOutput || isOutput) && eventData.pointerEnter.name != "Panel")
                {
                    //Debug.Log("0");
                    aux = true;

                    if (lastWasInventorySlot && lastInventorySlot.item == null)
                    {
                        dropItem.rectTransform.anchoredPosition = dropItem.startedPosition;
                    }
                    else
                    {
                        player.inventory.añadirItem(dropItem);
                        Destroy(dropItem.gameObject);
                        return false;
                    }

                    if (lastWasCraftingSlot)
                    {
                        lastCraftingSlot.item = dropItem;
                    }

                    if (lastWasInventorySlot)
                    {
                        lastInventorySlot.item = dropItem;
                    }
                }else{
                    if (eventData.pointerEnter.name != "Panel")
                    {
                        if (otherItem.item == dropItem.item)
                        {
                        //Debug.Log("1");
                        if (otherItem.itemMuch+dropItem.itemMuch <= dropItem.item.maxStack)
                        {
                            //Debug.Log("2");
                            otherItem.sumarCantidad(dropItem.itemMuch);
                            dropItem.borrarItem();
                        }
                        else
                        {
                            //Debug.Log("3");
                        
                            int x = dropItem.item.maxStack - (otherItem.itemMuch+dropItem.itemMuch);

                            if (otherItem.itemMuch < dropItem.itemMuch)
                            {
                                otherItem.itemMuch = otherItem.item.maxStack;
                        
                                dropItem.actualizarCantidad(-x);
                                otherItem.actualizarCantidad(dropItem.item.maxStack);
                            }
                            else
                            {
                                dropItem.itemMuch = dropItem.item.maxStack;
                        
                                otherItem.actualizarCantidad(-x);
                                dropItem.actualizarCantidad(dropItem.item.maxStack);
                            }

                            aux = true;
                            dropItem.rectTransform.anchoredPosition = dropItem.startedPosition;
                        }
                        
                        //Updateamos aqui los slots tambien porque si juntamos 2 bloques dentro del craft no los detecta
                        cf.update();
                        }
                    else
                    {
                        //Debug.Log("0");

                        otherItem.resetStartPosition();
                        
                        RectTransform otherItemTransform = otherItem.gameObject.GetComponent<RectTransform>();
                        otherItemTransform.anchoredPosition = dropItem.startedPosition;
                        
                        CraftingSlot xCS = otherItem.lastCraftingSlot;
                        InventorySlots yIS = otherItem.lastInventorySlot;
                        
                        bool x = otherItem.lastWasCraftingSlot;
                        bool y = otherItem.lastWasInventorySlot;

                        otherItem.lastCraftingSlot = dropItem.lastCraftingSlot;
                        otherItem.lastInventorySlot = dropItem.lastInventorySlot;
                        
                        otherItem.lastWasCraftingSlot = dropItem.lastWasCraftingSlot;
                        otherItem.lastWasInventorySlot = dropItem.lastWasInventorySlot;
                        
                        dropItem.lastCraftingSlot = xCS;
                        dropItem.lastInventorySlot = yIS;
                        
                        dropItem.lastWasCraftingSlot = x;
                        dropItem.lastWasInventorySlot = y;

                        dropItem.rectTransform.anchoredPosition = otherItem.startedPosition;

                        dropItem.resetStartPosition();
                        otherItem.resetStartPosition();
                        
                        if (dropItem.lastWasCraftingSlot)
                        {
                            dropItem.lastCraftingSlot.asign(this);
                        }
                        
                        if (otherItem.lastWasInventorySlot)
                        {
                            otherItem.lastInventorySlot.asign(otherItem);
                        }
                        
                        if (dropItem.lastWasInventorySlot)
                        {
                            dropItem.lastInventorySlot.asign(this);
                        }
                        
                        if (otherItem.lastWasCraftingSlot)
                        {
                            otherItem.lastCraftingSlot.asign(otherItem);
                        }

                        aux = true;
                    }
                    }
                }
            }
            else
            {
                Type t = slot.GetType();
                
                if (t.Equals(typeof(CraftingSlot)))
                {
                    if (!eventData.pointerEnter.transform.parent.GetComponent<CraftingSlot>().isOutput)
                    {
                        //Debug.Log("a");
                        
                        dropItem.lastWasCraftingSlot = true;
                    
                        dropItem.lastCraftingSlot = eventData.pointerEnter.transform.parent.GetComponent<CraftingSlot>();
                    }
                    else
                    {
                        dropItem.rectTransform.anchoredPosition = startedPosition;

                        if (dropItem.lastWasCraftingSlot)
                        {
                            dropItem.lastCraftingSlot.asign(this);
                        }
                    }
                    
                    cf.update();
                }

                if (t.Equals(typeof(InventorySlots)))
                {
                    dropItem.lastWasInventorySlot = true;
                    
                    dropItem.lastInventorySlot = eventData.pointerEnter.transform.parent.GetComponent<InventorySlots>();

                    eventData.pointerEnter.transform.parent.GetComponent<InventorySlots>().item = this;
                }
            }
            if(eventData.pointerEnter.name == "Panel")
            {
                dropItem.dropearItem();
            }
            
            cf.update();

            return aux;
    }

    public void dropearItem()
    {
        GameObject drop = null;
                        
        drop = Instantiate(dropableItem,GameManager.instance.player.transform.position+(GameManager.instance.player.transform.forward*0.5f), Quaternion.identity);

        drop.GetComponent<DropableItem>().Item = item;
        
        drop.GetComponent<DropableItem>().itemMuch = itemMuch;
        
        drop.GetComponent<DropableItem>().chunkRenderer = chunkRenderer;
        
        drop.GetComponent<Rigidbody>().AddForce(GameManager.instance.player.transform.forward*200);

        Block block = GameManager.instance.getBlockData(item.BlockType);
                            
        if (block != null && block.particleMaterial != null)
        {
            drop.GetComponent<MeshRenderer>().material = block.particleMaterial;
        }
        else
        {
            drop.GetComponent<MeshRenderer>().material = GameManager.instance.defaultMaterial;
        }
        
        Destroy(this.gameObject);
    }
    
    public bool dropearSoloUno()
    {
        GameObject drop = null;
                        
        drop = Instantiate(dropableItem,GameManager.instance.player.transform.position+(GameManager.instance.player.transform.forward*0.5f)+new Vector3(0,0.5f,0), Quaternion.identity);

        drop.GetComponent<DropableItem>().Item = item;
        
        drop.GetComponent<DropableItem>().itemMuch = 1;
        
        drop.GetComponent<DropableItem>().chunkRenderer = chunkRenderer;
        
        drop.GetComponent<Rigidbody>().AddForce(GameManager.instance.player.transform.forward*800);

        Block block = GameManager.instance.getBlockData(item.BlockType);

        if (block.particleMaterial != null)
        {
            drop.GetComponent<MeshRenderer>().material = block.particleMaterial;
        }
        else
        {
            drop.GetComponent<MeshRenderer>().material = GameManager.instance.defaultMaterial;
        }
        
        int x = restarCantidad(1);

        if (x == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void resetStartPosition()
    {
        startedPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        PointerEventData = eventData;
        
        rectTransform.anchoredPosition += eventData.delta * canvas.scaleFactor * 1.5f * 1.5F;
        rectTransform.anchoredPosition = eventData.position;

        if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
        {
            transform.SetAsLastSibling();
        }

        /*if (Input.GetMouseButtonDown(1) && eventData.pointerEnter.name == "Panel")
        {
            dropearSoloUno();
        }*/
    }

    public void OnDrop(PointerEventData eventData)
    {
        /*if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<Slot>() == null)
            {
                rectTransform.anchoredPosition = startedPosition;
            }
        }
        else
        {
            rectTransform.anchoredPosition = startedPosition;
        }*/
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOutput && Input.GetKey(KeyCode.LeftShift))
        {
            realizarOutput(item);
            
            GameManager.instance.player.GetComponent<PlayerController3D>().updateItems();
        }
    }

    private void realizarOutput(Item item)
    {
        int x = cf.outputAllDone();
        isOutput = false;
        
        cf.update();

        itemMuch = x;
            
        GameManager.instance.player.GetComponent<PlayerController3D>().inventory.añadirItem(item,x,chunkRenderer);
        
        //GameManager.instance.player.GetComponent<PlayerController3D>().updateItems();
    }

    public void actualizarCantidad(int x)
    {
        itemMuch = x;
        texto.text = itemMuch.ToString();

        if (x == 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void sumarCantidad(int x)
    {
        itemMuch += x;
        texto.text = itemMuch.ToString();
    }
    
    public int restarCantidad(int x)
    {
        itemMuch -= x;
        texto.text = itemMuch.ToString();

        //En caso de que se destruya devolvemos un valor
        if (itemMuch <= 0)
        {
            Destroy(this.gameObject);
            return 0;
        }
        
        return -1;
    }

    public void borrarItem()
    {
        Destroy(this.gameObject);
    }
}
