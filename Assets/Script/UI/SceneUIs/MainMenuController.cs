using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    private VisualElement root;
    private Button startButton;
    private Button continueButton;
    private Button exitButton;

    public UnityEvent continuEvent = new UnityEvent();
    public UnityEvent startEvent = new UnityEvent();

    public void Init(VisualElement root)
    {
        this.root = root; 
        BindButons();
    }

    private void BindButons()
    {
        startButton = root.Q<Button>("Start");
        continueButton = root.Q<Button>("Continue");
        exitButton = root.Q<Button>("Exit");

        startButton.clicked += StartButtonOnClicked;
        continueButton.clicked += ContinueButtonOnClicked;
        exitButton.clicked += ExitButtonOnClicked;
    }

    #region Bindings
    private void StartButtonOnClicked()
    {
        startEvent?.Invoke();
    }

    private void ContinueButtonOnClicked()
    {
        continuEvent?.Invoke();
    }

    private void ExitButtonOnClicked()
    {
        Application.Quit();
    }
    #endregion
}
