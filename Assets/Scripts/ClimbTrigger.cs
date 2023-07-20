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
        Vector3 dir = other.transform.position - parentObject.transform.position;

        Vector3 localDir = transform.InverseTransformDirection(dir);
        // Set y to the top of the parent object + taggerHeight
        float yCoord = parentObject.transform.position.y +
                       parentObject.transform.localScale.y / 2 + 1;
        // Set the x & z position of the tagger to the edge of the parent game object
        // relative to their current position.
        // float xCoord = parentObject.transform.position.x - 1 + 
        //               (parentObject.transform.localScale.x * dir.normalized.x) / 2f;
        // float zCoord = parentObject.transform.position.z - 1 + 
        //                (parentObject.transform.localScale.z * dir.normalized.z) / 2f;
        
        float dotRight = Vector3.Dot(localDir,  parentObject.transform.right);
        float dotForward = Vector3.Dot(localDir, parentObject.transform.forward);
                    
        Vector3 pos;
        
        if (Math.Abs(dotRight) > Math.Abs(dotForward))
        {
            float xOffset = (other.transform.localScale.x) * localDir.normalized.x; 
            pos = other.transform.position;
            pos.x -= xOffset;
        }
        else
        {
            float zOffset = (other.transform.localScale.x) * localDir.normalized.z; 
            pos = other.transform.position;
            pos.z -= zOffset;
        }

        pos.y = yCoord;
        
        if (taggers.Length > 0)
        {
            foreach (Tagger tagger in taggers)
            { 
                tagger.beginClimb(pos);
                
            }
        }
    }
}
