using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    // Start is called before the first frame update

    public Text scoreText;
    public void setScore(int val)
    {
        scoreText.text = val.ToString();
    }

    public void increment_score(int amount)
    {
        long cur_val = int.Parse(scoreText.text);
        cur_val += amount;
        scoreText.text = cur_val.ToString();
    }
}
