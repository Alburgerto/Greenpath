using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController m_fpsController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_fpsController.enabled = false;
    }
    
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_fpsController.enabled = true;
        gameObject.SetActive(false);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        m_fpsController.enabled = true;
        SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
