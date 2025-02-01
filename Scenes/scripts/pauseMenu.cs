using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    private bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
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
}
