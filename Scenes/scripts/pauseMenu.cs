using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject pausePanel;
    public GameObject savePanel;
    public GameObject loadPanel;
    public GameObject overwritePanel;
    public List<GameObject> saveSlots = new List<GameObject>();
    public List<GameObject> loadSlots = new List<GameObject>();
    public GameObject storyManager;
    private bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        loadPanel.SetActive(false);
        savePanel.SetActive(false);
        pausePanel.SetActive(true);
        if (!paused)
        {
            pauseCanvas.SetActive(true);
            paused = true;
        }
        else
        {
            pauseCanvas.SetActive(false);
            paused = false;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToSaveSlots()
    {
        // Ensure StoryManager script is fetched once
        StoryManager smScript = storyManager.GetComponent<StoryManager>();

        for (int i = 0; i < saveSlots.Count; i++)
        {
            // Retrieve save slot data from PlayerPrefs
            string slotName = PlayerPrefs.GetString($"saveName_{i}", "");
            string slotDate = PlayerPrefs.GetString($"saveDate_{i}", "");
            Debug.Log("slot " + i + " - " + slotName + " | " + slotDate);
            Text saveText = saveSlots[i].GetComponentInChildren<Text>();
            Button saveButton = saveSlots[i].GetComponentInChildren<Button>();
            // Clear previous listeners to avoid duplicate calls
            saveButton.onClick.RemoveAllListeners();
            if (!string.IsNullOrEmpty(slotName) && !string.IsNullOrEmpty(slotDate))
            {
                saveText.text = slotName + " | " + slotDate;
                int slotIndex = i; // Store in a local variable to avoid closure issues
                // This brings up the overwrite panel
                saveButton.onClick.AddListener(() =>
                {
                    Transform yesPanel = overwritePanel.transform.Find("Panel/Overwrite_yes");
                    Button yesButton = yesPanel.GetComponent<Button>();
                    Transform noPanel = overwritePanel.transform.Find("Panel/Overwrite_no");
                    Button noButton = noPanel.GetComponent<Button>();
                });
            }
            else
            {
                saveText.text = "Empty...";
                int slotIndex = i; // Store in a local variable to avoid closure issues
                saveButton.onClick.AddListener(() =>
                {
                    string newSave = smScript.Save(slotIndex);
                    saveText.text = newSave;
                });
            }
        }
        pausePanel.SetActive(false);
        savePanel.SetActive(true);
    }

    public void GoToLoadSlots()
    {
        // Ensure StoryManager script is fetched once
        StoryManager smScript = storyManager.GetComponent<StoryManager>();

        for (int i = 0; i < loadSlots.Count; i++)
        {
            // Retrieve save slot data from PlayerPrefs
            string slotName = PlayerPrefs.GetString($"saveName_{i}", "");
            string slotDate = PlayerPrefs.GetString($"saveDate_{i}", "");
            Debug.Log("slot " + i + " - " + slotName + " | " + slotDate);
            Text saveText = loadSlots[i].GetComponentInChildren<Text>();
            Button saveButton = loadSlots[i].GetComponentInChildren<Button>();
            // Clear previous listeners to avoid duplicate calls
            saveButton.onClick.RemoveAllListeners();
            if (!string.IsNullOrEmpty(slotName) && !string.IsNullOrEmpty(slotDate))
            {
                saveText.text = slotName + " | " + slotDate;
            }
            else
            {
                saveText.text = "Empty...";
            }
            int slotIndex = i; // Store in a local variable to avoid closure issues
            saveButton.onClick.AddListener(async () =>
            {
                smScript.Load(slotIndex);
                TogglePause();
            });
        }
        pausePanel.SetActive(false);
        savePanel.SetActive(true);
    }

    public void ReturnToPausePanel()
    {
        loadPanel.SetActive(false);
        savePanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
