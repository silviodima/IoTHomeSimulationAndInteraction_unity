using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterrruttoriPersianaController : MonoBehaviour
{
    private GameObject mqtt, blindController;
    private GameObject[] persiane;
    private Text[] textPersiane;
    private int [] lunghezzePersiane;

    public GameObject persianaCucina, persianaBagno, persianaStanza, persianaSoggiorno, persianaSoggiorno2;
    public Text cucina, bagno, stanza, soggiorno, soggiorno2;
    private int lunghezzaPersianaCucina, lunghezzaPersianaBagno, lunghezzaPersianaStanza, lunghezzaPersianaSoggiorno, lunghezzaPersianaSoggiorno2;

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
        persiane = new GameObject[5];
        persiane[0] = persianaSoggiorno;
        persiane[1] = persianaSoggiorno2;
        persiane[2] = persianaStanza;
        persiane[3] = persianaCucina;
        persiane[4] = persianaBagno;

        textPersiane = new Text[5];
        textPersiane[0] = soggiorno;
        textPersiane[1] = soggiorno2;
        textPersiane[2] = stanza;
        textPersiane[3] = cucina;
        textPersiane[4] = bagno;

        lunghezzePersiane = new int[5];
        lunghezzePersiane[0] = lunghezzaPersianaSoggiorno;
        lunghezzePersiane[1] = lunghezzaPersianaSoggiorno2;
        lunghezzePersiane[2] = lunghezzaPersianaStanza;
        lunghezzePersiane[3] = lunghezzaPersianaCucina;
        lunghezzePersiane[4] = lunghezzaPersianaBagno;

        blindController = GameObject.FindGameObjectWithTag("blindController");

        mqtt = GameObject.FindGameObjectWithTag("mqtt");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 2; i < persiane.Length; i++)
        {
            if (persiane[i].transform.position.y > 9.31)
            {
                lunghezzePersiane[i] = 0;
                textPersiane[i].text = " " + lunghezzePersiane[i] + " ";
            }
            if (persiane[i].transform.position.y < 9.31 && persiane[i].transform.position.y > 8.55)
            {
                lunghezzePersiane[i] = 25;
                textPersiane[i].text = " " + lunghezzePersiane[i] + " ";
            }
            if (persiane[i].transform.position.y < 8.55 && persiane[i].transform.position.y > 7.80)
            {
                lunghezzePersiane[i] = 50;
                textPersiane[i].text = " " + lunghezzePersiane[i] + " ";
            }
            if (persiane[i].transform.position.y < 7.80 && persiane[i].transform.position.y > 7.05)
            {
                lunghezzePersiane[i] = 75;
                textPersiane[i].text = " " + lunghezzePersiane[i] + " ";
            }
            if (persiane[i].transform.position.y < 7.05 && persiane[i].transform.position.y > 6.30)
            {
                lunghezzePersiane[i] = 100;
                textPersiane[i].text = "" + lunghezzePersiane[i] + "";
            }
        }

        for (int i = 0; i < 2; i++)
        {
            if (persiane[i].transform.position.y > 9.30)
            {
                lunghezzePersiane[i] = 0;
                textPersiane[i].text = " " + lunghezzePersiane[i] + " ";
            }
            if (persiane[i].transform.position.y < 9.30 && persiane[i].transform.position.y > 8.18)
            {
                lunghezzePersiane[i] = 25;
                textPersiane[i].text = " " + lunghezzePersiane[i] + " ";
            }
            if (persiane[i].transform.position.y < 8.18 && persiane[i].transform.position.y > 7.05)
            {
                lunghezzePersiane[i] = 50;
                textPersiane[i].text = " " + lunghezzePersiane[i] + " ";
            }
            if (persiane[i].transform.position.y < 7.05 && persiane[i].transform.position.y > 5.93)
            {
                lunghezzePersiane[i] = 75;
                textPersiane[i].text = " " + lunghezzePersiane[i] + " ";
            }
            if (persiane[i].transform.position.y < 5.93 && persiane[i].transform.position.y > 4.80)
            {
                lunghezzePersiane[i] = 100;
                textPersiane[i].text = "" + lunghezzePersiane[i] + "";
            }
        }



        Command cmd = new Command();
        cmd.type = 0;
        cmd.component = "p";
        cmd.cmd = "move";

        //Check if Mouse Button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            //Raycast from mouse cursor pos
            RaycastHit rayCastHit;
            Ray rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayCast, out rayCastHit))
            {
                //se hai cliccato sul - per abbassare la persiana della cucina
                if (rayCastHit.collider.name.Equals("Button Persiana Cucina -"))
                {
                    //non puoi andare oltre lo 0
                    if (lunghezzaPersianaCucina >= 25)
                    {
                        lunghezzaPersianaCucina -= 25;
                        cucina.text = "" + lunghezzaPersianaCucina;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaCucina.tag), lunghezzaPersianaCucina);
                        cmd.action = "" + lunghezzaPersianaCucina;
                        cmd.id = Int32.Parse(persianaCucina.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per alzare la persiana della cucina
                if (rayCastHit.collider.name.Equals("Button Persiana Cucina +"))
                {
                    //non puoi andare oltre il 10
                    if (lunghezzaPersianaCucina <= 75)
                    {
                        lunghezzaPersianaCucina += 25;
                        cucina.text = "" + lunghezzaPersianaCucina;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaCucina.tag), lunghezzaPersianaCucina);
                        cmd.action = "" + lunghezzaPersianaCucina;
                        cmd.id = Int32.Parse(persianaCucina.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }



                //se hai cliccato sul - per abbassare la persiana del bagno
                if (rayCastHit.collider.name.Equals("Button Persiana Bagno -"))
                {
                    //non puoi andare oltre lo 0
                    if (lunghezzaPersianaBagno >= 25)
                    {
                        lunghezzaPersianaBagno -= 25;
                        bagno.text = "" + lunghezzaPersianaBagno;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaBagno.tag), lunghezzaPersianaBagno);
                        cmd.action = "" + lunghezzaPersianaBagno;
                        cmd.id = Int32.Parse(persianaBagno.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per alzare la persiana del bagno
                if (rayCastHit.collider.name.Equals("Button Persiana Bagno +"))
                {
                    //non puoi andare oltre il 10
                    if (lunghezzaPersianaBagno <= 75)
                    {
                        lunghezzaPersianaBagno += 25;
                        bagno.text = "" + lunghezzaPersianaBagno;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaBagno.tag), lunghezzaPersianaBagno);
                        cmd.action = "" + lunghezzaPersianaBagno;
                        cmd.id = Int32.Parse(persianaBagno.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }


                //se hai cliccato sul - per abbassare la persiana della stanza
                if (rayCastHit.collider.name.Equals("Button Persiana Stanza -"))
                {
                    //non puoi andare oltre lo 0
                    if (lunghezzaPersianaStanza >= 25)
                    {
                        lunghezzaPersianaStanza -= 25;
                        stanza.text = "" + lunghezzaPersianaStanza;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaStanza.tag), lunghezzaPersianaStanza);
                        cmd.action = "" + lunghezzaPersianaStanza;
                        cmd.id = Int32.Parse(persianaStanza.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per alzare la 1 persiana della stanza
                if (rayCastHit.collider.name.Equals("Button Persiana Stanza +"))
                {
                    //non puoi andare oltre il 10
                    if (lunghezzaPersianaStanza <= 75)
                    {
                        lunghezzaPersianaStanza += 25;
                        stanza.text = "" + lunghezzaPersianaStanza;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaStanza.tag), lunghezzaPersianaStanza);
                        cmd.action = "" + lunghezzaPersianaStanza;
                        cmd.id = Int32.Parse(persianaStanza.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul - per abbassare la persiana 1 del soggiorno
                if (rayCastHit.collider.name.Equals("Button Persiana Soggiorno -"))
                {
                    //non puoi andare oltre lo 0
                    if (lunghezzaPersianaSoggiorno >= 25)
                    {
                        lunghezzaPersianaSoggiorno -= 25;
                        soggiorno.text = "" + lunghezzaPersianaSoggiorno;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaSoggiorno.tag), lunghezzaPersianaSoggiorno);
                        cmd.action = "" + lunghezzaPersianaSoggiorno;
                        cmd.id = Int32.Parse(persianaSoggiorno.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per alzare la persiana 1 del soggiorno
                if (rayCastHit.collider.name.Equals("Button Persiana Soggiorno +"))
                {
                    //non puoi andare oltre il 10
                    if (lunghezzaPersianaSoggiorno <= 75)
                    {
                        lunghezzaPersianaSoggiorno += 25;
                        soggiorno.text = "" + lunghezzaPersianaSoggiorno;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaSoggiorno.tag), lunghezzaPersianaSoggiorno);
                        cmd.action = "" + lunghezzaPersianaSoggiorno;
                        cmd.id = Int32.Parse(persianaSoggiorno.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul - per abbassare la persiana 2 del soggiorno
                if (rayCastHit.collider.name.Equals("Button Persiana Soggiorno 2 -"))
                {
                    //non puoi andare oltre lo 0
                    if (lunghezzaPersianaSoggiorno2 >= 25)
                    {
                        lunghezzaPersianaSoggiorno2 -= 25;
                        soggiorno2.text = "" + lunghezzaPersianaSoggiorno2;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaSoggiorno2.tag), lunghezzaPersianaSoggiorno2);
                        cmd.action = "" + lunghezzaPersianaSoggiorno2;
                        cmd.id = Int32.Parse(persianaSoggiorno2.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul + per alzare la persiana 1 del soggiorno
                if (rayCastHit.collider.name.Equals("Button Persiana Soggiorno 2 +"))
                {
                    //non puoi andare oltre il 10
                    if (lunghezzaPersianaSoggiorno2 <= 75)
                    {
                        lunghezzaPersianaSoggiorno2 += 25;
                        soggiorno2.text = "" + lunghezzaPersianaSoggiorno2;
                        blindController.GetComponent<BlindController>().move(Int32.Parse(persianaSoggiorno2.tag), lunghezzaPersianaSoggiorno2);
                        cmd.action = "" + lunghezzaPersianaSoggiorno2;
                        cmd.id = Int32.Parse(persianaSoggiorno2.tag);

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }
            }
        }
    }
}
