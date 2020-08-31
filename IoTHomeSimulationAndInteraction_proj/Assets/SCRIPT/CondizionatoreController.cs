using System.Collections;
using System.Collections.Generic;
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
    public static float[] lastTemperature = { 20, 20 };
    //booleano che viene settato a true quando si accende uno dei condizionatori (e quindi la temperatura può cambiare)
    private bool checkTemperature;
    
    private GameObject temperaturaController;

    public static Animator animatorSoggiorno, animatorStanza;

    //oggetti che nella scena simuleranno la diffusione 
    public GameObject airSoggiorno, airStanza;

    //vettori che salveranno la posizione iniziale degli oggetti precedenti
    private Vector3 airSoggiornoPos, airStanzaPos;


    private AudioSource rumoreCondSoggiorno, rumoreCondStanza;
    // Start is called before the first frame update
    void Start()
    {
        checkTemperature = false;
        rumoreCondSoggiorno = GameObject.FindGameObjectWithTag("18").GetComponent<AudioSource>();
        rumoreCondStanza = GameObject.FindGameObjectWithTag("19").GetComponent<AudioSource>();
        temperaturaController = GameObject.FindGameObjectWithTag("temperaturaController");

        airSoggiornoPos = airSoggiorno.transform.position;
        airStanzaPos = airStanza.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(checkTemperature)
        {
            checkTemperature = false;
            temperaturaController.GetComponent<TemperaturaController>().checkTemperature();
        }
    }

    public void switchConditioner(int id, string action)
    {
        print("switch condizionatore");
        if (action.Equals("true"))
        {
            if (id == 18)
            {
                //index 0 per condizionatore del soggiorno
                isOn[0] = true;
                soggiorno.text = lastTemperature[0] + "°C";
                rumoreCondSoggiorno.Play();

                animatorSoggiorno = airSoggiorno.GetComponent<Animator>();
                animatorSoggiorno.enabled = true;
                animatorSoggiorno.speed = 0.2f;
                animatorSoggiorno.SetBool("diffusion", true);

                //airSoggiorno = (GameObject) Instantiate(myPrefabSphere, soggiorno.transform.position, Quaternion.identity);
                //airSoggiorno.tag = "airSoggiorno";
                //airSoggiorno.AddComponent<Animator>();
                //airSoggiorno.AddComponent<Animation>().AddClip();

                checkTemperature = true;
            }

            else if (id == 19)
            {
                //index 1 per condizionatore della stanza
                isOn[1] = true;
                stanza.text = lastTemperature[1] + "°C";
                rumoreCondStanza.Play();

                animatorStanza = airStanza.GetComponent<Animator>();
                animatorStanza.speed = 0.2f;
                animatorStanza.SetBool("diffusion", true);

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

                animatorSoggiorno.enabled = false;
                airSoggiorno.transform.position = new Vector3(-8, 2, 3);
            }

            else if (id == 19)
            {
                isOn[1] = false;
                stanza.text = "-";
                rumoreCondStanza.Stop();
                checkTemperature = false;

                airStanza.transform.position = airStanzaPos;

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
                lastTemperature[0] = action;
                checkTemperature = true;

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
                lastTemperature[1] = action;
                checkTemperature = true;

            }

            else
            {
                print("ACCENDILO PRIMA" + id);
            }
        }
    }
}
