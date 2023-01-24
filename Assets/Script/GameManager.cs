using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;
using Boss.save;

public class GameManager : MonoBehaviour
{
   [SerializeField] UI_Manager ui_manager;
   [SerializeField] SaveManager saveManager;

    private void Start()
    {
        ui_manager.SaveGame.AddListener(() => saveManager.SaveGame());
        ui_manager.LoadGame.AddListener(() => saveManager.LoadGame());
    }
}
