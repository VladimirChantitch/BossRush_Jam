using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;
using Boss.save;
using player;
using Boss.inventory;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum CurrentScrenn { MainMenu, Hub, BossRoom }

    [SerializeField] UI_Manager ui_manager;
    [SerializeField] SaveManager saveManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] BossCharacter bossCharacter;
    [SerializeField] CurrentScrenn currentScrenn;

    [Header("Data")]
    List<Recipies> recipies = new List<Recipies>();

    public bool Save;
    public bool Load;
    [SerializeField] bool AutoLoad;

    [SerializeField] CameraJuice cameraJuice;
    [SerializeField] Explosion explosion;


    private void Start()
    {
        SetUIManagerEvents();
        if (currentScrenn == CurrentScrenn.BossRoom)
        {
            SetBossCharacterEvent();
        }
        SetPlayerManagerEvents();

        if (AutoLoad)
        {
            saveManager.LoadGame();
        }
    }

    private void SetUIManagerEvents()
    {
        ui_manager.SetCurrentScreen(currentScrenn);
        ui_manager.Init(recipies);

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

        ui_manager.onGoToHub.AddListener(() =>
        {
            saveManager.SaveGame();
            SceneManager.LoadScene("Hub");
        });
    }

    private void SetBossCharacterEvent()
    {
        bossCharacter.onBossDying.AddListener(() =>
        {
            explosion.Explode();
            cameraJuice.Shake(35f, 0.55f);
        });

        bossCharacter.onBossDead.AddListener(data =>
        {
            Debug.Log("GameManger");
            ui_manager.ShowLoots(data);
            playerManager.AddToInventory(data.guitareUpgrades, data.bossItems);
        });

        bossCharacter.onBossHit.AddListener(() =>
        {
            Debug.Log("HIT");
            cameraJuice.Shake(5f, 0.1f);
        });
    }

    private void SetPlayerManagerEvents()
    {
        playerManager.onPlayerDead.AddListener(() => ui_manager.PlayerLoose());
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
