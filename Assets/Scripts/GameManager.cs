using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instace;
    private Scene currentScene;
    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        SetLevelMusic(currentScene.name);
    }

    public void ChangeScene(String name)
    {
        SetLevelMusic(name);
        SceneManager.LoadScene(name);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    private void SetLevelMusic(String name)
    {
        if (name == "SampleScene")
        {
            AudioManager.Instace.PlayMusic("MenuTheme");
        }
        else if (name == "TestLevel")
        {
            AudioManager.Instace.PlayMusic("MainTheme");
        }
    }
}
