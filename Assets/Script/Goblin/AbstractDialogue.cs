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
    }
}

public enum DialogueActionType
{
    LooseDialogue,
    WinDialogue,
    BossBeatenDialogue,
    Tutorial,
    Actionfailled
}


