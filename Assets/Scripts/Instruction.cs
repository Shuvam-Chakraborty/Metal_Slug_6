using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instruction : MonoBehaviour
{
    public void ContinueGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
