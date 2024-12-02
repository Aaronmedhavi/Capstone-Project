using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainPanel;
    public void PlayGame(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
