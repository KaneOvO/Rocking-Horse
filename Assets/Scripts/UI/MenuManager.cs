using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject joinPanel;
    public GameObject tutorialPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Video Countdown")]
    public GameObject countdownObject;
    public VideoPlayer countdownVideoPlayer;

    [Header("Audio")]
    public AudioSource menuMusic;
    public Slider volumeSlider;

    [Header("Player Count")]
    public Slider playerCountSlider;
    public TextMeshProUGUI playerCountText;

    [Header("Settings")]
    public Toggle tutorialToggle;

    private bool hasPlayedCountdown = false;

    private void Start()
    {
        GameManager.Instance.CleanPlayers();

        // TEMP: Reset all preferences every time the game starts (for testing)
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Show only main menu at launch
        mainMenuPanel.SetActive(true);
        joinPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        // Load and apply saved volume
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        volumeSlider.value = savedVolume;
        if (menuMusic != null)
            menuMusic.volume = savedVolume;
        
        playerCountSlider.value = GameManager.Instance.PlayerCount;
        playerCountText.text = GameManager.Instance.PlayerCount.ToString();

        // Only apply saved tutorial toggle if it's been saved before
        if (PlayerPrefs.HasKey("SkipTutorial"))
        {
            bool savedSkipTutorial = PlayerPrefs.GetInt("SkipTutorial") == 1;
            tutorialToggle.isOn = savedSkipTutorial;
        }
    }

    public void OnEnable()
    {
        UpdatePlayerCount();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartRace();
        }
    }

    public void OpenJoin()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void StartRace()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OpenTutorial()
    {
        bool skipTutorial = PlayerPrefs.GetInt("SkipTutorial", 0) == 1;

        if (skipTutorial)
        {
            StartRace();
            return;
        }

        joinPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        if (!hasPlayedCountdown && countdownObject != null && countdownVideoPlayer != null)
        {
            countdownObject.SetActive(true);
            countdownVideoPlayer.Play();
            StartCoroutine(HandleCountdown());
            hasPlayedCountdown = true;
        }
    }

    private IEnumerator HandleCountdown()
    {
        if (menuMusic != null)
            menuMusic.volume = 0f;

        if (countdownObject != null)
            countdownObject.SetActive(true);

        if (countdownVideoPlayer != null)
            countdownVideoPlayer.Play();

        yield return new WaitForSeconds(6f);

        if (menuMusic != null)
            menuMusic.volume = 1f;

        if (countdownObject != null)
            Destroy(countdownObject);
    }

    public void OnVolumeChanged(float value)
    {
        if (menuMusic != null)
            menuMusic.volume = value;

        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save(); // optional but good to be safe
    }

    public void OnPlayerSliderPlayerCountChanged(float value)
    {
        playerCountText.text = value.ToString("0");
        GameManager.Instance.PlayerCount = (int)value;
    }


    public void OnTutorialToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("SkipTutorial", isOn ? 1 : 0);
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

    public void BackToMenu()
    {
        joinPanel.SetActive(false);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void UpdatePlayerCount()
    {
        playerCountSlider.minValue = 1;
        playerCountSlider.maxValue = GameManager.Instance.deviceCount > 0 ? GameManager.Instance.deviceCount : 1;
        playerCountSlider.value = GameManager.Instance.PlayerCount;
        playerCountText.text = GameManager.Instance.PlayerCount.ToString();

    }


}
