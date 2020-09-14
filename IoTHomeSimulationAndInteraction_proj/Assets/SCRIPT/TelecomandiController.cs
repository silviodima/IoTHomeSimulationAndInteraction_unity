using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TelecomandiController : MonoBehaviour
{
    private GameObject mqtt, tvSoggiorno, tvStanza;
    private VideoPlayer playerSoggiorno, playerStanza;
    private VideoClip[] lastChannel;

    public VideoClip channel1, channel2, channel3, channel4, channel5;

    private bool onTVSogg, onTVStanza;
    private bool muteSogg, muteStanza;


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
        mqtt = GameObject.FindGameObjectWithTag("mqtt");

        //booleani che tengono conto dello stato della tv: false spenta, true accesa
        onTVSogg = false;
        onTVStanza = false;

        //booleani che tengono conto dello stato del pulsante di muto: al primo click setta il volume a 0, al secondo riporta il volume al valore precedente
        muteSogg = false;
        muteStanza = false;

        tvSoggiorno = GameObject.FindGameObjectWithTag("20");
        tvStanza = GameObject.FindGameObjectWithTag("21");
        playerSoggiorno = tvSoggiorno.GetComponent<VideoPlayer>();
        playerStanza = tvStanza.GetComponent<VideoPlayer>();

        lastChannel = new VideoClip[2];
        //soggiorno
        lastChannel[0] = playerSoggiorno.clip;
        //stanza
        lastChannel[1] = playerStanza.clip;

    }

    // Update is called once per frame
    void Update()
    {
        //se la tv del soggiorno è accesa
        if (TVController.isOn[0])
            onTVSogg = true;

        //se la tv della stanza è accesa
        if (TVController.isOn[1])
            onTVStanza = true;

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
                //se hai cliccato sul pulsante per accendee la tv del soggiorno
                if (rayCastHit.collider.name.Equals("Pulsante TV Sogg ON/OFF"))
                {
                    //se era acceso, spegni
                    if (onTVSogg)
                    {
                        TVController.isOn[0] = false;
                        playerSoggiorno.clip = lastChannel[0];
                        playerSoggiorno.Stop();
                        onTVSogg = false;
                        cmd.action = "false";
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }

                    //se era spento, accendi
                    else
                    {
                        onTVSogg = true;
                        TVController.isOn[0] = true;
                        playerSoggiorno.clip = lastChannel[0];
                        playerSoggiorno.Play();
                        cmd.action = "true";
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "switch";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul canale 1 del telecomando del soggiorno
                if (rayCastHit.collider.name.Equals("Soggiorno canale 1"))
                {
                    //se la tv è accesa
                    if (playerSoggiorno.isPlaying)
                    {
                        playerSoggiorno.clip = channel1;
                        playerSoggiorno.Play();
                        lastChannel[0] = channel1;
                        cmd.action = "1";
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "channel";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul canale 2 del telecomando del soggiorno
                if (rayCastHit.collider.name.Equals("Soggiorno canale 2"))
                {
                    //se la tv è accesa
                    if (playerSoggiorno.isPlaying)
                    {
                        playerSoggiorno.clip = channel2;
                        playerSoggiorno.Play();
                        lastChannel[0] = channel2;
                        cmd.action = "2";
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "channel";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul canale 3 del telecomando del soggiorno
                if (rayCastHit.collider.name.Equals("Soggiorno canale 3"))
                {
                    //se la tv è accesa
                    if (playerSoggiorno.isPlaying)
                    {
                        playerSoggiorno.clip = channel3;
                        playerSoggiorno.Play();
                        lastChannel[0] = channel3;
                        cmd.action = "3";
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "channel";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul canale 4 del telecomando del soggiorno
                if (rayCastHit.collider.name.Equals("Soggiorno canale 4"))
                {
                    //se la tv è accesa
                    if (playerSoggiorno.isPlaying)
                    {
                        playerSoggiorno.clip = channel4;
                        playerSoggiorno.Play();
                        lastChannel[0] = channel4;
                        cmd.action = "4";
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "channel";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul canale 5 del telecomando del soggiorno
                if (rayCastHit.collider.name.Equals("Soggiorno canale 5"))
                {
                    //se la tv è accesa
                    if (playerSoggiorno.isPlaying)
                    {
                        playerSoggiorno.clip = channel5;
                        playerSoggiorno.Play();
                        lastChannel[0] = channel5;
                        cmd.action = "5";
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "channel";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }


                //se hai cliccato sul + per alzare il volume
                if (rayCastHit.collider.name.Equals("Volume TV Sogg +"))
                {
                    //se la tv è accesa e il volume non è al massimo
                    if (playerSoggiorno.isPlaying && TVController.volumes[0]<1)
                    {
                        print("Il volume è"+TVController.volumes[0]);
                        playerSoggiorno.SetDirectAudioVolume(0, TVController.volumes[0] +0.1f);
                        TVController.volumes[0] += 0.1f;
                        cmd.action = ""+ Math.Round(TVController.volumes[0]*10);
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "volume";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul - per alzare il volume
                if (rayCastHit.collider.name.Equals("Volume TV Sogg -"))
                {
                    //se la tv è accesa e il volume non è al massimo
                    if (playerSoggiorno.isPlaying && TVController.volumes[0] >= 0.1f)
                    {
                        playerSoggiorno.SetDirectAudioVolume(0, TVController.volumes[0] - 0.1f);
                        TVController.volumes[0] -= 0.1f;
                        cmd.action = "" + Math.Round(TVController.volumes[0]*10);
                        cmd.id = Int32.Parse(tvSoggiorno.tag);
                        cmd.component = "t";
                        cmd.cmd = "volume";

                        string toJson = JsonUtility.ToJson(cmd);
                        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                    }
                }

                //se hai cliccato sul pulsante per mutare e non hai già mutato la tv
                if (rayCastHit.collider.name.Equals("Muto TV Sogg"))
                {
                    //se la tv è accesa
                    if (playerSoggiorno.isPlaying)
                    {
                        if (!muteSogg)
                        {
                            muteSogg = true;
                            playerSoggiorno.SetDirectAudioVolume(0, 0);
                            cmd.action = "" + 0;
                            cmd.id = Int32.Parse(tvSoggiorno.tag);
                            cmd.component = "t";
                            cmd.cmd = "volume";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }

                        else
                        {
                            muteSogg = false;
                            playerSoggiorno.SetDirectAudioVolume(0, TVController.volumes[0]);
                            cmd.action = "" + TVController.volumes[0] * 10;
                            cmd.id = Int32.Parse(tvSoggiorno.tag);
                            cmd.component = "t";
                            cmd.cmd = "volume";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }

                }




                    //se hai cliccato sul pulsante per accendee la tv della stanza
                    if (rayCastHit.collider.name.Equals("Pulsante TV Stanza ON/OFF"))
                    {
                        //se era acceso, spegni
                        if (onTVStanza)
                        {
                            TVController.isOn[1] = false;
                            playerStanza.clip = lastChannel[1];
                            playerStanza.Stop();
                            onTVStanza = false;
                            cmd.action = "false";
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "switch";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }

                        //se era spento, accendi
                        else
                        {
                            onTVStanza = true;
                            TVController.isOn[1] = true;
                            playerStanza.clip = lastChannel[1];
                            playerStanza.Play();
                            cmd.action = "true";
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "switch";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }

                //se hai cliccato sul canale 1 del telecomando della Stanza
                if (rayCastHit.collider.name.Equals("Stanza canale 1"))
                    {
                        //se la tv è accesa
                        if (playerStanza.isPlaying)
                        {
                            playerStanza.clip = channel1;
                            playerStanza.Play();
                            lastChannel[1] = channel1;
                            cmd.action = "1";
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "channel";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }

                    //se hai cliccato sul canale 2 del telecomando della stanza
                    if (rayCastHit.collider.name.Equals("Stanza canale 2"))
                    {
                        //se la tv è accesa
                        if (playerStanza.isPlaying)
                        {
                            playerStanza.clip = channel2;
                            playerStanza.Play();
                            lastChannel[1] = channel2;
                            cmd.action = "2";
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "channel";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }

                    //se hai cliccato sul canale 3 del telecomando della stanza
                    if (rayCastHit.collider.name.Equals("Stanza canale 3"))
                    {
                        //se la tv è accesa
                        if (playerStanza.isPlaying)
                        {
                            playerStanza.clip = channel3;
                            playerStanza.Play();
                            lastChannel[1] = channel3;
                            cmd.action = "3";
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "channel";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }

                    //se hai cliccato sul canale 4 del telecomando della stanza
                    if (rayCastHit.collider.name.Equals("Stanza canale 4"))
                    {
                        //se la tv è accesa
                        if (playerStanza.isPlaying)
                        {
                            playerStanza.clip = channel4;
                            playerStanza.Play();
                            lastChannel[1] = channel4;
                            cmd.action = "4";
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "channel";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }

                    //se hai cliccato sul canale 5 del telecomando della stanza
                    if (rayCastHit.collider.name.Equals("Stanza canale 5"))
                    {
                        //se la tv è accesa
                        if (playerStanza.isPlaying)
                        {
                            playerStanza.clip = channel5;
                            playerStanza.Play();
                            lastChannel[1] = channel5;
                            cmd.action = "5";
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "channel";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }


                    //se hai cliccato sul + per alzare il volume
                    if (rayCastHit.collider.name.Equals("Volume TV Stanza +"))
                    {
                        //se la tv è accesa e il volume non è al massimo
                        if (playerStanza.isPlaying && TVController.volumes[1] < 1)
                        {
                            playerStanza.SetDirectAudioVolume(0, TVController.volumes[1] + 0.1f);
                            TVController.volumes[1] += 0.1f;
                            cmd.action = "" + Math.Round(TVController.volumes[1] * 10);
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "volume";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }

                    //se hai cliccato sul - per alzare il volume
                    if (rayCastHit.collider.name.Equals("Volume TV Stanza -"))
                    {
                        //se la tv è accesa e il volume non è al massimo
                        if (playerStanza.isPlaying && TVController.volumes[1] >= 0.1f)
                        {
                            playerStanza.SetDirectAudioVolume(0, TVController.volumes[1] - 0.1f);
                            TVController.volumes[1] -= 0.1f;
                            cmd.action = "" + Math.Round(TVController.volumes[1] * 10);
                            cmd.id = Int32.Parse(tvStanza.tag);
                            cmd.component = "t";
                            cmd.cmd = "volume";

                            string toJson = JsonUtility.ToJson(cmd);
                            mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                        }
                    }

                    //se hai cliccato sul pulsante per mutare e non hai già mutato la tv
                    if (rayCastHit.collider.name.Equals("Muto TV Stanza"))
                    {
                        //se la tv è accesa
                        if (playerStanza.isPlaying)
                        {
                            if (!muteStanza)
                            {
                                muteStanza = true;
                                playerStanza.SetDirectAudioVolume(0, 0);
                                cmd.action = "" + 0;
                                cmd.id = Int32.Parse(tvStanza.tag);
                                cmd.component = "t";
                                cmd.cmd = "volume";

                                string toJson = JsonUtility.ToJson(cmd);
                                mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                            }

                            else
                            {
                                muteStanza = false;
                                playerStanza.SetDirectAudioVolume(0, TVController.volumes[1]);
                                cmd.action = "" + TVController.volumes[1] * 10;
                                cmd.id = Int32.Parse(tvStanza.tag);
                                cmd.component = "t";
                                cmd.cmd = "volume";

                                string toJson = JsonUtility.ToJson(cmd);
                                mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                            }
                        }
                    }

                ////se hai cliccato sul pulsante per mutare e al momento la tv è muta
                //if (rayCastHit.collider.name.Equals("Muto TV Sogg") && muteSogg)
                //{
                //    //se la tv è accesa
                //    if (playerSoggiorno.isPlaying)
                //    {
                //        muteSogg = false;
                //        playerSoggiorno.SetDirectAudioVolume(0, TVController.volumes[0]);
                //        cmd.action = "" + TVController.volumes[0] * 10;
                //        cmd.id = Int32.Parse(tvSoggiorno.tag);
                //        cmd.component = "t";
                //        cmd.cmd = "volume";

                //        string toJson = JsonUtility.ToJson(cmd);
                //        mqtt.GetComponent<MqttController>().Publish("unity", toJson);
                //    }
                //}


            }
        }
    }
}
