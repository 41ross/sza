﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void Play()
    {
        ProgressSceneLoader sceneLoader = Global.Component.GetProgressSceneLoader();
        sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit() 
    {
        Application.Quit();
    }
}
