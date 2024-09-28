using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverLogic : MonoBehaviour
{
    public void GameMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }


}
