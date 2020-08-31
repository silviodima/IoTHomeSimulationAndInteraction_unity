using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CondizionatoreController : MonoBehaviour
{
    public Text soggiorno, stanza;
    public AudioClip rumore;
    //array di boolean di taglia 2 per tener conto dello stato on/off dei condizionatori
    public static bool[] isOn = { false, false };
    //array di float di taglia 2 per tener conto della temperatura dei condizionatori
    public static float[] lastTemperature = { 20, 20 };
    private Object myPrefabSphere;
    private bool checkTemperature;
    private GameObject temperaturaController;

    public static GameObject airSoggiorno, airStanza;


    private AudioSource rumoreCondSoggiorno, rumoreCondStanza;
    // Start is called before the first frame update
    void Start()
    {
        checkTemperature = false;
        rumoreCondSoggiorno = GameObject.FindGameObjectWithTag("18").GetComponent<AudioSource>();
        rumoreCondStanza = GameObject.FindGameObjectWithTag("19").GetComponent<AudioSource>();
        myPrefabSphere = Resources.Load<GameObject>("Sphere") as GameObject;
        temperaturaController = GameObject.FindGameObjectWithTag("temperaturaController");
    }

    // Update is called once per frame
    void Update()
    {
        if(checkTemperature)
        {
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

                airSoggiorno = (GameObject) Instantiate(myPrefabSphere, soggiorno.transform.position, Quaternion.identity);
                airSoggiorno.tag = "airSoggiorno";
                checkTemperature = true;
            }

            else if (id == 19)
            {
                //index 1 per condizionatore della stanza
                isOn[1] = true;
                stanza.text = lastTemperature[1] + "°C";
                rumoreCondStanza.Play();
                airStanza = (GameObject) Instantiate(myPrefabSphere, stanza.transform.position, Quaternion.identity);
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

                Destroy(airSoggiorno);
            }

            else if (id == 19)
            {
                isOn[1] = false;
                stanza.text = "-";
                rumoreCondStanza.Stop();
                checkTemperature = false;

                Destroy(airStanza);

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
