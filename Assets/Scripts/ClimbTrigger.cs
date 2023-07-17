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
        Tagger[] taggers = other.gameObject.GetComponents<Tagger>();
        Vector3 dir = other.transform.position - transform.position;

        float yCoord = parentObject.transform.position.y +
                       parentObject.transform.localScale.y / 2f + 1;
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
