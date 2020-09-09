using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindController : MonoBehaviour
{
    // Start is called before the first frame update
    //Slider nell'inspector per settare quanto abbassare le persiane 
    /*[Range(1.0f, 10.0f)]
    public float blindBathroom, blindKitchen, blindRoom, blindLounge;*/
    //lunghezza che dovrà avere la persiana, lunghezza iniziale, lunghezza massima che può avere
    private float[] finalScale, initialScale;
    //le costanti vengono ottenute dall'equazione: scale.y iniziale + percentuale*x = scale.y ideale per chiudere al massimo la persiana
    public static readonly float[] blindLength = { 0.060f, 0.060f, 0.060f, 0.090f, 0.090f };
    private GameObject[] rooms;
    private float temperature;
    private Time time;
    private int id_cmd, action_cmd;
    public static bool call;
    void Start()
    {
        call = false;
        rooms = new GameObject[5];
        //vanno riassegnati gli id
        rooms[0] = GameObject.FindGameObjectWithTag("12");
        rooms[1] = GameObject.FindGameObjectWithTag("13");
        rooms[2] = GameObject.FindGameObjectWithTag("22");
        rooms[3] = GameObject.FindGameObjectWithTag("10");
        rooms[4] = GameObject.FindGameObjectWithTag("11");

        initialScale = new float[5];
        initialScale[0] = rooms[0].transform.localScale.y;
        initialScale[1] = rooms[1].transform.localScale.y;
        initialScale[2] = rooms[2].transform.localScale.y;
        initialScale[3] = rooms[3].transform.localScale.y;
        initialScale[4] = rooms[4].transform.localScale.y;


        //finalScale[i] rappresenta la scale.y dell'i-esimo gameObject rintracciato
        //la costante moltiplicativa x viene ottenuta dall'equazione: scale.y iniziale + valore slider*x = scale.y ideale per chiudere al massimo la persiana





    }

    // Update is called once per frame
    void Update()
     {
        if(call)
        move(id_cmd, action_cmd);
          /*finalScale[0] = initialScale[0] + blindBathroom * 0.57f;
          finalScale[1] = initialScale[1] + blindKitchen * 0.57f;
          finalScale[2] = initialScale[2] + blindRoom * 0.57f;
          finalScale[3] = initialScale[3] + blindLounge * 0.85f;
          updateBlind();
          print("Bagno: " + blindBathroom);
          print("Cucina: " + blindKitchen);
          print("Stanza: " + blindRoom);
          print("Lounge: " + blindLounge);*/
     }
          /*
     public void updateBlind()
     {
         for(int i=0;i<4;i++)
         {
             if (rooms[i].transform.localScale.y <finalScale[i])
                 blindDown(i);

             else
             {
                blindUp(i);
             }
         }
     }

     public void blindDown(int i)
     {
         rooms[i].transform.position = new Vector3(rooms[i].transform.position.x, rooms[i].transform.position.y - 0.001f, rooms[i].transform.position.z);
         rooms[i].transform.localScale = new Vector3(rooms[i].transform.localScale.x, rooms[i].transform.localScale.y + 0.002f, rooms[i].transform.localScale.z);
     }

     public void blindUp(int i)
     {
         rooms[i].transform.position = new Vector3(rooms[i].transform.position.x, rooms[i].transform.position.y + 0.001f, rooms[i].transform.position.z);
         rooms[i].transform.localScale = new Vector3(rooms[i].transform.localScale.x, rooms[i].transform.localScale.y - 0.002f, rooms[i].transform.localScale.z);

     }*/

    public void move(int id, int length)
    {
        id_cmd = id;
        action_cmd = length;
        call = true;

        GameObject blind = GameObject.FindGameObjectWithTag(id.ToString());
        float firstScale = 0f, constLenght = 0f;
        for(int i =0;i<rooms.Length;i++)
        {
            if(rooms[i].CompareTag(blind.tag))
            {
                firstScale = initialScale[i];
                constLenght = blindLength[i];
            }
        }

        if (blind.transform.localScale.y < firstScale + length * constLenght)
        {
            blind.transform.position = new Vector3(blind.transform.position.x, blind.transform.position.y - 0.002f, blind.transform.position.z);
            blind.transform.localScale = new Vector3(blind.transform.localScale.x, blind.transform.localScale.y + 0.004f, blind.transform.localScale.z);
        }

        else if (blind.transform.localScale.y > firstScale + length * constLenght)

        {
            blind.transform.position = new Vector3(blind.transform.position.x, blind.transform.position.y + 0.002f, blind.transform.position.z);
            blind.transform.localScale = new Vector3(blind.transform.localScale.x, blind.transform.localScale.y - 0.004f, blind.transform.localScale.z);
        }

        else
        {
            print("false"); call = false;
        }

    }

}
