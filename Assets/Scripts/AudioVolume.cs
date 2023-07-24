using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    public AudioSource[] sources;

    public float volume;
    // Start is called before the first frame update
    void Start()
    {
        foreach (AudioSource source in sources)
        {
            source.volume *= volume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
