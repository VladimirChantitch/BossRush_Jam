using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;
using Boss.save;
using player;
using Boss.inventory;
using System;
using UnityEngine.SceneManagement;
using Boss.Map;

public class GameManager : MonoBehaviour
{
    public enum CurrentScrenn { MainMenu, Hub, BossRoom }

    [SerializeField] UI_Manager ui_manager;
    [SerializeField] SaveManager saveManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] BossCharacter bossCharacter;
    [SerializeField] CurrentScrenn currentScrenn;
    [SerializeField] MapManager mapManager;

    [Header("Data")]
    [SerializeField] List<Recipies> recipies = new List<Recipies>();

    public bool Save;
    public bool Load;
    [SerializeField] bool AutoLoad;

    [SerializeField] CameraJuice cameraJuice;
    [SerializeField] Explosion explosion;
    DamageEffect damageEffect;

    private void Start()
    {
        SetUIManagerEvents();
        if (currentScrenn == CurrentScrenn.BossRoom)
        {
            SetBossCharacterEvent();
        }
        SetPlayerManagerEvents();
        if (currentScrenn == CurrentScrenn.Hub)
        {
            SetMapEvents();
        }

        if (AutoLoad)
        {
            saveManager.LoadGame();
        }

        damageEffect = GetComponent<DamageEffect>();
    }

    private void SetUIManagerEvents()
    {
        ui_manager.SetCurrentScreen(currentScrenn);
        ui_manager.Init(recipies);

        ui_manager.SaveGame.AddListener(() => saveManager.SaveGame());
        ui_manager.LoadGame.AddListener(() => saveManager.LoadGame());
        ui_manager.DeleteSaveFile.AddListener(() => saveManager.DestroySaveFile());
        ui_manager.AskForInventory.AddListener(action =>
        {
            List<AbstractItem> items = playerManager.GetItems();
            action.Invoke(items);
        });
        ui_manager.AskForUpgrades.AddListener(action =>
        {
            List <GuitareUpgrade> items = playerManager.GetGuitareUpgrades();
            action.Invoke(items);
        });


        ui_manager.CrafterSuccess.AddListener(sucess_DTO =>
        {
            playerManager.RemoveFromInventory(sucess_DTO.item_1);
            playerManager.RemoveFromInventory(sucess_DTO.item_2);
            mapManager.UnlockNewLocation(sucess_DTO.resutl as BossSacrificeable);
        });
        ui_manager.onItemSetAsUpgrade.AddListener(item =>
        {
            playerManager.AddOrModifyUpgrade(item as GuitareUpgrade);
            playerManager.RemoveFromInventory(item);
        });
        ui_manager.onRemoveUpgrade.AddListener(upgrade => {
            playerManager.RemoveUpgrade(upgrade as GuitareUpgrade);
            playerManager.AddToInventory(upgrade);
        });

        ui_manager.onGoToHub.AddListener(() =>
        {
            saveManager.SaveGame();
            FindObjectOfType<PlayerMovement>().Dispose();
            SceneManager.LoadScene("Hub");
        });

        ui_manager.onRequestUseBlood.AddListener(action => playerManager.UseBlood(action));
    }

    private void SetBossCharacterEvent()
    {
        bossCharacter.onBossDying.AddListener(data =>
        {
            explosion.Explode();
            cameraJuice.Shake(35f, 0.55f);
            playerManager.ReceiveDialogueData(data);
        });

        bossCharacter.onBossDead.AddListener(data =>
        {
            ui_manager.ShowLoots(data);
            playerManager.AddToInventory(data.guitareUpgrades, data.bossItems);
            playerManager.SetStat(false, playerManager.GetStat(Boss.stats.StatsType.Blood).Value + bossCharacter.GetStat(Boss.stats.StatsType.Blood).Value, Boss.stats.StatsType.Blood);
        });

        bossCharacter.onBossHit.AddListener(() =>
        {
            cameraJuice.Shake(5f, 0.1f);
            damageEffect?.Blinking(); 
        });
    }

    private void SetPlayerManagerEvents()
    {
        playerManager.onPlayerDead.AddListener(() => {
            ui_manager.PlayerLoose();
            playerManager.ReceiveDialogueData(bossCharacter.bossRelatedDialogues);
        });
        playerManager.onJustCameBack.AddListener((dialogues, b) =>
        {
            if (currentScrenn == CurrentScrenn.Hub)
            {
                if (b)
                {
                    ui_manager.SendDialogue(dialogues?.LooseDialogue?.dialogue);
                }
                else
                {
                    ui_manager.SendDialogue(dialogues?.WinDialogue?.dialogue);
                }
            }
        });
    }

    private void SetMapEvents()
    {
        mapManager.onGoToBossFight.AddListener(s => {
            saveManager.SaveGame();
            FindObjectOfType<DoorManager>().LaunchNewBossFight(s);
        });
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
