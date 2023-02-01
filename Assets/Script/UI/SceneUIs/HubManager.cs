using Boss.inventory;
using Boss.UI;
using Boss.crafter;
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
    public UnityEvent<CrafterSuccessData> CrafterSuccess = new UnityEvent<CrafterSuccessData>();
    public UnityEvent<Action<List<AbstractItem>>> AskOfrInventory = new UnityEvent<Action<List<AbstractItem>>>();

    VisualElement root;
    VisualElement crafterRoot;
    List<UI_ItemSlot> itemSlots = new List<UI_ItemSlot>();
    List<UI_ItemSlot> crafterSlots = new List<UI_ItemSlot>();

    [SerializeField] HubInteractor currentHubInteractor;
    [SerializeField] Crafter crafter;
    [SerializeField] Goblin goblin;

    public void Init(VisualElement root, List<Recipies> recipies)
    {
        this.root = root;
        crafter = FindObjectOfType<Crafter>();
        goblin = FindObjectOfType<Goblin>();

        SetRefs(recipies);
        BindEvents();
    }

    private void SetRefs(List<Recipies> recipies)
    {
        crafterRoot = root.Q<VisualElement>("CrafterPopUp");
        crafterRoot.visible = false;
        crafter.Init(recipies);
        goblin.Init(recipies);

        List<VisualElement> slots = root.Q<VisualElement>("InventoryView").Children().ToList();
        slots.ForEach(s => itemSlots.Add(s as UI_ItemSlot));
        slots = root.Q<VisualElement>("Crafter").Children().ToList();
        slots.ForEach(s => crafterSlots.Add(s as UI_ItemSlot));
    }


    private void BindEvents()
    {
        //Slots Events
        crafterSlots.ForEach(c =>
        {
            c.ItemSelected.AddListener(t => Debug.Log("nothing, but thats ok"));
            c.ItemDeselected.AddListener(disSelected => crafter.DeselectSlot(disSelected));
        });

        itemSlots.ForEach(c =>
        {
            c.ItemSelected.AddListener(selected => crafter.HandleSelection(selected));
            c.ItemDeselected.AddListener(disSelected => crafter.DeselectSlot(disSelected));
        });

        //CrafterEvents
        crafter.onFail.AddListener((fail_dto) =>
        {
            crafterSlots.ForEach(c => c.Clean());
        });

        crafter.onInfo.AddListener((dto) =>
        {
            crafterSlots.ForEach(c => c.Clean());
            if (dto.item_1 != null) crafterSlots[0].Init(dto.item_1);
            if (dto.item_2 != null) crafterSlots[1].Init(dto.item_2);
        });

        crafter.onSuccess.AddListener((success_dto) =>
        {
            crafterSlots.ForEach(c => c.Clean());
            itemSlots.ForEach(c => {
                c.RemoveOverriderClass();
                c.isSelected = false;
            });
            CrafterSuccess?.Invoke(success_dto);
            crafterRoot.visible = false;

            Debug.Log($"<color=yellow> CRAFTED WITH SUCESS {success_dto.resutl.name} !!!! </color>");
        });

        crafter.onDeselect.AddListener(item =>
        {
            itemSlots.Find(i => i.Item == item).RemoveOverriderClass();
        });
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
        //CloseAllPopUp();
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
                OpenCrafterMenu(interactable);
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

    private void CloseAllPopUp()
    {
        crafterRoot.visible = false;
        crafterSlots.ForEach(c => c.Clean());
        itemSlots.ForEach(c => c.Clean());
    }

    private void OpenCrafterMenu(HubInteractor hubInteractor)
    {
        crafterRoot.visible = true;
        AskOfrInventory?.Invoke(i => SetItemSlots(i));
        //crafterRoot.transform.position = hubInteractor.transform.position;
        //crafterRoot.style.right = 0;
        //crafterRoot.style.left = 0;
        //crafterRoot.style.top = 0;
        //crafterRoot.style.
    }

    public void SetItemSlots(List<AbstractItem> items) 
    {
        for(int i =0; i < items.Count; i++)
        {
            itemSlots[i].Init(items[i]);
        }
    }
}
