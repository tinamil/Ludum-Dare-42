using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject popup;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (popup != null)
            {
                popup.SetActive(!popup.activeSelf);
            }
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            MainMenu();
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
