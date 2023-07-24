using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollection : MonoBehaviour
{
    public AudioSource step;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void endClimb()
    {
        GetComponentInParent<Tagger>().endClimb();
    }

    public void endVault()
    {
        GetComponentInParent<Tagger>().endVault();
    }

    public void playStep()
    {
        step.Play();
    }

}
