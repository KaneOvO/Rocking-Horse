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

    public static bool InputLocked = false;

    private void Start()
    {
        GameManager.Instance.CleanPlayers();

        // TEMP: Reset all preferences every time the game starts (for testing)
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();

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

        if (PlayerPrefs.HasKey("SkipTutorial"))
        {
            bool savedSkipTutorial = PlayerPrefs.GetInt("SkipTutorial") == 1;
            tutorialToggle.isOn = savedSkipTutorial;
        }
    }

    private void OnEnable()
    {
        UpdatePlayerCount();
    }

    private void Update()
    {
        if (InputLocked) return; 

        if (Input.GetKeyDown(KeyCode.M))
        {
            StartRace();
        }
    }

    public void OpenJoin()
    {
        if (InputLocked) return;

        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void StartRace()
    {
        if (InputLocked) return;

        SceneManager.LoadScene("MainGameScene");
    }

    public void OpenTutorial()
    {
        if (InputLocked) return;

        bool skipTutorial = PlayerPrefs.GetInt("SkipTutorial", 0) == 1;

        if (skipTutorial)
        {
            StartRace();
            return;
        }

        joinPanel.SetActive(false);

        if (!hasPlayedCountdown && countdownObject != null && countdownVideoPlayer != null)
        {
            StartCoroutine(HandleCountdownAndShowTutorial());
            hasPlayedCountdown = true;
        }
        else
        {
            tutorialPanel.SetActive(true);
        }
    }


    private IEnumerator HandleCountdownAndShowTutorial()
    {
        InputLocked = true;

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

        tutorialPanel.SetActive(true); 

        InputLocked = false;
    }


    public void OnVolumeChanged(float value)
    {
        if (menuMusic != null)
            menuMusic.volume = value;

        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    public void OnPlayerSliderPlayerCountChanged(float value)
    {
        if (InputLocked) return;

        playerCountText.text = value.ToString("0");
        if (GameManager.Instance.PlayerCount != 0)
        {
            GameManager.Instance.PlayerCount = (int)value;
        }
    }

    public void OnTutorialToggleChanged(bool isOn)
    {
        if (InputLocked) return;

        PlayerPrefs.SetInt("SkipTutorial", isOn ? 1 : 0);
    }

    public void BackToJoin()
    {
        if (InputLocked) return;

        joinPanel.SetActive(true);
        tutorialPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (InputLocked) return;

        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        if (InputLocked) return;

        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        if (InputLocked) return;

        joinPanel.SetActive(false);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        if (InputLocked) return;

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
