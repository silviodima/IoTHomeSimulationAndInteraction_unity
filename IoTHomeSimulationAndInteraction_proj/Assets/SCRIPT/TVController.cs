using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public VideoClip channel1, channel2, channel3, channel4;
   // private VideoPlayer tv;
    // Start is called before the first frame update
    void Start()
    {
        //tv = GetComponent<VideoPlayer>();
      //tv.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchTV(int id, string action)
    {
        GameObject tv = GameObject.FindGameObjectWithTag(id.ToString());
        if (action.Equals("on"))
        {
            tv.GetComponent<VideoPlayer>().Play();
        }

        if (action.Equals("off"))
        {
            tv.GetComponent<VideoPlayer>().Stop();
        }

    }

    public void switchChannel(int id, int action)
    {
        GameObject tv = GameObject.FindGameObjectWithTag(id.ToString());
        VideoPlayer player = tv.GetComponent<VideoPlayer>();
        if (player.isPlaying)
        {
            print("OK: CANALE" + action);
            if (action == 1)
            {
                player.clip = channel1;
                player.Play();


            }
            if (action == 2)
            {
                player.clip = channel2;
                player.Play();


            }
            if (action == 3)
            {
                player.clip = channel3;
                player.Play();


            }
            if (action == 4)
            {
                player.clip = channel4;
                player.Play();


            }
        }

    }

    public void changeVolume(int id, int action)
    {
        GameObject tv = GameObject.FindGameObjectWithTag(id.ToString());
        VideoPlayer player = tv.GetComponent<VideoPlayer>();

        print("BOH" + player.audioTrackCount);
        player.SetDirectAudioVolume( 0, action/10f);



    }
}
