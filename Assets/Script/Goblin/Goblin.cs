using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;
using Boss.Dialogue;

public class Goblin : HubInteractor
{
    internal void Init()
    {

    }

    bool playerHasJustLost = false;
    bool firstTimeUsingGauge = true;
    bool firtTimeCrafting = true;

    List<AbstractDialogue> unLockedDialogues = new List<AbstractDialogue>();
    List<AbstractDialogue> newDialogue = new List<AbstractDialogue>();
    
    public void ActionFailledDialogue(ActionTypes action)
    {
        //relier au player qui a �chou� � une action
    }

    public void PlayerComingBackDialogue()
    {
        //relier au player qui revient
    }

    public void PlayerGoingDialogue()
    {
        ///relier � l'appelle de la map :p
    }

    public void PlayerTutorial()
    {
        //Gros pav� pour le tutorial.
    }
}
