using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    private Animator doorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("mammt");
            doorAnimator.SetBool("opendoor", true);
            print(""+ doorAnimator.GetBool("opendoor").ToString());
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
