using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TelecomandiCondController : MonoBehaviour
{
    //canvas dei monitor dei telecomandi + canvas delle temperature mostrate sui condizionatori
    public Text monitorStanza, monitorSoggiorno, soggiorno, stanza;

    public AudioClip rumore;

    private AudioSource rumoreCondSoggiorno, rumoreCondStanza;

    private GameObject mqtt, condSoggiorno, condStanza;

    private bool onCondSogg, onCondStanza;

    [Serializable]
    public class Command
    {
        public int type;
        public int id;
        public string cmd, component, action;
    }

    // Start is called before the first frame update
    void Start()
    {
        mqtt  = GameObject.FindGameObjectWithTag("mqtt");

        condSoggiorno = GameObject.FindGameObjectWithTag("18");
        condStanza = GameObject.FindGameObjectWithTag("19");


        rumoreCondSoggiorno = condSoggiorno.GetComponent<AudioSource>();
        rumoreCondStanza = condStanza.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (CondizionatoreController.isOn[0])
        {
            onCondSogg = true;
            monitorSoggiorno.text = "" + CondizionatoreController.conditionerTemperature[0] + "°C";
        }

        else
        {
            monitorSoggiorno.text = "-";
        }

        if (CondizionatoreController.isOn[1])
        {
            onCondSogg = false;
            monitorStanza.text = "" + CondizionatoreController.conditionerTemperature[1] + "°C";
        }

        else
        {
            monitorStanza.text = "-";
        }

        Command cmd = new Command();
        cmd.type = 0;
        //Check if Mouse Button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            //Raycast from mouse cursor pos
            RaycastHit rayCastHit;
            Ray rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayCast, out rayCastHit))
            {
                //se hai cliccato sul pulsante per accendee il condizionatore della stanza
                if (rayCastHit.collider.name.Equals("Pulsante Cond Stanza ON/OFF"))
                {
                    //se era acceso, spegni
                    if (onCondStanza)
                    {
                        onCondStanza = false;
                        //CondizionatoreController c = new CondizionatoreController();
                        //c.switchConditioner(19, "false");
                        monitorStanza.text = "-";
                        CondizionatoreController.isOn[1] = false;
                        stanza.text = "-";
                        rumoreCondStanza.Stop();

                        CondizionatoreController.checkTemperature = false;
                        CondizionatoreController.room = 1;
                        CondizionatoreController.resetTemperature = true;

                        cmd.action = "false";
                        cmd.id = Int32.Parse(condStanza.tag);
                        cmd.component = "c";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onCondStanza = true;
                        CondizionatoreController.isOn[1] = true;
                        stanza.text = CondizionatoreController.conditionerTemperature[1] + "°C";
                        rumoreCondStanza.Play();

                        CondizionatoreController.room = 1;

                        // airStanza = (GameObject) Instantiate(myPrefabSphere, stanza.transform.position, Quaternion.identity);
                        CondizionatoreController.checkTemperature = true;
                        monitorStanza.text = CondizionatoreController.conditionerTemperature[1]+"°C";
                        
                        cmd.action = "true";
                        cmd.id = Int32.Parse(condStanza.tag);
                        cmd.component = "c";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);

                        Command c = new Command();
                        cmd.action = "" + CondizionatoreController.conditionerTemperature[1];
                        cmd.id = Int32.Parse(condStanza.tag);
                        cmd.component = "c";
                        cmd.cmd = "temp";
                        cmd.type = 0;


                        string toJson2 = JsonUtility.ToJson(c);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson2);

                    }
                }

                //se hai cliccato sul + per alzare la temperatura
                if (rayCastHit.collider.name.Equals("Condizionatore Stanza +"))
                {
                    //se il condizionatore è acceso
                    if (CondizionatoreController.isOn[1])
                    {
                        CondizionatoreController.conditionerTemperature[1] += 1;

                        //aggiorna la temp sul cond
                        stanza.text = CondizionatoreController.conditionerTemperature[1] + "°C";
                        //aggiorna la temp sul telecomando
                        monitorStanza.text = CondizionatoreController.conditionerTemperature[1] + "°C";

                        CondizionatoreController.checkTemperature = false;
                        CondizionatoreController.animators[1].SetBool("diffusion", false);
                        Task.Delay(new TimeSpan(0, 0, 5)).ContinueWith(o => { CondizionatoreController.checkTemperature = true; });

                        cmd.action = "" + CondizionatoreController.conditionerTemperature[1];
                        cmd.id = Int32.Parse(condStanza.tag);
                        cmd.component = "c";
                        cmd.cmd = "temp";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per alzare la temperatura
                if (rayCastHit.collider.name.Equals("Condizionatore Stanza -"))
                {
                    //se il condizionatore è acceso
                    if (CondizionatoreController.isOn[1])
                    {
                        CondizionatoreController.conditionerTemperature[1] -= 1;

                        //aggiorna la temp sul cond
                        stanza.text = CondizionatoreController.conditionerTemperature[1] + "°C";
                        //aggiorna la temp sul telecomando
                        monitorStanza.text = CondizionatoreController.conditionerTemperature[1] + "°C";

                        CondizionatoreController.checkTemperature = false;
                        CondizionatoreController.animators[1].SetBool("diffusion", false);
                        Task.Delay(new TimeSpan(0, 0, 5)).ContinueWith(o => { CondizionatoreController.checkTemperature = true; });

                        cmd.action = "" + CondizionatoreController.conditionerTemperature[1];
                        cmd.id = Int32.Parse(condStanza.tag);
                        cmd.component = "c";
                        cmd.cmd = "temp";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }







                //se hai cliccato sul pulsante per accendee il condizionatore del soggiorno
                if (rayCastHit.collider.name.Equals("Pulsante Cond Soggiorno ON/OFF"))
                {
                    //se era acceso, spegni
                    if (onCondSogg)
                    {
                        onCondSogg = false;
                        //CondizionatoreController c = new CondizionatoreController();
                        //c.switchConditioner(19, "false");
                        monitorSoggiorno.text = "-";
                        CondizionatoreController.isOn[0] = false;
                        soggiorno.text = "-";
                        rumoreCondSoggiorno.Stop();

                        CondizionatoreController.checkTemperature = false;
                        CondizionatoreController.room = 0;
                        CondizionatoreController.resetTemperature = true;

                        cmd.action = "false";
                        cmd.id = Int32.Parse(condSoggiorno.tag);
                        cmd.component = "c";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onCondSogg = true;
                        CondizionatoreController.isOn[0] = true;
                        soggiorno.text = CondizionatoreController.conditionerTemperature[0] + "°C";
                        rumoreCondSoggiorno.Play();

                        CondizionatoreController.room = 0;

                        // airStanza = (GameObject) Instantiate(myPrefabSphere, stanza.transform.position, Quaternion.identity);
                        CondizionatoreController.checkTemperature = true;
                        monitorSoggiorno.text = CondizionatoreController.conditionerTemperature[0] + "°C";

                        cmd.action = "true";
                        cmd.id = Int32.Parse(condSoggiorno.tag);
                        cmd.component = "c";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);

                        Command c = new Command();
                        cmd.action = "" + CondizionatoreController.conditionerTemperature[0];
                        cmd.id = Int32.Parse(condSoggiorno.tag);
                        cmd.component = "c";
                        cmd.cmd = "temp";
                        cmd.type = 0;


                        string toJson2 = JsonUtility.ToJson(c);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson2);
                    }
                }

                //se hai cliccato sul + per alzare la temperatura
                if (rayCastHit.collider.name.Equals("Condizionatore Soggiorno +"))
                {
                    //se il condizionatore è acceso
                    if (CondizionatoreController.isOn[0])
                    {
                        CondizionatoreController.conditionerTemperature[0] += 1;

                        //aggiorna la temp sul cond
                        soggiorno.text = CondizionatoreController.conditionerTemperature[0] + "°C";
                        //aggiorna la temp sul telecomando
                        monitorSoggiorno.text = CondizionatoreController.conditionerTemperature[0] + "°C";

                        CondizionatoreController.checkTemperature = false;
                        CondizionatoreController.animators[0].SetBool("diffusion", false);
                        Task.Delay(new TimeSpan(0, 0, 5)).ContinueWith(o => { CondizionatoreController.checkTemperature = true; });

                        cmd.action = "" + CondizionatoreController.conditionerTemperature[0];
                        cmd.id = Int32.Parse(condSoggiorno.tag);
                        cmd.component = "c";
                        cmd.cmd = "temp";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per alzare la temperatura
                if (rayCastHit.collider.name.Equals("Condizionatore Soggiorno -"))
                {
                    //se il condizionatore è acceso
                    if (CondizionatoreController.isOn[0])
                    {
                        CondizionatoreController.conditionerTemperature[0] -= 1;

                        //aggiorna la temp sul cond
                        soggiorno.text = CondizionatoreController.conditionerTemperature[0] + "°C";
                        //aggiorna la temp sul telecomando
                        monitorSoggiorno.text = CondizionatoreController.conditionerTemperature[0] + "°C";

                        CondizionatoreController.checkTemperature = false;
                        CondizionatoreController.animators[0].SetBool("diffusion", false);
                        Task.Delay(new TimeSpan(0, 0, 5)).ContinueWith(o => { CondizionatoreController.checkTemperature = true; });

                        cmd.action = "" + CondizionatoreController.conditionerTemperature[0];
                        cmd.id = Int32.Parse(condSoggiorno.tag);
                        cmd.component = "c";
                        cmd.cmd = "temp";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }



            }


        }
    }
}
