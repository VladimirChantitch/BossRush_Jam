using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;
using Boss.save;

public class GameManager : MonoBehaviour
{
    public enum CurrentScrenn { MainMenu, Hub, BossRoom }

    [SerializeField] UI_Manager ui_manager;
    [SerializeField] SaveManager saveManager;
    [SerializeField] CurrentScrenn currentScrenn;

    public bool Save;
    public bool Load;

    private void Start()
    {
        ui_manager.SetCurrentScreen(currentScrenn);

        ui_manager.SaveGame.AddListener(() => saveManager.SaveGame());
        ui_manager.LoadGame.AddListener(() => saveManager.LoadGame());
        ui_manager.DeleteSaveFile.AddListener(() => saveManager.DestroySaveFile());

        if (currentScrenn == CurrentScrenn.MainMenu)
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
