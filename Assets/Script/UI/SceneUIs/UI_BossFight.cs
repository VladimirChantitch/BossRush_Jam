using Boss.loot;
using Boss.stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UI_BossFight : MonoBehaviour
{
    VisualElement root;
    VisualElement fightScreen;
    VisualElement playerDeathScreen;
    VisualElement endScreen;

    ProgressBar playerHealth;
    ProgressBar bossHealth;

    VisualElement bossCraftableParent = null;
    VisualElement guitareUpgradeParent = null;

    Label congrats;

    StyleSheet style;

    [HideInInspector] public UnityEvent onBackToHub = new UnityEvent();


    internal void Init(VisualElement rootVisualElement)
    {
        root = rootVisualElement;

        BindUI();
    }

    private void BindUI()
    {
        style = (StyleSheet)AssetDatabase.LoadAssetAtPath("Assets/UI/Boss.uss",typeof(StyleSheet));

        fightScreen = root.Q<VisualElement>("FightScreen");
        playerDeathScreen = root.Q<VisualElement>("PlayerDeathScreen");
        endScreen = root.Q<VisualElement>("EndScreenLoot");
        bossCraftableParent = endScreen.Q<VisualElement>("BossLoots");
        guitareUpgradeParent = endScreen.Q<VisualElement>("PlayerLoots");

        playerDeathScreen.visible = false;
        endScreen.visible = false;
        fightScreen.visible = true;

        playerHealth = fightScreen.Q<VisualElement>("PlayerHP").Q<ProgressBar>();
        bossHealth = fightScreen.Q<VisualElement>("BossHP").Q<ProgressBar>();

        // To Do --- player dash
        // To Do --- pulse bar

        congrats = endScreen.Q<Label>("Congratulation");

        ///Set up eventss
        endScreen.Q<Button>("GetBackToHub").clicked += () =>
        {
            onBackToHub?.Invoke();
        };

        playerDeathScreen.Q<Button>("GetBackToHub").clicked += () =>
        {
            onBackToHub?.Invoke();
        };
    }

    public void UpdatePlayerLife(Stat_DTO stat_DTO)
    {
        throw new NotImplementedException();
    } 

    public void UpdateBossLife(Stat_DTO stat_DTO)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Show the loots on the end screen if the boss has been beaten 
    /// </summary>
    /// <param name="bossLootData"></param>
    public void ShowLoots(BossLootData bossLootData)
    {
        Debug.Log("ui_bossfight");
        bossLootData.guitareUpgrades.ForEach(gu =>
        {
            GenerateLoot(gu.Icon.texture, true);
        });

        bossLootData.bossItems.ForEach(bi =>
        {
            GenerateLoot(bi.Icon.texture, false);
        });
    }

    private async void GenerateLoot(Texture texture, bool isUpgrade)
    {
        OpenWinScreen();

        await Task.Delay(1000);
        //TO DO -- fondu

        VisualElement ve = new VisualElement();
        ve.styleSheets.Add(style);
        ve.AddToClassList("BigSlot");
        Image image = new Image();
        image.image = texture;
        ve.Add(image);

        if (isUpgrade)
        {
            guitareUpgradeParent.Add(ve);
        }
        else
        {
            bossCraftableParent.Add(ve);
        }

    }

    private void OpenWinScreen()
    {
        endScreen.visible = true;
        fightScreen.visible = false;
        fightScreen.Children().ToList().ForEach(c => c.visible = false);
        bossHealth.visible = false;
        playerHealth.visible = false;   
    }

    public void OpenDeathScreen()
    {
        playerDeathScreen.visible = true;
        fightScreen.visible = false;
        fightScreen.Children().ToList().ForEach(c => c.visible = false);
        bossHealth.visible = false;
        playerHealth.visible = false;
    }
}
