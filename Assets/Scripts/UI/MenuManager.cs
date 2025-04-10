using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject joinPanel;
    public GameObject tutorialPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    private void Start()
    {
        // Only show the main menu at the start
        mainMenuPanel.SetActive(true);
        joinPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void OpenJoin()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void StartRace()
    {
        SceneManager.LoadScene("RaceScene"); // Replace "RaceScene" with your actual scene name
    }

    public void OpenTutorial()
    {
        joinPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void BackToJoin()
    {
        joinPanel.SetActive(true);
        tutorialPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void BackToMenu()
    {
        joinPanel.SetActive(false);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
