using UnityEngine;
using UnityEngine.UI;

public class RebindMenuButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        if (GameManager.Instance != null)
        {
            button.onClick.AddListener(() => GameManager.Instance.GoToMenu());
        }
        else
        {
            Debug.LogWarning("GameManager.Instance is null — can't bind GoToMenu");
        }
    }
}
