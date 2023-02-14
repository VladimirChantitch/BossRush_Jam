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

    VisualElement playerDeathScreen;
    VisualElement endScreen;

    VisualElement bossCraftableParent = null;
    VisualElement guitareUpgradeParent = null;

    Label congrats;

    StyleSheet style;

    FightUI fightUI;

    [HideInInspector] public UnityEvent onBackToHub = new UnityEvent();


    internal void Init(VisualElement rootVisualElement)
    {
        root = rootVisualElement;

        BindUI();
    }

    private void BindUI()
    {
        fightUI = FindObjectOfType<FightUI>();

        style = DADDY.Instance.USS_STYLE;

        //fightScreen = root.Q<VisualElement>("FightScreen");
        playerDeathScreen = root.Q<VisualElement>("PlayerDeathScreen");
        endScreen = root.Q<VisualElement>("EndScreenLoot");
        bossCraftableParent = endScreen.Q<VisualElement>("BossLoots");
        guitareUpgradeParent = endScreen.Q<VisualElement>("PlayerLoots");

        playerDeathScreen.visible = false;
        endScreen.visible = false;

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
        await Task.Delay(1000);
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
        AudioManager.Instance.TransitionMusic(PlusMusic_DJ.PMTags.victory, 1);
        fightUI.Activate(false);
    }

    public void OpenDeathScreen()
    {
        playerDeathScreen.visible = true;
        AudioManager.Instance.TransitionMusic(PlusMusic_DJ.PMTags.failure, 1);
        fightUI.Activate(false);
    }
}
