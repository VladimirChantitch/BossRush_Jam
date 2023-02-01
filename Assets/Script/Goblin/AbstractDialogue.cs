using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Dialogue
{
    public abstract class AbstractDialogue : ScriptableObject
    {
        [SerializeField] string title;
        [SerializeField][TextArea] protected string dialogue;
    }
}


