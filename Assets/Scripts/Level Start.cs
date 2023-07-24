using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    public Tagger player;
    public AI_Strategy enemy;
    public AudioSource crowd;
    public AudioSource countDown;
    public AudioSource music;
    public CameraController camera;
    private float speed;
    public float timeRemaining = 30;
    public TextMeshProUGUI timeText;
    private bool waiting;

    void Start()
    {
        player.canMove = false;
        camera.enabled = false;

        waiting = true;
        speed = enemy.agent.speed;
        
        enemy.agent.speed = 0;
        enemy.agent.updatePosition = false;


        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        crowd.Play();

        yield return new WaitForSeconds(3);

        countDown.Play();

        yield return new WaitForSeconds(countDown.clip.length);

        crowd.Stop();
        music.Play();
        player.canMove = true;
        camera.enabled = true;
        enemy.agent.speed = speed;
        enemy.agent.updatePosition = true;
        
        waiting = false;

    }

    
    void Update()
    {
        if (!waiting)
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
            };
        }
        
  
        
    }
}