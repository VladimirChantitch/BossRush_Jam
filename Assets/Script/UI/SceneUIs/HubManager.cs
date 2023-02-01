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
    UI_Inventory UI_inventory;
    UI_Crafter UI_crafter;

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
        crafter.Init(recipies);
        goblin.Init(recipies);

        UI_inventory = root.Q<UI_Inventory>("UI_Inventory");
        UI_crafter = root.Q<UI_Crafter>("UI_Crafrater");

        UI_inventory.Init();
        UI_crafter.Init();
    }


    private void BindEvents()
    {
        //Slots Events
        UI_crafter.onItemDeselected.AddListener(disSelected => crafter.DeselectSlot(disSelected));
        UI_inventory.onItemSelected.AddListener(selected => crafter.HandleSelection(selected));
        UI_inventory.onItemDeselected.AddListener(disSelected => crafter.DeselectSlot(disSelected));

        //CrafterEvents
        crafter.onFail.AddListener((fail_dto) => {UI_crafter.Fail(); });

        crafter.onInfo.AddListener((dto) => {UI_crafter.Info(dto); });

        crafter.onSuccess.AddListener((success_dto) =>
        {
            UI_crafter.CraftSuccess();
            UI_inventory.CraftSuccess();

            CrafterSuccess?.Invoke(success_dto);
            UI_crafter.visible = false;

            Debug.Log($"<color=yellow> CRAFTED WITH SUCESS {success_dto.resutl.name} !!!! </color>");
        });

        crafter.onDeselect.AddListener(item =>
        {
            UI_inventory.DeselectItem(item);
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
        UI_crafter.visible = false;
        UI_crafter.ClearAllSlots();
        UI_inventory.ClearAllSlots();
    }

    private void OpenCrafterMenu(HubInteractor hubInteractor)
    {
        UI_crafter.visible = true;
        AskOfrInventory?.Invoke(i => SetItemSlots(i));
    }

    public void SetItemSlots(List<AbstractItem> items) 
    {
        UI_inventory.SetItemSlots(items);
    }
}
