﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour
{
    static ToolTipController instance;

    [SerializeField]
    Camera uiCamera;

    [SerializeField]
    GameObject toolTip = null;

    [SerializeField]
    float textPaddinSize = 4f;

    [SerializeField]
    float actionDistanceCof = 1f;

    Image bgImage;
    Text textItemName;
    Text textInteraction;
    RectTransform bgRectTransform;
    RectTransform UIRectTransform;

    Transform playerTransform;
    Controller controller;

    bool isTooltipOpen = false;
    float preferredHeight = 0;

    void Start()
    {
        bgImage = toolTip.transform.GetChild(0).GetComponent<Image>();
        textItemName = bgImage.transform.GetChild(0).GetComponent<Text>();
        textInteraction = bgImage.transform.GetChild(1).GetComponent<Text>();
        bgRectTransform = toolTip.GetComponent<RectTransform>();
        UIRectTransform = Global.UIElement.GetUI().GetComponent<RectTransform>();
        playerTransform = Global.Obj.GetPlayerGameObject().GetComponent<Transform>();
        controller = Global.Component.GetController();

        instance = this;

        preferredHeight = textItemName.preferredHeight;

        //ShowToolTip("qwe", "tretr");
        HideToolTip();
    }

    bool isdetected = false;
    
    
    void Update()
    {
        if (IsInActionRadius() == true)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length == 0)
            {
                isdetected = false;
            }

            foreach (var hit in hits)
            {
                //Debug.Log(hit.collider.name);

                if (hit.collider.tag == "table")
                {
                    TableController tableController = hit.collider.GetComponent<TableController>();

                    isdetected = true;
                    Vector2 tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                    if (IsCurrentHandEmpty() && IsObjectExist(hits, Global.DROPED_ITEM_PREFIX, false) == false)
                    {
                        ShowToolTip((tableController.tableName == string.Empty) ? "table" : tableController.tableName,
                                PrintRed(Global.Tooltip.NO_ACTIONS));
                    }
                    else if (IsCurrentHandEmpty() && IsObjectExist(hits, Global.DROPED_ITEM_PREFIX, false) == true)
                    {
                        ShowToolTip((tableController.tableName == string.Empty) ? "table" : tableController.tableName,
                                Global.Tooltip.LM_PICK_UP);
                    }
                    else if (IsCurrentHandEmpty() == false && IsObjectExist(hits, Global.DROPED_ITEM_PREFIX, false) == true)
                    {
                        ShowToolTip((tableController.tableName == string.Empty) ? "table" : tableController.tableName,
                                Global.Tooltip.LM_PUT + ((tableController.isCraftTable) ? " / " + Global.Tooltip.RM_CRAFT : string.Empty));
                    }
                    else
                    {
                        ShowToolTip((tableController.tableName == string.Empty) ? "table" : tableController.tableName,
                                Global.Tooltip.LM_PUT);
                    }

                    TooltipLocate(tooltipPosition);

                    return;
                }
                else if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX))
                {
                    bool onTable = false;
                    foreach (var hitTable in hits)
                    {
                        if (hitTable.collider.tag == "table")
                        {
                            onTable = true;
                        }
                    }

                    if (onTable == false)
                    {

                        Item item = hit.collider.GetComponent<ItemCell>().item;

                        isdetected = true;
                        Vector2 tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                        ShowToolTip(item.itemName, (controller.IsEmpty(controller.currentHand) == true) ?
                            Global.Tooltip.LM_PICK_UP : PrintRed(Global.Tooltip.NO_ACTIONS));
                        TooltipLocate(tooltipPosition);

                        return;
                    }
                }
                else if (hit.collider.tag == "case")
                {
                    CaseController caseController = hit.collider.GetComponent<CaseController>();

                    isdetected = true;
                    Vector2 tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                    ShowToolTip((caseController.caseName == string.Empty) ? "case" : caseController.caseName, (caseController.isOpen == false)
                                ? Global.Tooltip.LM_OPEN : Global.Tooltip.LM_CLOSE);
                    TooltipLocate(tooltipPosition);

                    return;
                }
                else if (hit.collider.tag == "tv") 
                {
                    TVController tvController = hit.collider.GetComponent<TVController>();

                    isdetected = true;
                    Vector2 tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                    ShowToolTip("tv", (tvController.IsTvOpen() == false)
                                ? Global.Tooltip.LM_TURN_ON : Global.Tooltip.LM_TURN_OFF + " / " + Global.Tooltip.RM_NEXT_CHANNEL);
                    TooltipLocate(tooltipPosition);

                    return;
                }
                else
                {
                    isdetected = false;
                }
            }

            if (isTooltipOpen == true && isdetected == false)
            {
                HideToolTip();
            }
        }
        else 
        {
            if (isTooltipOpen == true) 
            {
                HideToolTip();
            }
        }

        

    }

    void ShowToolTip(string itemName, string itemInteraction) 
    {
        toolTip.SetActive(true);
        isTooltipOpen = true;

        textItemName.text = itemName;
        textInteraction.text = itemInteraction;

        Vector2 bgSize = Vector2.zero;

        int itemInteractionLength = itemInteraction.Length;

        // если текст цветной
        if (itemInteraction.Contains(Global.Color.RED_COLOR_PREFIX)) 
        {
            itemInteractionLength -= Global.Color.RED_COLOR_PREFIX.Length + Global.Color.END_COLOR_PREFIX.Length;
        }

        if (itemName.Length >= itemInteractionLength)
        {
            bgSize = new Vector2(textItemName.preferredWidth + textPaddinSize * 2,
                (textInteraction.preferredHeight * 2) + textPaddinSize * 2);

          //  textInteraction.alignment = TextAnchor.MiddleCenter;
        }
        else 
        {
            bgSize = new Vector2(textInteraction.preferredWidth + textPaddinSize * 2, 
                (textInteraction.preferredHeight * 2) + textPaddinSize * 2);

           // textItemName.alignment = TextAnchor.MiddleCenter;
        }

        bgRectTransform.sizeDelta = bgSize;

        bgSize.y = bgSize.y + preferredHeight / 2;


        bgImage.rectTransform.sizeDelta = bgSize;
    }

    void HideToolTip() 
    {
      //  textItemName.alignment = TextAnchor.MiddleLeft;
      //  textInteraction.alignment = TextAnchor.MiddleLeft;

        toolTip.SetActive(false);
        isTooltipOpen = false;
    }

    void TooltipLocate(Vector2 pos) 
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIRectTransform,
            pos, uiCamera, out localPoint);

        localPoint.y = localPoint.y + preferredHeight * 4;
        toolTip.transform.localPosition = localPoint;
    }

    bool IsInActionRadius() 
    {
        return Vector2.Distance(playerTransform.position, 
            Camera.main.ScreenToWorldPoint(Input.mousePosition)) 
            < Controller.GetActioPlayerRadiusStatic() * actionDistanceCof;
    }

    string PrintRed(string str) 
    {
        return Global.Color.RED_COLOR_PREFIX + str + Global.Color.END_COLOR_PREFIX;
    }

    bool IsCurrentHandEmpty() 
    {
        return controller.IsEmpty(controller.currentHand);
    }

    bool IsObjectExist(RaycastHit2D[] hits, string objId, bool isTag) 
    {
        if (isTag == true)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.tag == objId)
                {
                    return true;
                }
            }
        }
        else 
        {
            foreach (var hit in hits)
            {
                if (hit.collider.name.Contains(objId))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static void Show(string itemName, string itemInteraction) 
    {
        instance.ShowToolTip(itemName, itemInteraction);
    }

    public static void Hide()
    {
        instance.HideToolTip();
    }
}
