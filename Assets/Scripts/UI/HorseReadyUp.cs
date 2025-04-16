using GameSystem.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace GameUI
{
    public class HorseReadyUp : MonoBehaviour
    {
        public UIController[] Controllers = new UIController[0];

        public GameObject[] HorsePreview = new GameObject[0];
        public GameObject[] ReadyUpObject = new GameObject[0];

        private uint ReadyAmount = 0;

        public UnityEvent OnAllReadyUp;

        private void OnEnable()
        {
            ReadyAmount = 0;
            for(int i = 0; i < Controllers.Length; i++)
            {
                var listener = Controllers[i];
                var horse = HorsePreview[i];
                if (!listener.IsConnected)
                {
                    ReadyAmount++;
                    horse.SetActive(false);
                }
                else
                {
                    horse.SetActive(true);
                }
            }
            foreach(GameObject readyup in ReadyUpObject)
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
            OnAllReadyUp?.Invoke();
        }

        
    }

}
