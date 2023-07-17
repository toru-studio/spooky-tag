using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTrigger : MonoBehaviour
{
    private GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get taggers interacting with the trigger
        Tagger[] taggers = other.gameObject.GetComponents<Tagger>();
        // Get the taggers position relative to the trigger
        Vector3 dir = other.transform.position - transform.position;

        // Set y to the top of the parent object + taggerHeight
        float yCoord = parentObject.transform.position.y +
                       parentObject.transform.localScale.y / 2f + 1;
        // Set the x & z position of the tagger to the edge of the parent game object
        // relative to their current position.
        float xCoord = parentObject.transform.position.x +
                      (parentObject.transform.localScale.x * dir.normalized.x) / 2f;
        float zCoord = parentObject.transform.position.z +
                       (parentObject.transform.localScale.z * dir.normalized.z) / 2f;

        if (taggers.Length > 0)
        {
            foreach (Tagger tagger in taggers)
            { 
                tagger.beginClimb(new Vector3(xCoord, yCoord, zCoord));
            }
        }
    }
}
