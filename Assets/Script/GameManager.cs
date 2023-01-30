using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;
using Boss.save;
using player;
using Boss.inventory;

public class GameManager : MonoBehaviour
{
    public enum CurrentScrenn { MainMenu, Hub, BossRoom }

    [SerializeField] UI_Manager ui_manager;
    [SerializeField] SaveManager saveManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] CurrentScrenn currentScrenn;

    public bool Save;
    public bool Load;
    [SerializeField] bool AutoLoad;

    private void Start()
    {
        ui_manager.SetCurrentScreen(currentScrenn);
        ui_manager.Init();

        ui_manager.SaveGame.AddListener(() => saveManager.SaveGame());
        ui_manager.LoadGame.AddListener(() => saveManager.LoadGame());
        ui_manager.DeleteSaveFile.AddListener(() => saveManager.DestroySaveFile());
        ui_manager.AskOfrInventory.AddListener(action =>
        {
            List<AbstractItem> items = playerManager.GetItems();
            action.Invoke(items);
        });

        ui_manager.CrafterSuccess.AddListener(sucess_DTO =>
        {
            playerManager.RemoveFromInventory(sucess_DTO.item_1);
            playerManager.RemoveFromInventory(sucess_DTO.item_2);
            playerManager.AddToInventory(sucess_DTO.resutl);
        });

        if (AutoLoad)
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
