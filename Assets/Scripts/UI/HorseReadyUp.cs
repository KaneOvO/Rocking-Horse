using GameSystem.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameUI
{
    public class HorseReadyUp : MonoBehaviour
    {
        public UIController[] Controllers = new UIController[0];
        public GameObject[] HorsePreview = new GameObject[0];
        public GameObject[] ReadyUpObject = new GameObject[0];
        public GameObject[] PlayerIcon = new GameObject[0];

        private uint ReadyAmount = 0;
        public UnityEvent OnAllReadyUp;

        private List<Sprite> originalSprites = new List<Sprite>();

        private void OnEnable()
        {
            ReadyAmount = 0;
            originalSprites.Clear();

            for (int i = 0; i < Controllers.Length; i++)
            {
                var listener = Controllers[i];
                var horse = HorsePreview[i];
                var playericon = PlayerIcon[i];

                if (!listener.IsConnected)
                {
                    ReadyAmount++;
                    horse.SetActive(false);
                    playericon.SetActive(false);
                }
                else
                {
                    horse.SetActive(true);
                    playericon.SetActive(true);
                }
            }

            foreach (GameObject readyup in ReadyUpObject)
            {
                readyup.SetActive(false);
            }
        }

        public void Ready(int index)
        {
            if (ReadyUpObject[index].activeSelf)
            {
                return;
            }

            ReadyUpObject[index].SetActive(true);
            ReadyAmount++;

            if (ReadyAmount >= ReadyUpObject.Length)
            {
                StartCoroutine(WaitForSeconds());
            }
        }

        private IEnumerator WaitForSeconds()
        {
            yield return new WaitForSeconds(1f);

            foreach (GameObject readyUpObj in ReadyUpObject)
            {
                Image image = readyUpObj.GetComponent<Image>();
                Button button = readyUpObj.GetComponent<Button>();

                if (image != null && button != null && button.spriteState.pressedSprite != null)
                {
                    originalSprites.Add(image.sprite); 
                    image.sprite = button.spriteState.pressedSprite;
                }
            }

            OnAllReadyUp?.Invoke();
        }

        public void ResetSprites()
        {
            for (int i = 0; i < ReadyUpObject.Length; i++)
            {
                Image image = ReadyUpObject[i].GetComponent<Image>();
                if (image != null && i < originalSprites.Count)
                {
                    image.sprite = originalSprites[i]; 
                }
            }
        }
    }
}
