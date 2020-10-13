﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftController : MonoBehaviour
{
    List<ItemCraftData> itemCraftData;

    void Awake()
    {
        itemCraftData = Resources.LoadAll<ItemCraftData>(Global.Path.RECEPT).ToList();
    }
    public void Craft_Table(RaycastHit2D[] hits, Item tool) 
    {
        GameObject GameObjOnTable = GetGameObjOnTable(hits);
        
        if (GameObjOnTable == null) 
        {
            Debug.Log("TableIsEmpty");
            return;        
        }

        Item itemOnTabe = GameObjOnTable.GetComponent<ItemCell>().item;

        List<ItemCraftData> sameTool = new List<ItemCraftData>();
        
        foreach (var cd in itemCraftData)
        {
            if (cd.recept.craftTool.Contains(tool)) 
            {
                sameTool.Add(cd);
            }
        }
        Debug.Log(sameTool.Count);

        ItemCraftData recept = sameTool
            .Where(r => r.recept.ingredients[0].itemName.Equals(itemOnTabe.itemName))
            .FirstOrDefault();

        if (recept == null) 
        {
            Debug.Log("no recept");
            return;
        }

        Item craftResult = recept.recept.craftResult;

        GameObjOnTable.GetComponent<ItemCell>().item = craftResult;
        GameObjOnTable.GetComponent<SpriteRenderer>().sprite = craftResult.itemSprite;
        
    }

    public void Craft_Microwave(MicrowaveController microwave) 
    {
        if (microwave.isOpen && microwave.itemInside)
        {
            microwave.Close();
        }
        else if (!microwave.isOpen && microwave.itemInside) 
        {
            StartCoroutine(microwave.Work());
        }


    }

    GameObject GetGameObjOnTable(RaycastHit2D[] hits) 
    {
        foreach (var hit in hits)
        {
            if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX)) 
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }

}
