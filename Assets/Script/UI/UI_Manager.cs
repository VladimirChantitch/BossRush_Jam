using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Boss.UI
{
    public class UI_Manager : MonoBehaviour
    {
        public enum CurrentScrenn { MainMenu, Hub, BossRoom }

        [SerializeField] CurrentScrenn currentScrenn;
        public UnityEvent SaveGame = new UnityEvent();
        public UnityEvent LoadGame = new UnityEvent();
        public UnityEvent LoadNewScreen = new UnityEvent();

        [SerializeField] List<UI_Datafiles> datafiles = new List<UI_Datafiles> ();

        [SerializeField] UIDocument uIDocument;
        [SerializeField] MainMenuController mainMenuController;
        [SerializeField] HubManager hubManager;

        private void Awake()
        {
            if (uIDocument == null)
            {
                uIDocument = GetComponent<UIDocument>();
            }
        }

        private void Start()
        {
            switch (currentScrenn)
            {
                case CurrentScrenn.MainMenu:
                    MainMenu();
                    break;
                case CurrentScrenn.Hub:
                    Hub();
                    break;
                case CurrentScrenn.BossRoom:
                    BossRoom();
                    break;
            }
        }

        public void MainMenu()
        {
            uIDocument.visualTreeAsset = datafiles.Where(file => file.GetScreen() == CurrentScrenn.MainMenu).First().GetVisualTreeAsset();

            Type[] types = new Type[1] { typeof(MainMenuController) };
            GameObject go = new GameObject("MainMenuController", types);
            go.transform.parent = transform;

            mainMenuController = go.GetComponent<MainMenuController>();
            mainMenuController.Init(uIDocument.rootVisualElement);

            mainMenuController.continuEvent.AddListener(() => LoadGame?.Invoke());
            mainMenuController.startEvent.AddListener(() => SceneManager.LoadScene("Hub"));
        }

        public void Hub()
        {
            Type[] types = new Type[1] { typeof(HubManager) };
            GameObject go = new GameObject("HubManager", types);
            go.transform.parent = transform;

            hubManager = go.GetComponent<HubManager>();
            hubManager.GoblinInteract.AddListener(() => Debug.Log("I'm a goblin"));
        }

        public void BossRoom()
        {

        }
            
        [Serializable]
        public class UI_Datafiles
        {
            [SerializeField] CurrentScrenn CurrentScrenn;
            [SerializeField] VisualTreeAsset treeAsset;

            public VisualTreeAsset GetVisualTreeAsset()
            {
                return treeAsset;
            }

            public CurrentScrenn GetScreen()
            {
                return CurrentScrenn;
            }
        }
    }
}

