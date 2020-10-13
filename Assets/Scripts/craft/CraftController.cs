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
    public bool Craft_Table(RaycastHit2D[] hits, Item tool, CraftType craftType, CraftTable craftTable) 
    {
        GameObject GameObjOnTable = GetGameObjOnTable(hits);
        
        if (GameObjOnTable == null) 
        {
            Debug.Log("TableIsEmpty");
            return false;        
        }

        Item itemOnTabe = GameObjOnTable.GetComponent<ItemCell>().item;

        ItemCraftData recept = FindRecept(tool, itemOnTabe, craftType, craftTable);

        if (recept == null) 
        {
            Debug.Log("no recept");
            return false;
        }

        Item craftResult = recept.recept.craftResult;

        GameObjOnTable.GetComponent<ItemCell>().item = craftResult;
        GameObjOnTable.GetComponent<SpriteRenderer>().sprite = craftResult.itemSprite;

        return recept.removeTool;
    }

    public void Craft_Microwave(MicrowaveController microwave, Item hand, CraftType craftType, CraftTable craftTable) 
    {
        if (microwave.isOpen && microwave.itemInside)
        {
            microwave.Close();
        }
        else if (!microwave.isOpen && microwave.itemInside) 
        {
            StartCoroutine(microwave.Work());

            ItemCraftData recept = FindRecept(hand, microwave.itemInside, craftType, craftTable);

            if (recept == null)
            {
                Debug.Log("no recept");
                return;
            }

            Item craftResult = recept.recept.craftResult;
            Debug.Log(craftResult.itemName);
            microwave.itemInside = craftResult;
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

    ItemCraftData FindRecept(Item tool, Item originItem, CraftType craftType, CraftTable craftTable) 
    {
        List<ItemCraftData> sameTool = new List<ItemCraftData>();

        foreach (var cd in itemCraftData)
        {
            if (cd.recept.craftTool.Contains(tool))
            {
                sameTool.Add(cd);
            }
        }

        Debug.Log(sameTool.Count);

        List<ItemCraftData> sameType = sameTool
                    .Where(r => r.craftType == craftType)
                    .Where(r => r.craftTable == craftTable).ToList();

        ItemCraftData recept = sameType
                    .Where(r => r.recept.ingredients[0].itemName.Equals(originItem.itemName))
                    .FirstOrDefault();

        return recept;
    }
}