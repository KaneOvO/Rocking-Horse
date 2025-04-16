using UnityEngine;
using UnityEngine.UI;

public class DynamicPlayerNumber : MonoBehaviour
{
    [Header("Color Variants")]
    public Sprite redIcon;
    public Sprite blueIcon;
    public Sprite greenIcon;
    public Sprite yellowIcon;

    [Header("Target")]
    public Image iconImage;

    public void SetColor(PlayerColor color)
    {
        switch (color)
        {
            case PlayerColor.Red:
                iconImage.sprite = redIcon;
                break;
            case PlayerColor.blue:
                iconImage.sprite = blueIcon;
                break;
            case PlayerColor.green:
                iconImage.sprite = greenIcon;
                break;
            case PlayerColor.yellow:
                iconImage.sprite = yellowIcon;
                break;
        }
    }
}
