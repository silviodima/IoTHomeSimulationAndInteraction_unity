using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterruttoriController : MonoBehaviour
{
    private GameObject mqtt;
    private float dimmerIntensityIngresso, dimmerIntensitySoggiorno, dimmerIntensityStanza;

    private bool onIngresso, onBagno, onFarettoBagno, onCucina, onSoggiorno, onPiantanaSoggiorno, onStanza, onAbat1, onAbat2;
    public GameObject lampadinaIngresso, lampadinaBagno, farettoBagno, lampadinaCucina, lampadinaSoggiorno, piantanaSoggiorno, lampadinaStanza, abat1, abat2;
    public Text dimmerIngresso, dimmerSoggiorno, dimmerStanza;
    public GameObject dimmerIngressoPlus, dimmerIngressoMinus, dimmerSoggiornoPlus, dimmerSoggiornoMinus, dimmerStanzaPlus, dimmerStanzaMinus;

    [Serializable]
    public class Command
    {
        public int type;
        public int id;
        public string cmd, component, action;
    }

    private void Start()
    {
        if (lampadinaIngresso.GetComponent<Light>().enabled)
            onIngresso = true;
        else if (!lampadinaIngresso.GetComponent<Light>().enabled)
            onIngresso = false;


        if (lampadinaBagno.GetComponent<Light>().enabled)
            onBagno = true;
        else if (!lampadinaBagno.GetComponent<Light>().enabled)
            onBagno = false;

        if (farettoBagno.GetComponent<Light>().enabled)
            onFarettoBagno = true;
        else if (!farettoBagno.GetComponent<Light>().enabled)
            onFarettoBagno = false;


        if (lampadinaCucina.GetComponent<Light>().enabled)
            onCucina = true;
        else if (!lampadinaCucina.GetComponent<Light>().enabled)
            onCucina = false;


        if (lampadinaSoggiorno.GetComponent<Light>().enabled)
            onSoggiorno = true;
        else if (!lampadinaSoggiorno.GetComponent<Light>().enabled)
            onSoggiorno = false;


        if (piantanaSoggiorno.GetComponent<Light>().enabled)
            onPiantanaSoggiorno = true;
        else if (!piantanaSoggiorno.GetComponent<Light>().enabled)
            onPiantanaSoggiorno = false;


        if (lampadinaStanza.GetComponent<Light>().enabled)
            onStanza = true;
        else if (!lampadinaStanza.GetComponent<Light>().enabled)
            onStanza = false;


        if (abat1.GetComponent<Light>().enabled)
            onAbat1 = true;
        else if (!abat1.GetComponent<Light>().enabled)
            onAbat1 = false;


        if (abat2.GetComponent<Light>().enabled)
            onAbat2 = true;
        else if (!abat2.GetComponent<Light>().enabled)
            onAbat2 = false;

        mqtt = GameObject.FindGameObjectWithTag("mqtt");

        dimmerIntensityIngresso = lampadinaIngresso.GetComponent<Light>().intensity;
        dimmerIngresso.text = "" + dimmerIntensityIngresso*10;

        dimmerIntensitySoggiorno = lampadinaSoggiorno.GetComponent<Light>().intensity;
        dimmerSoggiorno.text = "" + dimmerIntensitySoggiorno * 10;

        dimmerIntensityStanza = lampadinaStanza.GetComponent<Light>().intensity;
        dimmerStanza.text = "" + dimmerIntensityStanza * 10;

    }

    void Update()
    {
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
                //se hai cliccato su Interruttore ingresso
                if (rayCastHit.collider.name.Equals("Interruttore Ingresso"))
                {
                    //se era acceso, spegni
                    if (onIngresso)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, -15);
                        lampadinaIngresso.GetComponent<Light>().enabled = false;
                        onIngresso = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(lampadinaIngresso.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd); 
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onIngresso = true;
                        lampadinaIngresso.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, 15);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(lampadinaIngresso.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul - per diminuire la potenza della luce di ingresso, e la luce è accesa
                if(rayCastHit.collider.name.Equals("Button Dimmer Ingresso -") && onIngresso)
                {
                    //non puoi andare oltre lo 0
                    if(dimmerIntensityIngresso>=2.5f)
                    {
                        dimmerIntensityIngresso -= 2.5f;
                        dimmerIngresso.text = "" + dimmerIntensityIngresso*10;
                        lampadinaIngresso.GetComponent<Light>().intensity = dimmerIntensityIngresso;
                        cmd.action = ""+dimmerIntensityIngresso*10;
                        cmd.id = Int32.Parse(lampadinaIngresso.tag);
                        cmd.component = "l";
                        cmd.cmd = "power";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per aumentare la potenza della luce di ingresso, e la luce è accesa
                if (rayCastHit.collider.name.Equals("Button Dimmer Ingresso +") && onIngresso)
                {
                    //non puoi andare oltre il 10
                    if (dimmerIntensityIngresso <= 7.5f)
                    {
                        dimmerIntensityIngresso += 2.5f;
                        dimmerIngresso.text = "" + dimmerIntensityIngresso*10;
                        lampadinaIngresso.GetComponent<Light>().intensity = dimmerIntensityIngresso;
                        cmd.action = "" + dimmerIntensityIngresso*10;
                        cmd.id = Int32.Parse(lampadinaIngresso.tag);
                        cmd.component = "l";
                        cmd.cmd = "power";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Interruttore bagno
                if (rayCastHit.collider.name.Equals("Interruttore Bagno"))
                {
                    //se era acceso, spegni
                    if (onBagno)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(-15, 0, 0);
                        lampadinaBagno.GetComponent<Light>().enabled = false;
                        onBagno = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(lampadinaBagno.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onBagno = true;
                        lampadinaBagno.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(15, 0, 0);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(lampadinaBagno.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su faretto bagno
                if (rayCastHit.collider.name.Equals("Interruttore Faretto Bagno"))
                {
                    //se era acceso, spegni
                    if (onFarettoBagno)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, -15);
                        farettoBagno.GetComponent<Light>().enabled = false;
                        onFarettoBagno = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(farettoBagno.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onFarettoBagno = true;
                        farettoBagno.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, 15);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(farettoBagno.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su interruttore cucina
                if (rayCastHit.collider.name.Equals("Interruttore Cucina"))
                {
                    //se era acceso, spegni
                    if (onCucina)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(-15, 0, 0);
                        lampadinaCucina.GetComponent<Light>().enabled = false;
                        onCucina = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(lampadinaCucina.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onCucina = true;
                        lampadinaCucina.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(15, 0, 0);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(lampadinaCucina.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Interruttore Soggiorno
                if (rayCastHit.collider.name.Equals("Interruttore Soggiorno"))
                {
                    //se era acceso, spegni
                    if (onSoggiorno)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, -15);
                        lampadinaSoggiorno.GetComponent<Light>().enabled = false;
                        onSoggiorno = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(lampadinaSoggiorno.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onSoggiorno = true;
                        lampadinaSoggiorno.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, 15);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(lampadinaSoggiorno.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }


                //se hai cliccato sul - per diminuire la potenza della luce del soggiorno, e la luce è accesa
                if (rayCastHit.collider.name.Equals("Button Dimmer Soggiorno -") && onSoggiorno)
                {
                    //non puoi andare oltre lo 0
                    if (dimmerIntensitySoggiorno >= 2.5f)
                    {
                        dimmerIntensitySoggiorno -= 2.5f;
                        dimmerSoggiorno.text = "" + dimmerIntensitySoggiorno * 10;
                        lampadinaSoggiorno.GetComponent<Light>().intensity = dimmerIntensitySoggiorno;
                        cmd.action = "" + dimmerIntensitySoggiorno * 10;
                        cmd.id = Int32.Parse(lampadinaSoggiorno.tag);
                        cmd.component = "l";
                        cmd.cmd = "power";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per aumentare la potenza della luce del soggiorno, e la luce è accesa
                if (rayCastHit.collider.name.Equals("Button Dimmer Soggiorno + ") && onSoggiorno)
                {
                    //non puoi andare oltre il 10
                    if (dimmerIntensitySoggiorno <= 7.5f)
                    {
                        dimmerIntensitySoggiorno += 2.5f;
                        dimmerSoggiorno.text = "" + dimmerIntensitySoggiorno * 10;
                        lampadinaSoggiorno.GetComponent<Light>().intensity = dimmerIntensitySoggiorno;
                        cmd.action = "" + dimmerIntensitySoggiorno * 10;
                        cmd.id = Int32.Parse(lampadinaSoggiorno.tag);
                        cmd.component = "l";
                        cmd.cmd = "power";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Interruttore Piantana Soggiorno
                if (rayCastHit.collider.name.Equals("Interruttore Piantana Soggiorno"))
                {
                    //se era acceso, spegni
                    if (onPiantanaSoggiorno)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, -15);
                        piantanaSoggiorno.GetComponent<Light>().enabled = false;
                        onPiantanaSoggiorno = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(piantanaSoggiorno.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onPiantanaSoggiorno = true;
                        piantanaSoggiorno.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, 15);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(piantanaSoggiorno.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Interruttore Stanza
                if (rayCastHit.collider.name.Equals("Interruttore Stanza"))
                {
                    //se era acceso, spegni
                    if (onStanza)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, -15);
                        lampadinaStanza.GetComponent<Light>().enabled = false;
                        onStanza = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(lampadinaStanza.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onStanza = true;
                        lampadinaStanza.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, 15);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(lampadinaStanza.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }


                //se hai cliccato sul - per diminuire la potenza della luce della stanza, e la luce è accesa
                if (rayCastHit.collider.name.Equals("Button Dimmer Stanza -") && onStanza)
                {
                    //non puoi andare oltre lo 0
                    if (dimmerIntensityStanza >= 2.5f)
                    {
                        dimmerIntensityStanza -= 2.5f;
                        dimmerStanza.text = "" + dimmerIntensityStanza * 10;
                        lampadinaStanza.GetComponent<Light>().intensity = dimmerIntensityStanza;
                        cmd.action = "" + dimmerIntensityStanza * 10;
                        cmd.id = Int32.Parse(lampadinaStanza.tag);
                        cmd.component = "l";
                        cmd.cmd = "power";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per aumentare la potenza della luce del soggiorno, e la luce è accesa
                if (rayCastHit.collider.name.Equals("Button Dimmer Stanza +") && onStanza)
                {
                    //non puoi andare oltre il 10
                    if (dimmerIntensityStanza <= 7.5f)
                    {
                        dimmerIntensityStanza += 2.5f;
                        dimmerStanza.text = "" + dimmerIntensityStanza * 10;
                        lampadinaStanza.GetComponent<Light>().intensity = dimmerIntensityStanza;
                        cmd.action = "" + dimmerIntensityStanza * 10;
                        cmd.id = Int32.Parse(lampadinaStanza.tag);
                        cmd.component = "l";
                        cmd.cmd = "power";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Interruttore Abat1
                if (rayCastHit.collider.name.Equals("Interruttore Abat1"))
                {
                    //se era acceso, spegni
                    if (onAbat1)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, -15);
                        abat1.GetComponent<Light>().enabled = false;
                        onAbat1 = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(abat1.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onAbat1 = true;
                        abat1.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, 15);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(abat1.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Interruttore Abat2
                if (rayCastHit.collider.name.Equals("Interruttore Abat2"))
                {
                    //se era acceso, spegni
                    if (onAbat2)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, -15);
                        abat2.GetComponent<Light>().enabled = false;
                        onAbat2 = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(abat2.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onAbat2 = true;
                        abat2.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(0, 0, 15);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(abat2.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }





            }
        }
    }

}
