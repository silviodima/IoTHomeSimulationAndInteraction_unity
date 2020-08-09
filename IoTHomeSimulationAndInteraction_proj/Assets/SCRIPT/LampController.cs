using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampController : MonoBehaviour
{
    private GameObject lamp;
    // Start is called before the first frame update
    void Start()
    { 


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchLamp(int id, string action)
    {
        lamp = GameObject.FindGameObjectWithTag(id.ToString());
        if (action.Equals("on"))
            lamp.GetComponent<Light>().enabled = true;

        else lamp.GetComponent<Light>().enabled = false;
    }

}
