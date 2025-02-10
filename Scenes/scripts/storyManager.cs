using System;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset inkJSONAsset = null;
    public Story story;

    [SerializeField]
    private GameObject canvas = null;

    [SerializeField]
    private GameObject panel = null;

    [SerializeField]
    private GameObject textContainer = null;

    // UI Prefabs
    [SerializeField]
    private Text textPrefab = null;

    [SerializeField]
    private Button buttonPrefab = null;

    [SerializeField]
    private InputField inputPrefab = null;

    [SerializeField]
    private GameObject continueButton = null;

    [SerializeField]
    private GameObject backgroundObject = null;

    [SerializeField]
    private GameObject tagsPanel = null;

    [SerializeField]
    private GameObject leftPortrait = null;

    [SerializeField]
    private GameObject centerPortrait = null;

    [SerializeField]
    private GameObject rightPortrait = null;

    [SerializeField]
    private GameObject leftText = null;

    [SerializeField]
    private GameObject centerText = null;

    [SerializeField]
    private GameObject rightText = null;

    [SerializeField]
    private GameObject greenHouse = null;

    [SerializeField]
    private GameObject redHouse = null;

    private bool isChoiceDisplayed = false; // Tracks if choices are already displayed
    private List<Button> activeButtons = new List<Button>(); // List to store active buttons
    private bool isGameplayActive = false;
    private List<string> displayedText = new List<string>();

    void Awake()
    {
        story = new Story(inkJSONAsset.text);

        story.BindExternalFunction(
            "ChangeBackground",
            (string imageName) =>
            {
                Image image = backgroundObject.GetComponent<Image>();
                Sprite newSprite = Resources.Load<Sprite>($"backgrounds/{imageName}");
                image.sprite = newSprite;
            }
        );
        story.BindExternalFunction(
            "ToggleGameplay",
            (bool shouldItStart) =>
            {
                if (shouldItStart)
                {
                    canvas.SetActive(false);
                    tagsPanel.SetActive(false);
                    isGameplayActive = true;
                    continueButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    canvas.SetActive(true);
                    tagsPanel.SetActive(true);
                    isGameplayActive = false;
                    continueButton.GetComponent<Button>().interactable = true;
                }
            }
        );
        story.BindExternalFunction(
            "AskInput",
            (string varName, string placeholder, string continueTag) =>
            {
                isGameplayActive = true;
                continueButton.GetComponent<Button>().interactable = false;
                InputField newInputField = Instantiate(inputPrefab, textContainer.transform);
                newInputField.placeholder.GetComponent<Text>().text = placeholder;
                newInputField.onEndEdit.AddListener(
                    (string userInput) =>
                    {
                        story.variablesState[varName] = userInput;
                        Destroy(newInputField.gameObject);
                        StartStoryFromTag(continueTag);
                    }
                );
            }
        );
        story.BindExternalFunction("Scene_Home", () => Scene_Home());

        story.BindExternalFunction(
            "ChangePortraits",
            (string leftSprite, string centerSprite, string rightSprite) =>
            {
                Image imageLeft = leftPortrait.GetComponent<Image>();
                if (leftSprite == "")
                {
                    imageLeft.sprite = null;
                    leftPortrait.SetActive(false);
                    TMP_Text spriteName = leftText.GetComponent<TMP_Text>();
                    spriteName.text = "";
                }
                else
                {
                    leftPortrait.SetActive(true);
                    Sprite spriteLeft = Resources.Load<Sprite>($"portraits/{leftSprite}");
                    imageLeft.sprite = spriteLeft;
                    TMP_Text spriteName = leftText.GetComponent<TMP_Text>();
                    spriteName.text = leftSprite;
                }

                Image imageCenter = centerPortrait.GetComponent<Image>();
                if (centerSprite == "")
                {
                    imageCenter.sprite = null;
                    centerPortrait.SetActive(false);
                    TMP_Text spriteName = centerText.GetComponent<TMP_Text>();
                    spriteName.text = "";
                }
                else
                {
                    centerPortrait.SetActive(true);
                    Sprite spriteCenter = Resources.Load<Sprite>($"portraits/{centerSprite}");
                    imageCenter.sprite = spriteCenter;
                    TMP_Text spriteName = centerText.GetComponent<TMP_Text>();
                    spriteName.text = centerSprite;
                }

                Image imageRight = rightPortrait.GetComponent<Image>();
                if (rightSprite == "")
                {
                    imageRight.sprite = null;
                    rightPortrait.SetActive(false);
                    TMP_Text spriteName = rightText.GetComponent<TMP_Text>();
                    spriteName.text = "";
                }
                else
                {
                    rightPortrait.SetActive(true);
                    Sprite spriteRight = Resources.Load<Sprite>($"portraits/{rightSprite}");
                    imageRight.sprite = spriteRight;
                    TMP_Text spriteName = rightText.GetComponent<TMP_Text>();
                    spriteName.text = rightSprite;
                }
            }
        );

        string storyText = story.Continue();
        CreateTextObject(storyText);
    }

    public void UpdateClick()
    {
        if (!isGameplayActive)
        {
            // If there is still content to continue, show it.
            if (story.canContinue)
            {
                string storyText = story.Continue();
                CreateTextObject(storyText);
                // Reset the choices display flag when new text is shown
                isChoiceDisplayed = false;
                DestroyChoiceButtons(); // Destroy buttons before showing new choices
            }
            else if (!isChoiceDisplayed && story.currentChoices.Count > 0)
            {
                // Display choices only once
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    CreateChoiceButton(story.currentChoices[i].text, i);
                }
                // Mark that choices have been displayed
                isChoiceDisplayed = true;
            }
            // scroll to the bottom
            ScrollRect scrollRect = panel.GetComponent<ScrollRect>();
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }

    public void StartStoryFromTag(string tagName)
    {
        // This will start the story from the given tag name (e.g., "NPCConversationStart")
        story.ChoosePathString(tagName);
        isGameplayActive = false;
        continueButton.GetComponent<Button>().interactable = true;
        canvas.SetActive(true);
        tagsPanel.SetActive(true);
    }

    public void Scene_Home()
    {
        redHouse.SetActive(true);
        greenHouse.SetActive(true);
    }

    // Create a new Text object for displaying the story
    void CreateTextObject(string text)
    {
        displayedText.Add(text);
        Text newText = Instantiate(textPrefab, textContainer.transform);
        newText.text = text;
        Canvas.ForceUpdateCanvases();
    }

    // Create a new Button for choices
    void CreateChoiceButton(string choiceText, int choiceIndex)
    {
        // Instantiate the new Button
        Button newButton = Instantiate(buttonPrefab, textContainer.transform);
        // Get the Text component of the Button to set its label
        Text buttonText = newButton.GetComponentInChildren<Text>();
        buttonText.text = choiceText;
        // Set up the button click handler to make a choice when clicked
        newButton.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
        // Add the button to the activeButtons list for later removal
        activeButtons.Add(newButton);
        // Force a canvas update to refresh the layout
        Canvas.ForceUpdateCanvases();
    }

    // Handle the player's choice selection
    void OnChoiceSelected(int choiceIndex)
    {
        displayedText.Add(story.currentChoices[choiceIndex].text);
        story.ChooseChoiceIndex(choiceIndex); // Apply the selected choice to the story
        // Reset choice display flag after the player has selected an option
        isChoiceDisplayed = false;
        DestroyChoiceButtons(); // Destroy the buttons after a choice is made
        UpdateClick();
    }

    // Destroy all active choice buttons
    void DestroyChoiceButtons()
    {
        foreach (Button button in activeButtons)
        {
            Destroy(button.gameObject); // Destroy the button GameObject
        }
        activeButtons.Clear(); // Clear the list of active buttons
    }

    public string Save(int slot)
    {
        string savedJson = story.state.ToJson();
        PlayerPrefs.SetString($"storySave_{slot}", savedJson);
        string textHistory = string.Join("|SPLIT|", displayedText); // Use a unique separator
        PlayerPrefs.SetString($"textHistory_{slot}", textHistory);
        // save portraits and background
        VisualSetter visualSetter = GetComponent<VisualSetter>();
        string visualSaveData = visualSetter.GetCurrentVisualConfig();
        PlayerPrefs.SetString($"visualData_{slot}", visualSaveData);
        // save save file identifiers
        string playerNameString = story.variablesState["playerName"].ToString();
        if (playerNameString == "")
        {
            playerNameString = "???";
        }
        PlayerPrefs.SetString($"saveName_{slot}", playerNameString);
        string formattedDate = DateTime.Now.ToString("dd-MM-yyyy, HH:mm");
        PlayerPrefs.SetString($"saveDate_{slot}", formattedDate);
        PlayerPrefs.Save();
        return playerNameString + " | " + formattedDate;
    }

    public void Load(int slot)
    {
        if (PlayerPrefs.HasKey($"storySave_{slot}"))
        {
            string loadedJson = PlayerPrefs.GetString($"storySave_{slot}");
            story.state.LoadJson(loadedJson);

            // Clear previous text UI
            foreach (Transform child in textContainer.transform)
            {
                Destroy(child.gameObject);
            }

            // Load displayed text history
            if (PlayerPrefs.HasKey($"textHistory_{slot}"))
            {
                string textHistory = PlayerPrefs.GetString($"textHistory_{slot}");
                displayedText = new List<string>(
                    textHistory.Split(new string[] { "|SPLIT|" }, StringSplitOptions.None)
                );

                foreach (string text in displayedText)
                {
                    Text newText = Instantiate(textPrefab, textContainer.transform);
                    newText.text = text;
                    Canvas.ForceUpdateCanvases();
                }
            }

            // load portraits and background
            string visualSavaData = PlayerPrefs.GetString($"visualData_{slot}");
            VisualSetter visualSetter = GetComponent<VisualSetter>();
            visualSetter.SetVisualConfig(visualSavaData);

            if (story.currentChoices.Count > 0)
            {
                UpdateClick();
            }
            ScrollRect scrollRect = panel.GetComponent<ScrollRect>();
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
        else
        {
            Debug.Log($"No save data found for slot {slot}.");
        }
    }
}
