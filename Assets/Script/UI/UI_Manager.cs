using Boss.inventory;
using Boss.loot;
using player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Boss.UI
{
    public class UI_Manager : MonoBehaviour
    {

        [SerializeField] GameManager.CurrentScrenn currentScrenn;
        public void SetCurrentScreen(GameManager.CurrentScrenn currentScrenn)
        {
            this.currentScrenn = currentScrenn; 
        }
        [HideInInspector] public UnityEvent SaveGame = new UnityEvent();
        [HideInInspector] public UnityEvent LoadGame = new UnityEvent();
        [HideInInspector] public UnityEvent LoadNewScreen = new UnityEvent();
        [HideInInspector] public UnityEvent DeleteSaveFile = new UnityEvent();
        [HideInInspector] public UnityEvent<CrafterSuccessData> CrafterSuccess = new UnityEvent<CrafterSuccessData>();
        [HideInInspector] public UnityEvent<Action<List<AbstractItem>>> AskOfrInventory = new UnityEvent<Action<List<AbstractItem>>>();
        [HideInInspector] public UnityEvent onGoToHub = new UnityEvent();
        [HideInInspector] public UnityEvent<AbstractItem> onItemSetAsUpgrade = new UnityEvent<AbstractItem>();
        [HideInInspector] public UnityEvent<AbstractItem> onRemoveUpgrade = new UnityEvent<AbstractItem>();

        [SerializeField] List<UI_Datafiles> datafiles = new List<UI_Datafiles> ();

        [SerializeField] UIDocument uIDocument;
        [SerializeField] MainMenuController mainMenuController;
        [SerializeField] HubManager hubManager;
        [SerializeField] UI_BossFight ui_bossFight;

        private void Awake()
        {
            if (uIDocument == null)
            {
                uIDocument = GetComponent<UIDocument>();
            }
        }

        /// <summary>
        /// Init the UI depending on the type of the scene
        /// </summary>
        public void Init(List<Recipies> recipies)
        {
            switch (currentScrenn)
            {
                case GameManager.CurrentScrenn.MainMenu:
                    MainMenu();
                    break;
                case GameManager.CurrentScrenn.Hub:
                    Hub(recipies);
                    break;
                case GameManager.CurrentScrenn.BossRoom:
                    BossRoom();
                    break;
            }
        }

        #region mainMenu
        /// <summary>
        /// Load main meanu manager
        /// </summary>
        public void MainMenu()
        {
            uIDocument.visualTreeAsset = datafiles.Where(file => file.GetScreen() == GameManager.CurrentScrenn.MainMenu).First().GetVisualTreeAsset();

            Type[] types = new Type[1] { typeof(MainMenuController) };
            GameObject go = new GameObject("MainMenuController", types);
            go.transform.parent = transform;

            mainMenuController = go.GetComponent<MainMenuController>();
            mainMenuController.Init(uIDocument.rootVisualElement);

            mainMenuController.continuEvent.AddListener(() => LoadGame?.Invoke());
            mainMenuController.startEvent.AddListener(() =>
            {
                DeleteSaveFile?.Invoke();
                SceneManager.LoadScene("Hub");
            });
        }
        #endregion

        #region hub
        /// <summary>
        /// Load the hub controller
        /// </summary>
        public void Hub(List<Recipies> recipies)
        {
            uIDocument.visualTreeAsset = datafiles.Where(file => file.GetScreen() == GameManager.CurrentScrenn.Hub).First().GetVisualTreeAsset();

            Type[] types = new Type[1] { typeof(HubManager) };
            GameObject go = new GameObject("HubManager", types);
            go.transform.parent = transform;

            hubManager = go.GetComponent<HubManager>();
            hubManager.GoblinInteract.AddListener(() => Debug.Log("I'm a goblin"));
            hubManager.CrafterSuccess.AddListener(succes_dto =>
            {
                CrafterSuccess?.Invoke(succes_dto);
            });
            hubManager.AskOfrInventory.AddListener(action =>
            {
                AskOfrInventory?.Invoke(action);
            });
            hubManager.onItemSetAsUpgrade.AddListener(item => onItemSetAsUpgrade?.Invoke(item));

            hubManager.Init(uIDocument.rootVisualElement, recipies);
        }
        #endregion

        #region boss
        /// <summary>
        /// When the fight has finished and the boss gives loots 
        /// </summary>
        /// <param name="bossLootData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ShowLoots(BossLootData bossLootData)
        {
            ui_bossFight.ShowLoots(bossLootData);
        }

        /// <summary>
        /// Called when the fight is finished (player or boss death)
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void PlayerLoose()
        {
            ui_bossFight.OpenDeathScreen();
        }

        /// <summary>
        /// Load Boss Room
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void BossRoom()
        {
            uIDocument.visualTreeAsset = datafiles.Where(file => file.GetScreen() == GameManager.CurrentScrenn.BossRoom).First().GetVisualTreeAsset();
            Type[] types = new Type[1] { typeof(UI_BossFight) };
            GameObject go = new GameObject("HubManager", types);
            go.transform.parent = transform;

            ui_bossFight = go.GetComponent<UI_BossFight>();
            ui_bossFight.Init(uIDocument.rootVisualElement);

            ui_bossFight.onBackToHub.AddListener(() => onGoToHub?.Invoke());
        }

        #endregion

        [Serializable]
        public class UI_Datafiles
        {
            [SerializeField] GameManager.CurrentScrenn CurrentScrenn;
            [SerializeField] VisualTreeAsset treeAsset;

            public VisualTreeAsset GetVisualTreeAsset()
            {
                return treeAsset;
            }

            public GameManager.CurrentScrenn GetScreen()
            {
                return CurrentScrenn;
            }
        }
    }
}

