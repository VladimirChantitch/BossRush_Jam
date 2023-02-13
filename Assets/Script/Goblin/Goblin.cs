using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;
using Boss.Dialogue;
using UnityEngine.Events;

public class Goblin : HubInteractor
{
    public UnityEvent<string, bool> onPlayDialogue = new UnityEvent<string, bool>();  

}
