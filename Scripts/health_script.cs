using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health_script : MonoBehaviour
{
    public Slider slide;

    public void setMaxHealth(int health)
    {
        slide.maxValue = health;
        slide.value = health;
    }
    public void setHealth(int health)
    {
        slide.value = health;
    }

    public void weaker(int amount)
    {
        slide.value -= amount;
    }

    public bool is_dead()
    {
        return slide.value <= 0;
    }

    public void increase_health(int amount)
    {
        // Ensure the health does not exceed the max value
        slide.value = Mathf.Min(slide.value + amount, slide.maxValue);
    }
}
