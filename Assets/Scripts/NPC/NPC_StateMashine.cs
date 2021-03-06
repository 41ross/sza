﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ByTheTale.StateMachine;
using System;
public enum StateTypes 
{
    NPC_STATE_clickWaiting,
    NPC_STATE_dialogue,
    NPC_STATE_itemRequier,
    NPC_STATE_thinking,
    NPC_STATE_tableItemCheck,
    NPC_STATE_itemReturn,
    NPC_STATE_walkTill,
    NPC_STATE_openDoor,
    NPC_STATE_waitTillPlayerReachPos,
    NPC_STATE_walk,
    NPC_STATE_actionComplete,
    NPC_STATE_stateTransitionModify,
    NPC_STATE_tableLuggageCheck,
    NPC_STATE_spawnItem
}

public class NPC_StateMashine : MachineBehaviour
{
    public List<StateTypes> stateList;

    public override void AddStates()
    {
        foreach (var st in stateList)
        {
            AddState(Type.GetType(st.ToString()));
        }

        SetInitialState(Type.GetType(stateList[0].ToString()));
    }
}
