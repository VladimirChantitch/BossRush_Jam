using Boss.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class HubManager : MonoBehaviour
{
    public UnityEvent GoblinInteract = new UnityEvent();
    public UnityEvent CrafterInteract = new UnityEvent();

    VisualElement root;
    List<UI_ItemSlot> itemSlots = new List<UI_ItemSlot>();
    List<UI_ItemSlot> crafterSlots = new List<UI_ItemSlot>();

    [SerializeField] HubInteractor currentHubInteractor;

    public void Init(VisualElement root)
    {
        this.root = root;
        SetRefs();
        BindEvents();
    }

    private void SetRefs()
    {
        List<VisualElement> slots = root.Q<VisualElement>("InventoryView").Children().Where(c => c is UI_ItemSlot).ToList();
        slots.ForEach(s => itemSlots.Add(s as UI_ItemSlot));
        slots = root.Q<VisualElement>("Crafter").Children().Where(c => c is UI_ItemSlot).ToList();
        slots.ForEach(s => crafterSlots.Add(s as UI_ItemSlot));
    }


    private void BindEvents()
    {
        throw new NotImplementedException(); 
    }

    // Update is called once per frame
    void Update()
    {
        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<HubInteractor>() is HubInteractor hubInteractor)
                {
                    HandleInteractable(hubInteractor);
                }
            }
        }
    }

    private void HandleInteractable(HubInteractor interactable)
    {
        interactable.interacts?.Invoke();
        switch (interactable.interactables)
        {
            case HubInteractor.Interactables.Door:
                break;
            case HubInteractor.Interactables.Wheel:
                break;
            case HubInteractor.Interactables.Goblin:
                GoblinInteract?.Invoke();
                break;
            case HubInteractor.Interactables.Crafter:
                CrafterInteract?.Invoke();
                break;
            case HubInteractor.Interactables.Gauge:
                break;
            case HubInteractor.Interactables.Weapon:
                break;
            case HubInteractor.Interactables.Map:
                break;
            case HubInteractor.Interactables.Pot_1:
                break;
            case HubInteractor.Interactables.Pot_2:
                break;
            case HubInteractor.Interactables.Pot_3:
                break;
        }
    }
}
