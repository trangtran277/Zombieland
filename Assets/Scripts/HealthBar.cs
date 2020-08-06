using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
     // Start is called before the first frame update
     public Slider slider;
     public Gradient gradient;
     public Image fill;

     public void SetMaxHealth(float health)
     {
          slider.maxValue = health;
          slider.value = health;

          fill.color = gradient.Evaluate(2f);
     }
       public void TakeDame(float dame)
    {
        slider.value -= dame;
    }
    public void PlusDame(float heal)
    {
        slider.value += heal;
    }
    public void SetHealth(float health)
     {
          slider.value = health;

          fill.color = gradient.Evaluate(slider.normalizedValue);
     }
     public float GetHealth()
     {
          return slider.value;
     }
}
