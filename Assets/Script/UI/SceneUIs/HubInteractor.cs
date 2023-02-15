using Boss.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Boss.UI
{
    public class HubInteractor : MonoBehaviour
    {
        public UnityEvent interacts = new UnityEvent();

        [SerializeField] AbstractDialogue abstractDialogue;

        public AbstractDialogue AbstractDialogue { get => abstractDialogue; }
    }
}
