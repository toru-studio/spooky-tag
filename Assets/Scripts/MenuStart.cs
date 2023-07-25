using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuStart : MonoBehaviour
{
    public AudioSource start;
    public AudioSource music;
    public Canvas cover;
    public GameObject Text;
    private bool starting;
    void Start()
    {
        starting = true;
        StartCoroutine(MenuCoroutine());
    }

    private void FixedUpdate()
    {
        if (starting)
        {
            Text.transform.localScale += 0.01f*Text.transform.localScale;
        }
    }

    IEnumerator MenuCoroutine()
    {
        start.Play();

        yield return new WaitForSeconds(start.clip.length - 1f);
        
        starting = false;

        music.Play();

        cover.sortingOrder = -1;
    }
}
