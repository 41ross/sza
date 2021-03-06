﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;

    public List<QuestEvent> questEvents;
    public List<QuestDialog> questDialogs;

    public int currentEventIndex = 0;
    public int currentDialogeIndex = 0;

    QuestDialog currentQuestDialog = null;

    public QuestDialog GetCurrentQuestDialog() 
    {
        currentQuestDialog = questDialogs[currentDialogeIndex];

        return currentQuestDialog;
    }

    public QuestEvent NextQuestEvent()
    {
        QuestEvent eventToReturn = null;

        if (currentEventIndex < questEvents.Count) 
        {
            eventToReturn = questEvents[currentEventIndex];
            currentEventIndex++;
        }

        return eventToReturn;
    }

    public QuestDialog NextDialog() 
    {

        if (currentDialogeIndex < questDialogs.Count)
        {
            currentQuestDialog = questDialogs[currentDialogeIndex];
            currentDialogeIndex++;
        }

        return currentQuestDialog;
    }

    // QUEST LINES

    public bool StartDialog(QuestDialog questDialog, string speaker)
    {
        DialogueManager dialogue = GameObject.FindGameObjectWithTag("dialogWindow").GetComponent<DialogueManager>();
        
        if (dialogue.speaker == speaker) 
        { 
            dialogue.SetDialog(questDialog.dialog);

        }

        return dialogue.isLastPart();
    }

    public bool Gather(Item item, GameObject gatherPoint, int count = -1)
    {
        Item itemInHand = gatherPoint.GetComponent<ItemCell>().item;

        return itemInHand.IsSameItems(item);
        
    }

    public bool Use(string arg) 
    {
        GameObject dt = GameObject.FindGameObjectWithTag("dialogText");
        if (dt == null) 
        {
            return false;
        }

        return dt.GetComponent<Text>().text == arg;
    }

    public bool Spawn(GameObject spawnController, Vector2 spawnPosition) 
    {
        Instantiate(spawnController, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
        ISpawn spawn = spawnController.gameObject.GetComponent<ISpawn>();

        if (spawn != null)
        {
            spawn.Spawn();
            return true;
        }

        return false;
    }

    public bool EndQuest() 
    {

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        ProgressSceneLoader sceneLoader = Global.Component.GetProgressSceneLoader();
        sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        return true;
    }

    public bool Dialogue(string speacker) 
    {
        DialogueManager dialogue = GameObject.FindGameObjectWithTag("dialogWindow").GetComponent<DialogueManager>();
        
        if (!dialogue.isOpen) 
        {
            return false;
        }

        return dialogue.speaker == speacker && dialogue.isLastPart();
        
    }
}
