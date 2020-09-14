using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public VideoClip channel1, channel2, channel3, channel4, channel5;
    private VideoClip[] lastChannel;
    public static bool[] isOn = { false, false };
    public static float[] volumes;

    private VideoPlayer playerSoggiorno, playerStanza;

    // private VideoPlayer tv;
    // Start is called before the first frame update
    void Start()
    {
        /*
        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        VA AGGIUSTATO IL MANTENIMENTO DELLO STATO DI SIMULAIONE IN SIMULAZIONE??????????????????'
        */
        //tv = GetComponent<VideoPlayer>();
        //tv.Play();
        GameObject tvSoggiorno = GameObject.FindGameObjectWithTag("20");
        GameObject tvStanza = GameObject.FindGameObjectWithTag("21");
         playerSoggiorno = tvSoggiorno.GetComponent<VideoPlayer>();
         playerStanza = tvStanza.GetComponent<VideoPlayer>();
        lastChannel = new VideoClip[2];
        //soggiorno
        lastChannel[0] = playerSoggiorno.clip;
        //stanza
        lastChannel[1] = playerStanza.clip;


        //    playerSoggiorno.clip = lastChannel[0];
        //    playerStanza.clip = lastChannel[1];
        volumes = new float[2];
        volumes[0] = playerSoggiorno.GetDirectAudioVolume(0);
        volumes[1] = playerStanza.GetDirectAudioVolume(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchTV(int id, string action)
    {
        print("switch tv");
        GameObject tv = GameObject.FindGameObjectWithTag(id.ToString());
        VideoPlayer player = tv.GetComponent<VideoPlayer>();
        //int per capire di quale tv bisogna riprendere lo stato
        int room;
        //0 per la tv del soggiorno, 1 per quella della stanza
        if (id == 20) room = 0;
        else room = 1;
        if (action.Equals("true"))
        {
            isOn[room] = true;
            player.clip = lastChannel[room];
            player.Play();
        }

        if (action.Equals("false"))
        {
            isOn[room] = false;
            player.clip = lastChannel[room];
            player.Stop();
        }

    }

    public void switchChannel(int id, int action)
    {
        GameObject tv = GameObject.FindGameObjectWithTag(id.ToString());
        VideoPlayer player = tv.GetComponent<VideoPlayer>();
        //int per capire di quale tv bisogna salvare lo stato
        int room;
        if (id == 20) room = 0;
        else room = 1;
        if (player.isPlaying)
        {
            //print("OK: CANALE" + action);
            if (action == 1)
            {
                player.clip = channel1;
                player.Play();
                lastChannel[room] = channel1;


            }
            if (action == 2)
            {
                player.clip = channel2;
                player.Play();
                lastChannel[room] = channel2;


            }
            if (action == 3)
            {
                player.clip = channel3;
                player.Play();
                lastChannel[room] = channel3;


            }
            if (action == 4)
            {
                player.clip = channel4;
                player.Play();
                lastChannel[room] = channel4;


            }

            if (action == 5)
            {
                player.clip = channel5;
                player.Play();
                lastChannel[room] = channel5;



            }
        }

    }

    public void changeVolume(int id, int action)
    {
        GameObject tv = GameObject.FindGameObjectWithTag(id.ToString());
        VideoPlayer player = tv.GetComponent<VideoPlayer>();

        //print("BOH" + player.audioTrackCount);
        player.SetDirectAudioVolume( 0, action/10f);
        volumes[0] = playerSoggiorno.GetDirectAudioVolume(0);
        volumes[1] = playerStanza.GetDirectAudioVolume(0);


    }
}
