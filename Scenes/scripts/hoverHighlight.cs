using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text buttonText;
    public Color hoverColor = Color.red;
    private Color originalColor;

    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            originalColor = buttonText.color;
        }
        else
        {
            Debug.LogError("No Text component found on the button's child!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = originalColor;
        }
    }
}
