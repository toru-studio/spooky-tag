using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    public Tagger player;
    public AI_Strategy enemy;
    public AudioSource crowd;
    public AudioSource countDown;
    public AudioSource music;
    public CameraController camera;

    void Start()
    {
        player.canMove = false;
        camera.enabled = false;
        if (enemy != null)
        {
            enemy.agent.updatePosition = false;
        }

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
        if (enemy != null)
        {
            enemy.agent.updatePosition = true;
        }


    }

}
