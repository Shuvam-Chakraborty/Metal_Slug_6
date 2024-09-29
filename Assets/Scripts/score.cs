using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    // Start is called before the first frame update
    public Text scoreText;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            // Set score to 0 for level 1
            SceneController.instance.score = 0;
            scoreText.text = "0";
        }
        else
            scoreText.text = SceneController.instance.score.ToString();
    }
    public void setScore(int val)
    {
        scoreText.text = val.ToString();
    }

    public void increment_score(int amount)
    {
        long cur_val = int.Parse(scoreText.text);
        cur_val += amount;
        SceneController.instance.AddScore(amount);
        scoreText.text = cur_val.ToString();
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = SceneController.instance.score.ToString();
    }
}