using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private UIDocument doc;
    private Button startButton;
    private Button continueButton;
    private Button exitButton;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        startButton = doc.rootVisualElement.Q<Button>("Start");
        continueButton = doc.rootVisualElement.Q<Button>("Continue");
        exitButton = doc.rootVisualElement.Q<Button>("Exit");
        startButton.clicked += StartButtonOnClicked;
        continueButton.clicked += ContinueButtonOnClicked;
        exitButton.clicked += ExitButtonOnClicked;
    }

    private void StartButtonOnClicked()
    {
        SceneManager.LoadScene("Hub");
    }

    private void ContinueButtonOnClicked()
    {
        Debug.Log("I'm doing something useful");
    }

    private void ExitButtonOnClicked()
    {
        Application.Quit();
    }
}
