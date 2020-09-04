using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TemperaturaController : MonoBehaviour
{
    public Text soggiorno, stanza, cucina, bagno, ingresso;
    public static float[] roomTemperature = { 10, 23, 23, 23, 23 };
    private float diffusion, incremento, positionDifference, counter;
    private float[]initialPositionAir, finalPositionAir;
    private bool checkPositionSogg, checkPositionStan, tempPlus, resetTempSogg;
    private Text[] roomText;
    private int room;

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
        finalPositionAir[0] = -3;
        finalPositionAir[1] = 8;
        initialPositionAir = new float[2];
        initialPositionAir[0] = -23;
        initialPositionAir[1] = 24;

        roomText = new Text[2];
        roomText[0] = soggiorno;
        roomText[1] = stanza;


        //booleano per capire se aumentare o decrementare la temperatura, a seconda del controllo che assegna valore a diffusion
        tempPlus = false;

        resetTempSogg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkPositionSogg && CondizionatoreController.isOn[0] && CondizionatoreController.conditionerTemperature[0]!=roomTemperature[0])
        {
          print("ora cambia");
          checkAirPosition(CondizionatoreController.airSoggiorno, room);
        }

        if (checkPositionSogg && CondizionatoreController.isOn[1] && CondizionatoreController.conditionerTemperature[1] != roomTemperature[1])
        {
            //print("sto controllando");
            checkAirPosition(CondizionatoreController.airStanza, room);
        }

        if (resetTempSogg && 23 != roomTemperature[0])
        {
            //print("sto controllando");
            resetTemp(CondizionatoreController.airSoggiorno, room);
        }

        if (resetTempSogg && 23 != roomTemperature[1])
        {
            //print("sto controllando");
            resetTemp(CondizionatoreController.airStanza, room);
        }
    }

    public void checkTemperature(int index)
    {
        room = index;

        //cammino da fare per la palla del soggiorno: da -23 a -3
        positionDifference = initialPositionAir[index] - finalPositionAir[index];
        if (CondizionatoreController.isOn[index])
        {
            print("calcoiamo" + roomTemperature[index]+","+CondizionatoreController.conditionerTemperature[0]);
            //diffusion è la variabile che ci indica la differenza di temperature: maggiore è la differenza, più tempo ci vorrà affinchè nella stanza venga raggiunta la temp. scelta sul condizionatore
            if (CondizionatoreController.conditionerTemperature[index] > roomTemperature[index])
            {
                print("temperatura della stanza da alzare");
                diffusion = CondizionatoreController.conditionerTemperature[index] - roomTemperature[index];
                tempPlus = true;
                //print("diffusion: "+diffusion);
            }

            else if (roomTemperature[index] > CondizionatoreController.conditionerTemperature[index])
            {
                print("temperatura della stanza da abbassare");
                diffusion = roomTemperature[index] - CondizionatoreController.conditionerTemperature[index];
                //print("diffusion: " + diffusion);
            }
            //la differenza (diffusion) è pari a 0: nessun aggiornamento da fare: il gameobject air non viaggerà
            else
            {
                //CondizionatoreController.animatorSoggiorno.enabled = true;
                //return;
                diffusion = 0;
                CondizionatoreController.animators[index].enabled = true;
                CondizionatoreController.animators[index].speed = 2;
                CondizionatoreController.animators[index].SetBool("diffusion", true);
                checkPositionSogg = true;
                return;

            }
            incremento = positionDifference / diffusion;
            counter = incremento;
            

            //settaggi per far partire l'animazione di movimento del gameobject che simulerà lo spostamento dell'aria
            CondizionatoreController.animators[index].enabled = true;
            //settiamo la velocità dell'animazione, e quindi del gameobject, in base alla differenza di temperatura tra quella dell'ambiente e quella scelta sul condizionatore
            //dividiamo 3 per diffusion: se si pensa che sia troppo lento, aumentare il 3
            float velocity = (float)Math.Round(3/ diffusion, 2, MidpointRounding.AwayFromZero);
            CondizionatoreController.animators[index].speed = velocity;
         
            //settiamo diffusion (bool nell'animator) a true, condizione necessaria per far cambiare stato all'animator
            CondizionatoreController.animators[index].SetBool("diffusion", true);

            //andiamo a controllare la posizione del gameobject
            checkPositionSogg = true;

        }
    }

    public void resetTemperature(int index)
    {
        room = index;
        if (!CondizionatoreController.isOn[index])
        {
            //diffusion è la variabile che ci indica la differenza di temperature: maggiore è la differenza, più tempo ci vorrà affinchè nella stanza venga raggiunta la temp. base della casa (23)
            if (23 > roomTemperature[index])
            {
                print("temperatura della stanza da alzare ueue");
                diffusion =23 - roomTemperature[index];
                tempPlus = true;
                //print("diffusion: "+diffusion);
            }

            else if (roomTemperature[index] > 23)
            {
                print("temperatura della stanza da abbassare");
                diffusion = roomTemperature[index] -23;
                //print("diffusion: " + diffusion);
            }
            //la differenza (diffusion) è pari a 0: nessun aggiornamento da fare: il gameobject air non viaggerà
            else
            {
                //CondizionatoreController.animatorSoggiorno.enabled = true;
                //return;
                diffusion = 0;
                CondizionatoreController.animators[index].enabled = true;
                CondizionatoreController.animators[index].speed = 2;
                CondizionatoreController.animators[index].SetBool("diffusion", false);
                resetTempSogg = true;
                return;

            }
            incremento = positionDifference / diffusion;
            counter = incremento;


            //settaggi per far partire l'animazione di movimento del gameobject che simulerà lo spostamento dell'aria
            CondizionatoreController.animators[index].enabled = true;
            //settiamo la velocità dell'animazione, e quindi del gameobject, in base alla differenza di temperatura tra quella dell'ambiente e quella scelta sul condizionatore
            //dividiamo 3 per diffusion: se si pensa che sia troppo lento, aumentare il 3
            float velocity = (float)Math.Round(3 / diffusion, 2, MidpointRounding.AwayFromZero);
            CondizionatoreController.animators[index].speed = velocity;

            //settiamo diffusion (bool nell'animator) a true, condizione necessaria per far cambiare stato all'animator
            CondizionatoreController.animators[index].SetBool("diffusion", false);

            //andiamo a controllare la posizione del gameobject
            resetTempSogg = true;

        }
    }
    void checkAirPosition(GameObject air, int room)
    {
        print("room" + room + "diffusion" + diffusion + "incremento" + incremento+"position"+positionDifference);
        //se dobbiamo controllare la posizione della palla nel soggiorno
        if(diffusion!=0 && CondizionatoreController.isOn[room])
        {
            //la sfera dovrà fare #incremento passi, ciascuno di dimensione counter (es. temperatura stanza 30, temperatura condizionatore 20: incremento sarà uguale a positionDifference/diffusion cioè
            // 20/10 all'inizio, e man mano verrà incrementata di counter
            //In questa sorta di if ciclato(update provvede a chiamare per ogni frame questo metodo), controlliamo man mano la posizione della sfera, andando a incrementare incremento ogni volta che 
            //la sfera ha effettuato un passo di dimensione counter
            if (room == 0)
            {
                if (air.transform.position.x > initialPositionAir[room] - incremento)
                {
                    incremento += counter;
                    //temperatura da incrementare fin quando non coincidono
                    if (tempPlus && roomTemperature[room] < CondizionatoreController.conditionerTemperature[room])
                    {
                        roomTemperature[room]++;
                        //print("OH");
                        // tempPlus = false;
                    }

                    //temperatura da decrementare
                    else
                    {
                        print("stiamo abbassando" + room);
                        roomTemperature[room]--;
                    }

                    if (room == 0)
                        roomText[room].text = "SOGGIORNO: " + roomTemperature[room] + "°C";

                    else if (room == 1)
                        roomText[room].text = "STANZA: " + roomTemperature[room] + "°C";
                    //print("ecco"+incremento);
                }
            }

            else if (room == 1)
            {
                if (air.transform.position.x < initialPositionAir[room] - incremento)
                {
                    incremento += counter;
                    //temperatura da incrementare fin quando non coincidono
                    if (tempPlus && roomTemperature[room] < CondizionatoreController.conditionerTemperature[room])
                    {
                        roomTemperature[room]++;
                        //print("OH");
                        // tempPlus = false;
                    }

                    //temperatura da decrementare
                    else
                    {
                        print("stiamo abbassando" + room);
                        roomTemperature[room]--;
                    }

                    if (room == 0)
                        roomText[room].text = "SOGGIORNO: " + roomTemperature[room] + "°C";

                    else if (room == 1)
                        roomText[room].text = "STANZA: " + roomTemperature[room] + "°C";
                    //print("ecco"+incremento);
                }
            }

            //condizione di uscita per evitare le continue chiamate da update
            if (incremento==(diffusion*counter)+counter)
            {
                checkPositionSogg = false;
                tempPlus = false;
            }






        }
    }

    public void resetTemp(GameObject air, int room)
    {
        if ( diffusion != 0 && !CondizionatoreController.isOn[room])
        {
            //la sfera dovrà fare #incremento passi, ciascuno di dimensione counter (es. temperatura stanza 30, temperatura condizionatore 20: incremento sarà uguale a positionDifference/diffusion cioè
            // 20/10 all'inizio, e man mano verrà incrementata di counter
            //In questa sorta di if ciclato(update provvede a chiamare per ogni frame questo metodo), controlliamo man mano la posizione della sfera, andando a incrementare incremento ogni volta che 
            //la sfera ha effettuato un passo di dimensione counter
            if (room == 0)
            {
                if (air.transform.position.x < finalPositionAir[room] - incremento)
                {
                    incremento += counter;
                    //temperatura da incrementare fin quando non coincidono
                    if (tempPlus && roomTemperature[room] < 23)
                    {
                        roomTemperature[room]++;
                        //print("OH");
                        // tempPlus = false;
                    }

                    //temperatura da decrementare
                    else
                    {
                        //print("UI");
                        roomTemperature[room]--;
                    }

                    if (room == 0)
                        roomText[room].text = "SOGGIORNO: " + roomTemperature[room] + "°C";

                    else if (room == 1)
                        roomText[room].text = "STANZA: " + roomTemperature[room] + "°C";
                    //print("ecco"+incremento);
                }
            }

            else if (room == 1)
            {
                if (air.transform.position.x > finalPositionAir[room] - incremento)
                {
                    incremento += counter;
                    //temperatura da incrementare fin quando non coincidono
                    if (tempPlus && roomTemperature[room] < 23)
                    {
                        roomTemperature[room]++;
                        //print("OH");
                        // tempPlus = false;
                    }

                    //temperatura da decrementare
                    else
                    {
                        //print("UI");
                        roomTemperature[room]--;
                    }

                    if (room == 0)
                        roomText[room].text = "SOGGIORNO: " + roomTemperature[room] + "°C";

                    else if (room == 1)
                        roomText[room].text = "STANZA: " + roomTemperature[room] + "°C";
                    //print("ecco"+incremento);
                }
            }

            //condizione di uscita per evitare le continue chiamate da update
            if (incremento == (diffusion * counter) + counter)
            {
                resetTempSogg = false;
                tempPlus = false;
            }






        }
    }

}
