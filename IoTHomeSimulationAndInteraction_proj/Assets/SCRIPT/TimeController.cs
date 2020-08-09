using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    Text textTime;
    float timer;
    double hour, minutes;
    // Start is called before the first frame update
    void Start()
    {
        hour = 6;
        minutes = 0;
        textTime = GetComponent<Text>();
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        /*   timer = Time.time;
           /* if (minutes < 60)
            {
                minutes++;
                updateHour();
            }

            if(minutes == 59)
            {
                hour++;
                minutes = 0;
                updateHour();
            }

            if (hour==23 && minutes ==59)
            {
                hour = 0;
                minutes = 00;
                updateHour();
            }
           if(timer<6)
           print("" + timer);

           if (timer>6)
           {
               timer = 0f;
               hour++;
               updateHour();
           }
           */
        float t = Time.time - timer;

        String minutes = ((int)t / 60).ToString();
        String seconds = (t % 60).ToString("f2");

        if (Int16.Parse(seconds) == 7)
            print("mammt");
        textTime.text = hour + ":" + minutes;
    }

  /*  void updateHour()
    {
        if (hour < 10)
        {
            if (minutes < 10)
                time.text = "Ora:0" + hour + ".0" + minutes;

            else time.text = "Ora:0" + hour + "." + minutes;

        }

        else
        {
            if (minutes < 10)
                time.text = "Ora:" + hour + ".0" + minutes;

            else time.text = "Ora:" + hour + "." + minutes;

        }

        if (hour == 23 || minutes == 59)
        {
            hour = 0;
            minutes = 00;
            updateHour();
        }
    }*/
}
