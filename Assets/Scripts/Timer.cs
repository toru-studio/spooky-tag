using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 30;
    public TextMeshProUGUI timeText;
    
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeText.text = "Time Remaining: " + timeRemaining % 60;
        }
        else
        {
            timeText.text = "Round Complete";
            string currentScene = SceneManager.GetActiveScene().name;
            var num = currentScene.Last();
            var nextNum = char.GetNumericValue(num) + 1;
            if (nextNum == 3)
            {
                SceneManager.LoadScene("Start Screen");
            }
            else
            {
                SceneManager.LoadScene("Level " + nextNum);
            }
        }
        
    }
}
