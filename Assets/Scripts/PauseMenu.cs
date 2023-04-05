using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioSource audio;
    private float defaultPitch;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        defaultPitch = audio.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            audio.pitch = 0;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            audio.pitch = defaultPitch;
        }
    }

    public void Restart(string sceneName)
    {
        //PauseGame();
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
