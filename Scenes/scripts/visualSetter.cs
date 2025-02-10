using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VisualSetter : MonoBehaviour
{
    // Assign these in the Inspector
    public GameObject backgroundObject;
    public GameObject leftPortrait,
        centerPortrait,
        rightPortrait;
    public GameObject leftText,
        centerText,
        rightText;

    /// <summary>
    /// Sets the visuals based on a config string with this format:
    /// "Background: {backgroundName}\nLeft: {leftPortraitName}\nCenter: {centerPortraitName}\nRight: {rightPortraitName}"
    /// </summary>
    public void SetVisualConfig(string config)
    {
        // Split the config string into lines
        string[] lines = config.Split('\n');
        if (lines.Length < 4)
        {
            Debug.LogError("Invalid config string.");
            return;
        }

        // Extract names by removing the key and trimming whitespace.
        string bgName = lines[0].Replace("Background:", "").Trim();
        string leftName = lines[1].Replace("Left:", "").Trim();
        string centerName = lines[2].Replace("Center:", "").Trim();
        string rightName = lines[3].Replace("Right:", "").Trim();

        // Set the background image.
        Image bgImage = backgroundObject.GetComponent<Image>();
        bgImage.sprite = string.IsNullOrEmpty(bgName)
            ? null
            : Resources.Load<Sprite>($"backgrounds/{bgName}");

        // Set each portrait using a helper method.
        SetPortrait(leftPortrait, leftText, leftName);
        SetPortrait(centerPortrait, centerText, centerName);
        SetPortrait(rightPortrait, rightText, rightName);
    }

    // Helper method to set a portrait and its label.
    private void SetPortrait(GameObject portraitObj, GameObject textObj, string spriteName)
    {
        Image portraitImage = portraitObj.GetComponent<Image>();
        TMP_Text portraitText = textObj.GetComponent<TMP_Text>();

        if (string.IsNullOrEmpty(spriteName))
        {
            portraitImage.sprite = null;
            portraitObj.SetActive(false);
            portraitText.text = "";
        }
        else
        {
            portraitObj.SetActive(true);
            portraitImage.sprite = Resources.Load<Sprite>($"portraits/{spriteName}");
            portraitText.text = spriteName;
        }
    }

    public string GetCurrentVisualConfig()
    {
        // Get the current background sprite's name (if set)
        Image bgImage = backgroundObject.GetComponent<Image>();
        // Use null-conditional operator in case sprite is null
        string backgroundName = bgImage.sprite?.name ?? "";

        // Get the portrait configuration from the text components
        string leftPortraitName = leftText.GetComponent<TMP_Text>().text;
        string centerPortraitName = centerText.GetComponent<TMP_Text>().text;
        string rightPortraitName = rightText.GetComponent<TMP_Text>().text;

        // Combine them into a single string (adjust formatting as desired)
        return $"Background: {backgroundName}\nLeft: {leftPortraitName}\nCenter: {centerPortraitName}\nRight: {rightPortraitName}";
    }
}
