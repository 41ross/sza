﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EWFormCheckerController : EWBase, IEWInit
{
    [SerializeField]
    Text idText = null;

    [SerializeField]
    Text formText = null;

    [SerializeField]
    Text statusText = null;

    List<Item> savedItems = new List<Item>();

    public GameObject prefToSpawn;

    public Item id = null;

    public Item form = null;

    const string idNotPresent = "ID not present";
    const string formNotPresent = "Form not present";

    const string idPresent = "ID present";
    const string formPresent = "Form present";

    void Update()
    {
        if (IsPlayerInEWindowRadius() == false) 
        {
            Close();
        }



        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider.tag == "envObj")
                {
                    Item itemInHand = controller.GetItemInHand(controller.currentHand);

                    if (id.IsSameItems(itemInHand))
                    {
                        if (!savedItems.Contains(id)) 
                        {
                            savedItems.Add(id);
                            idText.text = idPresent;
                            controller.SetDefaultItem(controller.currentHand);
                        }
                    }
                    else if (form.IsSameItems(itemInHand)) 
                    {
                        if (!savedItems.Contains(form))
                        {
                            savedItems.Add(form);
                            formText.text = formPresent;
                            controller.SetDefaultItem(controller.currentHand);
                        }

                    }
                    
                    return;
                    //if (id.IsSameItems(controller.GetItemInHand(controller.currentHand)))
                    //foreach (var item in reqItems)
                    //{
                    //    if (item.IsSameItems(controller.GetItemInHand(controller.currentHand))) 
                    //    {
                    //        if (!savedItems.Contains(item)) 
                    //        {
                    //            savedItems.Add(item);
                    //            controller.SetDefaultItem(controller.currentHand);

                    //        }
                    //    }
                    //}
                }
            }
        }
    }
    public void Init(GameObject window, GameObject envObj)
    {
        BaseInit(window, envObj);
    }

    public void OnPullOutClick(bool isId) 
    {
        if (isId && savedItems.Contains(id))
        {
            savedItems.Remove(id);
            idText.text = idNotPresent;

            Item idClone = Instantiate(id);
            prefToSpawn.GetComponent<ItemCell>().item = idClone;
            prefToSpawn = Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + prefToSpawn.name;
        }
        else if (!isId && savedItems.Contains(form)) 
        {
            savedItems.Remove(form);
            formText.text = formNotPresent;

            Item formClone = Instantiate(form);
            prefToSpawn.GetComponent<ItemCell>().item = formClone;
            prefToSpawn = Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + prefToSpawn.name;
        }
    }

    public void OnCheckClick() 
    { 
        
    }
}
