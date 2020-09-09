using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManopoleController : MonoBehaviour
{
    private GameObject mqtt;

    private bool onFornello1, onFornello2, onFornello3, onFornello4;
    public GameObject fornello1, fornello2, fornello3, fornello4;

    [Serializable]
    public class Command
    {
        public int type;
        public int id;
        public string cmd, component, action;
    }

    private void Start()
    {
        if (fornello1.GetComponent<Light>().enabled)
            onFornello1 = true;
        else if (!fornello1.GetComponent<Light>().enabled)
            onFornello1 = false;


        if (fornello2.GetComponent<Light>().enabled)
            onFornello2 = true;
        else if (!fornello2.GetComponent<Light>().enabled)
            onFornello2 = false;

        if (fornello3.GetComponent<Light>().enabled)
            onFornello3 = true;
        else if (!fornello3.GetComponent<Light>().enabled)
            onFornello3 = false;


        if (fornello4.GetComponent<Light>().enabled)
            onFornello4 = true;
        else if (!fornello4.GetComponent<Light>().enabled)
            onFornello4 = false;

        mqtt = GameObject.FindGameObjectWithTag("mqtt");

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
            print(""+rayCast);
            if (Physics.Raycast(rayCast, out rayCastHit))
            {
                //se hai cliccato su Manopola fornello 1
                if (rayCastHit.collider.name.Equals("Manopola Fornello 1"))
                {
                    //se era acceso, spegni
                    if (onFornello1)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(-30, 0, -0);
                        fornello1.GetComponent<Light>().enabled = false;
                        onFornello1 = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(fornello1.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onFornello1 = true;
                        fornello1.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(30, 0, 0);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(fornello1.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Manopola fornello 2
                if (rayCastHit.collider.name.Equals("Manopola Fornello 2"))
                {
                    //se era acceso, spegni
                    if (onFornello2)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(-30, 0, -0);
                        fornello2.GetComponent<Light>().enabled = false;
                        onFornello2 = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(fornello1.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onFornello2 = true;
                        fornello2.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(30, 0, 0);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(fornello2.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Manopola fornello 3
                if (rayCastHit.collider.name.Equals(" Manopola Fornello 3"))
                {
                    //se era acceso, spegni
                    if (onFornello3)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(-30, 0, -0);
                        fornello3.GetComponent<Light>().enabled = false;
                        onFornello3 = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(fornello3.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onFornello3 = true;
                        fornello3.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(30, 0, 0);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(fornello3.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato su Manopola fornello 4
                if (rayCastHit.collider.name.Equals("Manopola Fornello 4"))
                {
                    //se era acceso, spegni
                    if (onFornello4)
                    {
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(-30, 0, -0);
                        fornello4.GetComponent<Light>().enabled = false;
                        onFornello4 = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(fornello4.tag);
                        cmd.component = "l";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onFornello4 = true;
                        fornello4.GetComponent<Light>().enabled = true;
                        rayCastHit.collider.transform.rotation = Quaternion.Euler(30, 0, 0);
                        cmd.action = "true";
                        cmd.id = Int32.Parse(fornello4.tag);
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
