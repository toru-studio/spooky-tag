using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultTrigger : MonoBehaviour
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

            float scale = parentObject.transform.localScale.x > parentObject.transform.localScale.z
                ? parentObject.transform.localScale.x / 2f
                : parentObject.transform.localScale.z / 2f;

            float dotRight = Vector3.Dot(localDir, transform.right);
            float dotForward = Vector3.Dot(localDir, transform.forward);
            
            Vector3 pos;
            
            if (Math.Abs(dotRight) > Math.Abs(dotForward) && parentObject.transform.localScale.x <= parentObject.transform.localScale.y)
            {
                float xOffset = (scale + other.transform.localScale.x) * Mathf.Sign(dotRight);
                pos = other.transform.position;
                pos.x -= xOffset;
            }
            else if (parentObject.transform.localScale.y <= parentObject.transform.localScale.x)
            {
                print("test");
                float zOffset = (scale + other.transform.localScale.x) * Mathf.Sign(dotForward);
                pos = other.transform.position;
                pos.z -= zOffset;
            }
            else
            {
                return;
            }
            


            if (taggers.Length > 0)
            {
                foreach (Tagger tagger in taggers)
                { 
                    tagger.beginVault(pos);
                }
            }
        }
}
