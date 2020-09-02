using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class TemperaturaController : MonoBehaviour
{
    public Text soggiorno, stanza, cucina, bagno, ingresso;
    private float[] roomTemperature = { 21, 23, 23, 23, 23 };
    private float diffusion, incremento, positionDifference, counter;
    private float[]initialPositionAir, finalPositionAir;
    private bool checkPositionSogg, checkPositionStan, tempPlus;

    // Start is called before the first frame update
    void Start()
    {
        soggiorno.text = soggiorno.text + roomTemperature[0] + "°C";
        stanza.text = stanza.text + roomTemperature[1] + "°C";
        cucina.text = cucina.text + roomTemperature[2] + "°C";
        bagno.text = bagno.text + roomTemperature[3] + "°C";
        ingresso.text = ingresso.text + roomTemperature[4] + "°C";

        checkPositionSogg = false;
        finalPositionAir = new float[2];
        finalPositionAir[1] = -3;
        initialPositionAir = new float[2];
        initialPositionAir[1] = -23;

        //cammino da fare per la palla del soggiorno: da -23 a -3
        positionDifference = initialPositionAir[1] - finalPositionAir[1];

        //booleano per capire se aumentare o decrementare la temperatura, a seconda del controllo che assegna valore a diffusion
        tempPlus = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (checkPositionSogg)
        {
            //print("sto controllando");
          checkAirPosition(CondizionatoreController.airSoggiorno);
        }
    }

    public void checkTemperature()
    {
        print("OILLOC");
        if(CondizionatoreController.isOn[0])
        {
            //print("calcoiamo" + roomTemperature[0]+CondizionatoreController.conditionerTemperature[0]);
            //diffusion è la variabile che ci indica la differenza di temperature: maggiore è la differenza, più tempo ci vorrà affinchè nella stanza venga raggiunta la temp. scelta sul condizionatore
            if(CondizionatoreController.conditionerTemperature[0]>roomTemperature[0])
            {
                //print("ueue");
                diffusion = CondizionatoreController.conditionerTemperature[0] - roomTemperature[0];
                tempPlus = true;
                //print("diffusion: "+diffusion);
            }

            else if(roomTemperature[0]>CondizionatoreController.conditionerTemperature[0])
            {
                //print("UOUO");
                diffusion = roomTemperature[0] - CondizionatoreController.conditionerTemperature[0];
                //print("diffusion: " + diffusion);
            }
            //la differenza (diffusion) è pari a 0: nessun aggiornamento da fare: la palla non viaggerà
            else return;

            incremento = positionDifference / diffusion;
            counter = incremento;
            

            //settaggi per far partire l'animazione di movimento del gameobject che simulerà lo spostamento dell'aria
            CondizionatoreController.animatorSoggiorno.enabled = true;
            //settiamo la velocità dell'animazione, e quindi del gameobject, in base alla differenza di temperatura tra quella dell'ambiente e quella scelta sul condizionatore
            //dividiamo 3 per diffusion: se si pensa che sia troppo lento, aumentare il 3
            float velocity = (float)Math.Round(3/ diffusion, 2, MidpointRounding.AwayFromZero);
            CondizionatoreController.animatorSoggiorno.speed = velocity;
         
            //settiamo diffusion (bool nell'animator) a true, condizione necessaria per far cambiare stato all'animator
            CondizionatoreController.animatorSoggiorno.SetBool("diffusion", true);

            //andiamo a controllare la posizione del gameobject
            checkPositionSogg = true;

        }

        if (CondizionatoreController.isOn[1])
        {
            print("è acceso il condizionatore della stanza");
        }
    }

    void checkAirPosition(GameObject air)
    {
        //print("DIFFUSIOEN CAZZO" + diffusion);
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //THREAD?
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //se dobbiamo controllare la posizione della palla nel soggiorno
        if(air.CompareTag("airSoggiorno") && diffusion!=0)
        {
            //la sfera dovrà fare #incremento passi, ciascuno di dimensione counter (es. temperatura stanza 30, temperatura condizionatore 20: incremento sarà uguale a positionDifference/diffusion cioè
            // 20/10 all'inizio, e man mano verrà incrementata di counter
            //In questa sorta di if ciclato(update provvede a chiamare per ogni frame questo metodo), controlliamo man mano la posizione della sfera, andando a incrementare incremento ogni volta che 
            //la sfera ha effettuato un passo di dimensione counter
            if (air.transform.position.x > initialPositionAir[1] - incremento)
            {
                incremento+= counter;
                //temperatura da incrementare
                if (tempPlus)
                {
                    roomTemperature[0]++;
                }

                //temperatura da decrementare
                else
                {
                    roomTemperature[0]--;
                }
                soggiorno.text = "SOGGIORNO: " + roomTemperature[0] + "°C";
                //print("ecco"+incremento);
            }

            //condizione di uscita per evitare le continue chiamate da update
            if(incremento==(diffusion*counter)+counter)
            {
                checkPositionSogg = false;
            }

              
            

         

        }
    }

}
