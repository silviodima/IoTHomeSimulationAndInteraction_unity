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
        if (action.Equals("true"))
        {
            lamp.GetComponent<Light>().enabled = true;
        }

        else if (action.Equals("false"))
        {
            lamp.GetComponent<Light>().enabled = false;
        }
    }

    public void powerLamp(int id, int action)
    {
        lamp = GameObject.FindGameObjectWithTag(id.ToString());

        if (lamp.GetComponent<Light>().enabled)
        {
            lamp.GetComponent<Light>().intensity = action / 10;
        }

        else
        {
            print("luce spenta");
        }

    }

    }
