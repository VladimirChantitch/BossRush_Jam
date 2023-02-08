using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue")]
    public class AbstractDialogue : ScriptableObject
    {
        [SerializeField] public string title;
        [SerializeField][TextArea] public string dialogue;
        public DialogueActionType type;
        public AbstractDialogue nextDialogue;
        [Header("has Sens Only for Tutorial and ActionFailled")]
        public ActionTypes actionType;
    }
}

public enum ActionTypes
{
    NONE,
    Craft,
    Upgrade,
    BloodUse
}

public enum DialogueActionType
{
    LooseDialogue,
    WinDialogue,
    BossBeatenDialogue,
    Tutorial,
    Actionfailled
}


