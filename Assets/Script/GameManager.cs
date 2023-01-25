using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;
using Boss.save;

public class GameManager : MonoBehaviour
{
   [SerializeField] UI_Manager ui_manager;
   [SerializeField] SaveManager saveManager;
    UI_Manager.CurrentScrenn currentScrenn;

    public bool Save;
    public bool Load;

    private void Start()
    {
        ui_manager.SaveGame.AddListener(() => saveManager.SaveGame());
        ui_manager.LoadGame.AddListener(() => saveManager.LoadGame());
        ui_manager.DeleteSaveFile.AddListener(() => saveManager.DestroySaveFile());

        currentScrenn = ui_manager.GetCurrentScreen();

        if (currentScrenn == UI_Manager.CurrentScrenn.MainMenu)
        {
            saveManager.LoadGame();
        }
    }

    private void Update()
    {
        if (Save)
        {
            saveManager.SaveGame();
        }

        if (Load)
        {
            saveManager.LoadGame();
        }
    }

    private void LateUpdate()
    {
        Save = false;
        Load = false;
    }
}
