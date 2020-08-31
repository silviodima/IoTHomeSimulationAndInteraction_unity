using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperaturaController : MonoBehaviour
{
    public Text soggiorno, stanza, cucina, bagno, ingresso;
    private float[] actualTemperature = { 23, 23, 23, 23, 23 };
    private float diffusion;
    // Start is called before the first frame update
    void Start()
    {
        soggiorno.text = soggiorno.text + actualTemperature[0] + "°C";
        stanza.text = stanza.text + actualTemperature[1] + "°C";
        cucina.text = cucina.text + actualTemperature[2] + "°C";
        bagno.text = bagno.text + actualTemperature[3] + "°C";
        ingresso.text = ingresso.text + actualTemperature[4] + "°C";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkTemperature()
    {
        if(CondizionatoreController.isOn[0])
        {
            print("è acceso il condizionatore del soggiorno");
            if(CondizionatoreController.lastTemperature[0]>actualTemperature[0])
            {
                diffusion = CondizionatoreController.lastTemperature[0] - actualTemperature[0];
               // print("diffusion: "+diffusion);
            }

            else 
            {
                diffusion = actualTemperature[0] - CondizionatoreController.lastTemperature[0];
               // print("diffusion: " + diffusion);
            }

          //  Diffusion(CondizionatoreController.airSoggiorno);
            
            for(int i = 1; i<diffusion; i++)
            {

            }
        }

        if (CondizionatoreController.isOn[1])
        {
            print("è acceso il condizionatore della stanza");
        }
    }


    void Diffusion(GameObject air)
    {
        print("CE STAMO");

   
        //if (air.CompareTag("airSoggiorno"))
        //{
        //    print("OH");
        //    if (air.transform.position.x < -2.9)
        //    {
        //        air.transform.position = new Vector3(air.transform.position.x + 0.0001f, air.transform.position.y, air.transform.position.z);
        //    }
        //}
    }
}
