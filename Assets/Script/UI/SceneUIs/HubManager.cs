using Boss.inventory;
using Boss.Upgrades;
using Boss.crafter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Boss.Upgrades.UI;

namespace Boss.UI
{
    public class HubManager : MonoBehaviour
    {
        public UnityEvent GoblinInteract = new UnityEvent();
        public UnityEvent<CrafterSuccessData> CrafterSuccess = new UnityEvent<CrafterSuccessData>();
        public UnityEvent<Action<List<AbstractItem>>> AskOfrInventory = new UnityEvent<Action<List<AbstractItem>>>();

        VisualElement root;
        VisualElement crafterRoot;
        VisualElement inventoryRoot;
        VisualElement upgradeRoot;

        UI_Inventory uI_inventory;
        UI_Crafter uI_crafter;
        UI_GuitareUpgrades uI_GuitareUpgrades;

        [SerializeField] HubInteractor currentHubInteractor;
        [SerializeField] Crafter crafter;
        [SerializeField] Goblin goblin;
        [SerializeField] GuitareAspect guitare;

        #region Initialisation
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

            crafterRoot = root.Q<VisualElement>("Crafter");
            inventoryRoot = root.Q<VisualElement>("Inventory");
            upgradeRoot = root.Q<VisualElement>("Upgrades");
            
            uI_inventory = root.Q<UI_Inventory>("UI_Inventory");
            uI_crafter = root.Q<UI_Crafter>("UI_Crafter");
            uI_GuitareUpgrades = root.Q<UI_GuitareUpgrades>("UI_GuitareUpgrades");

            uI_inventory.Init();
            uI_crafter.Init();
            uI_GuitareUpgrades.Init();

            crafterRoot.visible = false;
            inventoryRoot.visible = false;
            upgradeRoot.visible = false;
        }


        private void BindEvents()
        {
            //UI ___Crafter
            uI_crafter.onItemDeselected.AddListener(disSelected => crafter.DeselectSlot(disSelected));

            //UI ___Inventory
            uI_inventory.onItemSelected.AddListener(selected => {
                if (currentHubInteractor is Crafter)
                {
                    crafter.HandleSelection(selected);
                }
                else
                {
                    uI_GuitareUpgrades.SetInfo(selected as GuitareUpgrade);
                    //TODO -- Send to player the new powerups
                }
            });

            uI_inventory.onItemDeselected.AddListener(disSelected => {
                crafter.DeselectSlot(disSelected);
                uI_crafter.Deselect(disSelected);
            });

            //UI __Upgrades
            uI_GuitareUpgrades.onDisupgraded.AddListener(data =>
            {
                guitare.UpdateGuitareAspect(data);
                //TODO -- Send to player to delete precedent powerups
            });


            //CrafterEvents
            crafter.onFail.AddListener((fail_dto) => { uI_crafter.Fail(); });

            crafter.onInfo.AddListener((dto) => { uI_crafter.Info(dto); });

            crafter.onSuccess.AddListener((success_dto) =>
            {
                uI_crafter.CraftSuccess();
                uI_inventory.CraftSuccess();

                CrafterSuccess?.Invoke(success_dto);
                crafterRoot.visible = false;
                inventoryRoot.visible = false;

                Debug.Log($"<color=yellow> CRAFTED WITH SUCESS {success_dto.resutl.name} !!!! </color>");
            });

            crafter.onDeselect.AddListener(item =>
            {
                uI_inventory.DeselectItem(item);
            });
        }
        #endregion

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

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
            switch (interactable)
            {
                case Goblin:
                    currentHubInteractor = interactable;
                    GoblinInteract?.Invoke();
                    inventoryRoot.visible = true;
                    upgradeRoot.visible = true;
                    OpenGuitareUpgradesMenu();
                    break;
                case Crafter:
                    currentHubInteractor = interactable;
                    inventoryRoot.visible = true;
                    crafterRoot.visible = true;
                    OpenCrafterMenu(interactable);
                    break;
            }
        }

        #region setters
        public void SetGuitareSprite(List<GuitareUpgrade> guitareUpgrades)
        {
            guitare.UpdateGuitareAspect(guitareUpgrades);
        }

        public void SetInventoryItemSlots(List<AbstractItem> items)
        {
            uI_inventory.SetItemSlots(items);
        }
        #endregion

        #region Open/CloseMenus
        private void OpenGuitareUpgradesMenu()
        {
            upgradeRoot.visible = true;
        }

        private void OpenCrafterMenu(HubInteractor hubInteractor)
        {
            crafterRoot.visible = true;
            AskOfrInventory?.Invoke(i => SetInventoryItemSlots(i));
        }

        private void CloseAllPopups()
        {

        }
        #endregion
    }
}

