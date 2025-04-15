using System.Collections;
using UnityEngine;

namespace GameUI
{
    public class Timer : MonoBehaviour
    {
        [Header("Countdown Assets")]
        [SerializeField] private GameObject countdown3;
        [SerializeField] private GameObject countdown2;
        [SerializeField] private GameObject countdown1;
        [SerializeField] private GameObject giddyUp;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip countdownStartSFX;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip mainTrack;

        public void BeginCountdown()
        {
            HideAllCountdowns();
            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            yield return new WaitForSeconds(0.5f); // Short delay after intro

            if (audioSource != null && countdownStartSFX != null)
            {
                audioSource.PlayOneShot(countdownStartSFX);
            }

            GameManager.TimeBeforeStart = 3;
            countdown3.SetActive(true);
            yield return new WaitForSeconds(1f);

            countdown3.SetActive(false);
            GameManager.TimeBeforeStart = 2;
            countdown2.SetActive(true);
            yield return new WaitForSeconds(1f);

            countdown2.SetActive(false);
            GameManager.TimeBeforeStart = 1;
            countdown1.SetActive(true);
            yield return new WaitForSeconds(1f);

            countdown1.SetActive(false);
            GameManager.TimeBeforeStart = 0;
            giddyUp.SetActive(true);

            if (musicSource != null && mainTrack != null)
            {
                musicSource.clip = mainTrack;
                musicSource.loop = true;
                musicSource.Play();
            }

            GameManager.Instance.StartGame();
            GameManager.GameStartEvent?.Invoke();

            yield return new WaitForSeconds(0.5f);
            giddyUp.SetActive(false);
        }

        private void HideAllCountdowns()
        {
            countdown3.SetActive(false);
            countdown2.SetActive(false);
            countdown1.SetActive(false);
            giddyUp.SetActive(false);
        }
    }
}
