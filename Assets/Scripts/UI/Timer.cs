using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class Timer : MonoBehaviour
    {
        private List<TextMeshProUGUI> startTimeTexts = new List<TextMeshProUGUI>();

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip countdownStartSFX;
        [SerializeField] private AudioSource musicSource;       
        [SerializeField] private AudioClip mainTrack;

        private void Start()
        {
            GameObject uis = GameObject.Find("UIs");
            if (uis == null) return;

            for (int i = 1; i <= 4; i++)
            {
                Transform ui = uis.transform.Find("UI" + i);
                if (ui != null)
                {
                    Transform startTime = ui.Find("StartTime");
                    if (startTime != null)
                    {
                        TextMeshProUGUI text = startTime.GetComponent<TextMeshProUGUI>();
                        if (text != null)
                        {
                            text.enabled = false;
                            startTimeTexts.Add(text);
                        }
                    }
                }
            }

            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            yield return new WaitForSeconds(2.5f); // Initial delay

            if (audioSource != null && countdownStartSFX != null)
            {
                audioSource.PlayOneShot(countdownStartSFX);
            }

            GameManager.TimeBeforeStart = 3;
            ShowTextOnAll("3");
            yield return new WaitForSeconds(1f);

            GameManager.TimeBeforeStart = 2;
            ShowTextOnAll("2");
            yield return new WaitForSeconds(1f);

            GameManager.TimeBeforeStart = 1;
            ShowTextOnAll("1");
            yield return new WaitForSeconds(1f);

            GameManager.TimeBeforeStart = 0;
            ShowTextOnAll("GO!");
            if (musicSource != null && mainTrack != null)
            {
                musicSource.clip = mainTrack;
                musicSource.loop = true;
                musicSource.Play();
            }
            GameManager.Instance.StartGame();
            GameManager.GameStartEvent?.Invoke();
            yield return new WaitForSeconds(0.5f);

            HideAllText();
        }

        private void ShowTextOnAll(string content)
        {
            foreach (var text in startTimeTexts)
            {
                text.text = content;
                text.enabled = true;
            }
        }

        private void HideAllText()
        {
            foreach (var text in startTimeTexts)
            {
                text.enabled = false;
            }
        }
    }
}
