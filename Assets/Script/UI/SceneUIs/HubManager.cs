using Boss.inventory;
using Boss.Upgrades;
using Boss.crafter;
using Boss.Map;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Boss.Upgrades.UI;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

namespace Boss.UI
{
    public class HubManager : MonoBehaviour
    {
        public UnityEvent GoblinInteract = new UnityEvent();
        public UnityEvent<CrafterSuccessData> CrafterSuccess = new UnityEvent<CrafterSuccessData>();
        public UnityEvent<Action<List<AbstractItem>>> AskForInventory = new UnityEvent<Action<List<AbstractItem>>>();
        public UnityEvent<Action<List<GuitareUpgrade>>> AskForUpgrades = new UnityEvent<Action<List<GuitareUpgrade>>>();
        public UnityEvent<AbstractItem> onItemSetAsUpgrade = new UnityEvent<AbstractItem>();
        public UnityEvent<AbstractItem> onRemoveUpgrade = new UnityEvent<AbstractItem>();
        public UnityEvent<Action<float>> onRequestUseBlood = new UnityEvent<Action<float>>();

        VisualElement root;
        VisualElement crafterRoot;
        VisualElement inventoryRoot;
        VisualElement upgradeRoot;
        VisualElement dialogueRoot;
        

        UI_Inventory uI_inventory;
        UI_Crafter uI_crafter;
        UI_GuitareUpgrades uI_GuitareUpgrades;
        UI_Dialogue uI_Dialogue;

        [SerializeField] HubInteractor currentHubInteractor;
        [SerializeField] Crafter crafter;
        [SerializeField] Goblin goblin;
        [SerializeField] GuitareAspect guitareAspect;
        [SerializeField] HubBloodGauge hubBloodGauge;

        #region Initialisation
        public void Init(VisualElement root, List<Recipies> recipies)
        {
            this.root = root;


            SetRefs(recipies);
            BindEvents();
        }

        private void SetRefs(List<Recipies> recipies)
        {
            crafter = FindObjectOfType<Crafter>();
            goblin = FindObjectOfType<Goblin>();
            guitareAspect = FindObjectOfType<GuitareAspect>();
            hubBloodGauge = FindObjectOfType<HubBloodGauge>();

            crafter.Init(recipies);
            hubBloodGauge.Init();

            crafterRoot = root.Q<VisualElement>("Crafter");
            inventoryRoot = root.Q<VisualElement>("Inventory");
            upgradeRoot = root.Q<VisualElement>("Upgrades");
            dialogueRoot = root.Q<VisualElement>("Dialogue");


            uI_Dialogue = root.Q<UI_Dialogue>("UI_Dialogue");
            uI_inventory = root.Q<UI_Inventory>("UI_Inventory");
            uI_crafter = root.Q<UI_Crafter>("UI_Crafter");
            uI_GuitareUpgrades = root.Q<UI_GuitareUpgrades>("UI_GuitareUpgrades");

            uI_inventory.Init();
            uI_crafter.Init();
            uI_GuitareUpgrades.Init();
            uI_Dialogue.Init();

            crafterRoot.visible = false;
            inventoryRoot.visible = false;
            upgradeRoot.visible = false;
        }


        private void BindEvents()
        {
            //UI ___Crafter
            uI_crafter.onItemDeselected.AddListener(disSelected => crafter.DeselectSlot(disSelected));

            //UI ___Inventory
            uI_inventory.onItemSelected.AddListener(async selected => {
                if (currentHubInteractor is Crafter)
                {
                    crafter.HandleSelection(selected);
                }
                else
                {
                    if(selected is GuitareUpgrade guitareUpgrade)
                    {
                        onItemSetAsUpgrade.Invoke(guitareUpgrade);
                        AskForInventory?.Invoke(inventoryContent => SetInventoryItemSlots(inventoryContent));
                        guitareAspect.UpdateGuitareAspect(guitareUpgrade);
                        uI_GuitareUpgrades.SetInfo(guitareUpgrade);
                    }
                    //TODO -- give a feed back to tell thats not the rght item
                    uI_inventory.DeselectItem(null);
                }

                OpenDialogue(selected?.Description);
            });

            uI_inventory.onItemDeselected.AddListener(disSelected => {
                if (currentHubInteractor is Crafter)
                {
                    crafter.DeselectSlot(disSelected);
                    uI_crafter.Deselect(disSelected);
                }
            });

            //UI __Upgrades
            uI_GuitareUpgrades.onDisupgraded.AddListener(guitareUpgrade =>
            {
                onRemoveUpgrade?.Invoke(guitareUpgrade);
                AskForInventory?.Invoke(inventoryContent => SetInventoryItemSlots(inventoryContent));
            });


            //CrafterEvents
            crafter.onFail.AddListener((fail_dto) => { uI_crafter.Fail(); });

            crafter.onInfo.AddListener((dto) => { uI_crafter.Info(dto); });

            crafter.onSuccess.AddListener(async (success_dto) =>
            {
                CrafterSuccess?.Invoke(success_dto);
                CloseAllPopups();

                await Task.Delay(100);
                uI_crafter.CraftSuccess();
                uI_inventory.CraftSuccess();
                OpenDialogue(success_dto.resutl.Description);
                AskForInventory?.Invoke(inventoryContent => SetInventoryItemSlots(inventoryContent));
            });

            crafter.onDeselect.AddListener(item =>
            {
                uI_inventory.DeselectItem(item);
            });

            crafter.interacts.AddListener(() => OpenDialogue(crafter.AbstractDialogue?.dialogue));

            ///Blood Events
            hubBloodGauge.interacts.AddListener(() => onRequestUseBlood?.Invoke(amount => hubBloodGauge.UpdateAmount(amount)));
            hubBloodGauge.interacts.AddListener(() => OpenDialogue(hubBloodGauge.AbstractDialogue?.dialogue));

            //Goblin
            goblin.onPlayDialogue.AddListener(s =>
            {
                if (currentDialoguePlaying != null)
                {
                    StopCoroutine(currentDialoguePlaying);
                }

                currentDialoguePlaying = uI_Dialogue.SetNewDialogue(s);
                StartCoroutine(currentDialoguePlaying);
            });

            goblin.interacts.AddListener(() => OpenDialogue(goblin.AbstractDialogue?.dialogue));

            //UI_ Dialogue
            uI_Dialogue.onFinished?.AddListener(() => {
                dialogueRoot.visible = false;
                uI_Dialogue.visible = false;
            });
        }
        #endregion
        IEnumerator currentDialoguePlaying;

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
                    OpenCrafterMenu();
                    break;
                case MapInterractor mapInterractor:
                    mapInterractor.Interact();
                    break;
            }
        }

        #region setters
        public void SetGuitareSprite(List<GuitareUpgrade> guitareUpgrades)
        {
            guitareAspect.UpdateGuitareAspect(guitareUpgrades);
        }

        public void SetInventoryItemSlots(List<AbstractItem> items)
        {

            uI_inventory.SetItemSlots(items);
        }

        public void SetUpgradeItemSlots(List<GuitareUpgrade> items)
        {
            uI_GuitareUpgrades.SetInfo(items);
        }
        #endregion

        #region Open/CloseMenus
        private void OpenGuitareUpgradesMenu()
        {
            CloseAllPopups();
            upgradeRoot.visible = true;
            inventoryRoot.visible = true;
            uI_inventory.visible = true;
            uI_GuitareUpgrades.visible = true;
            AskForInventory?.Invoke(i => SetInventoryItemSlots(i));
            AskForUpgrades?.Invoke(u => SetUpgradeItemSlots(u));
        }

        private void OpenCrafterMenu()
        {
            CloseAllPopups();
            crafterRoot.visible = true;
            inventoryRoot.visible = true;
            uI_inventory.visible = true;
            uI_crafter.visible = true;
            AskForInventory?.Invoke(i => SetInventoryItemSlots(i));
        }

        public void OpenDialogue(string txt)
        {
            dialogueRoot.visible = true;
            uI_Dialogue.visible = true;
            goblin.onPlayDialogue?.Invoke(txt);
        }

        private void CloseAllPopups()
        {
            inventoryRoot.visible = false;  
            upgradeRoot.visible = false;
            uI_inventory.visible = false;
            uI_GuitareUpgrades.visible = false;
            crafterRoot.visible = false;
            uI_crafter.visible = false;
        }
        #endregion
    }
}

