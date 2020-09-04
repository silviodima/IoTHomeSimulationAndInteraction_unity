using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CondizionatoreController : MonoBehaviour
{
    public Text soggiorno, stanza;
    //file audio che simula il rumore del condiziontore
    public AudioClip rumore;
    //array di boolean di taglia 2 per tener conto dello stato on/off dei condizionatori
    public static bool[] isOn = { false, false };
    //array di float di taglia 2 per tener conto della temperatura dei condizionatori
    public static float[] conditionerTemperature = { 20, 20 };
    //booleano che viene settato a true quando si accende uno dei condizionatori (e quindi la temperatura può cambiare)
    public static bool checkTemperature;
    //booleano settato a true quando viene spento uno dei condizionatori (e quindi la temperatura della stanza va riportata a 23)
    public static bool resetTemperature;


    private GameObject temperaturaController;

    public static Animator animatorSoggiorno, animatorStanza;

    public static Animator[] animators;

    //oggetti che nella scena simuleranno la diffusione 
    public static GameObject airSoggiorno, airStanza;

    //vettori che salveranno la posizione iniziale degli oggetti precedenti
    private Vector3 airSoggiornoPos, airStanzaPos;

    private int room;


    private AudioSource rumoreCondSoggiorno, rumoreCondStanza;
    // Start is called before the first frame update
    void Start()
    {
        checkTemperature = false;
        resetTemperature = false;
        rumoreCondSoggiorno = GameObject.FindGameObjectWithTag("18").GetComponent<AudioSource>();
        rumoreCondStanza = GameObject.FindGameObjectWithTag("19").GetComponent<AudioSource>();
        temperaturaController = GameObject.FindGameObjectWithTag("temperaturaController");

        airSoggiorno = GameObject.FindGameObjectWithTag("airSoggiorno");
        airStanza = GameObject.FindGameObjectWithTag("airStanza");

        airSoggiornoPos = airSoggiorno.transform.position;
        airStanzaPos = airStanza.transform.position;

        animatorSoggiorno = airSoggiorno.GetComponent<Animator>();
        animatorStanza = airStanza.GetComponent<Animator>();
        animators = new Animator[2];
        animators[0] = animatorSoggiorno;
        animators[1] = animatorStanza;

    }

    // Update is called once per frame
    void Update()
    {
        if(checkTemperature)
        {
            checkTemperature = false;
            temperaturaController.GetComponent<TemperaturaController>().checkTemperature(room);
        }

        if(resetTemperature)
        {
            resetTemperature = false;
            temperaturaController.GetComponent<TemperaturaController>().resetTemperature(room);
        }
    }

    public void switchConditioner(int id, string action)
    {
        print("switch condizionatore");
        if (action.Equals("true"))
        {
            if (id == 18)
            {
                print("l'hai acceso");
                //index 0 per condizionatore del soggiorno
                isOn[0] = true;
                //aggiornamento della temperatura mostrata sul condizionatore
                soggiorno.text = conditionerTemperature[0] + "°C";
                //sound del condizionatore
                rumoreCondSoggiorno.Play();

                room = 0;

                //andiamo a fare il check sulla temperatura per aggiornare le temperature degli ambienti
                checkTemperature = true;
            }

            else if (id == 19)
            {
                //index 1 per condizionatore della stanza
                isOn[1] = true;
                stanza.text = conditionerTemperature[1] + "°C";
                rumoreCondStanza.Play();

                room = 1;

                // airStanza = (GameObject) Instantiate(myPrefabSphere, stanza.transform.position, Quaternion.identity);
                checkTemperature = true;

            }

        }

        if (action.Equals("false"))
        {
            if (id == 18)
            {
                isOn[0] = false;
                soggiorno.text = "-";
                rumoreCondSoggiorno.Stop();

                checkTemperature = false;
                //animatorSoggiorno.SetBool("diffusion", false);
                room = 0;

                resetTemperature = true;
                //airSoggiorno.transform.position = airSoggiornoPos;
               
               //airSoggiorno.transform.position = new Vector3(-8, 2, 3);
            }

            else if (id == 19)
            {
                isOn[1] = false;
                stanza.text = "-";
                rumoreCondStanza.Stop();

                checkTemperature = false;
                room = 1;
                resetTemperature = true;
            }
        }

    }


    public void changeTemp(int id, int action)
    {
     //se spento, inutile anche controllare l'id
     if(isOn[0])
        {
            //condizionatore del soggiorno
            if(id == 18)
            {
                soggiorno.text = action + "°C";
                conditionerTemperature[0] = action;

                checkTemperature = false;
                animators[0].SetBool("diffusion", false);
                //print("animatorbool:" + animators[0].GetBool("diffusion"));
                //airSoggiorno.transform.position = airSoggiornoPos;
                //animatorSoggiorno.SetBool("diffusion", true);
                //print("animatorbool ora:" + animatorSoggiorno.GetBool("diffusion"));
                //checkTemperature = true;
                Task.Delay(new TimeSpan(0, 0, 5)).ContinueWith(o => { checkTemperature = true; });




            }

            else
            {
                print("ACCENDILO PRIMA" + id);
            }
        }

        if (isOn[1])
        {
            //condizionatore della stanza
            if (id == 19)
            {
                stanza.text = action + "°C";
                conditionerTemperature[1] = action;
                checkTemperature = false;
                animators[1].SetBool("diffusion", false);
                //print("animatorbool:" + animators[0].GetBool("diffusion"));
                //airSoggiorno.transform.position = airSoggiornoPos;
                //animatorSoggiorno.SetBool("diffusion", true);
                //print("animatorbool ora:" + animatorSoggiorno.GetBool("diffusion"));
                //checkTemperature = true;
                Task.Delay(new TimeSpan(0, 0, 5)).ContinueWith(o => { checkTemperature = true; });

            }

            else
            {
                print("ACCENDILO PRIMA" + id);
            }
        }
    }
}
