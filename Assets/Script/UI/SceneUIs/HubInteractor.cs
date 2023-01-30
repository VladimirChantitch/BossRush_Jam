using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Boss.UI
{
    public class HubInteractor : MonoBehaviour
    {
        public enum Interactables
        {
            Door,
            Wheel,
            Goblin,
            Crafter,
            Gauge,
            Weapon,
            Map,
            Pot_1,
            Pot_2,
            Pot_3
        }

        public Interactables interactables;
        public UnityEvent interacts = new UnityEvent();
    }
}
