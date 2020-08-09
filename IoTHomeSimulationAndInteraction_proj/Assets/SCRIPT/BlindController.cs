using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindController : MonoBehaviour
{
    // Start is called before the first frame update
    //Slider nell'inspector per settare quanto abbassare le persiane 
    [Range(1.0f, 10.0f)]
    public float blindBathroom, blindKitchen, blindRoom, blindLounge;
    //lunghezza che dovrà avere la persiana, lunghezza iniziale, lunghezza massima che può avere
    private float[] finalScale, initialScale, idealScale, blindLenght;
    private GameObject[] rooms;
    private float temperature;
    private Time time;
    void Start()
    {
        rooms = new GameObject[4];
        rooms[0] = GameObject.FindGameObjectWithTag("blindBath");
        rooms[1] = GameObject.FindGameObjectWithTag("blindKitchen");
        rooms[2] = GameObject.FindGameObjectWithTag("blindRoom");
        rooms[3] = GameObject.FindGameObjectWithTag("blindLounge");

        initialScale = new float[4];
        initialScale[0] = rooms[0].transform.localScale.y;
        initialScale[1] = rooms[1].transform.localScale.y;
        initialScale[2] = rooms[2].transform.localScale.y;
        initialScale[3] = rooms[3].transform.localScale.y;

        //finalScale[i] rappresenta la scale.y dell'i-esimo gameObject rintracciato
        //la costante moltiplicativa x viene ottenuta dall'equazione: scale.y iniziale + valore slider*x = scale.y ideale per chiudere al massimo la persiana
        finalScale = new float[4];
        

       /* idealScale = new float[4];
        idealScale[0] = 7f;
        idealScale[1] = 7f;
        idealScale[2] = 7f;
        idealScale[3] = 9.5f;*/

        blindLenght = new float[4];
        blindLenght[0] = blindBathroom;
        blindLenght[1] = blindKitchen;
        blindLenght[2] = blindRoom;
        blindLenght[3] = blindLounge;




    }

    // Update is called once per frame
    void Update()
    {
        finalScale[0] = initialScale[0] + blindBathroom * 0.57f;
        finalScale[1] = initialScale[1] + blindKitchen * 0.57f;
        finalScale[2] = initialScale[2] + blindRoom * 0.57f;
        finalScale[3] = initialScale[3] + blindLounge * 0.85f;
        updateBlind();
      /*  print("Bagno: " + blindBathroom);
        print("Cucina: " + blindKitchen);
        print("Stanza: " + blindRoom);
        print("Lounge: " + blindLounge);*/
    }

    void updateBlind()
    {
        for(int i=0;i<4;i++)
        {
           //print("" + rooms[i].transform.localScale.y + "," + rooms[i].transform.localScale.y * blindLenght[i]);
            if (rooms[i].transform.localScale.y <finalScale[i])
                blindDown(i);

            else
            {
               blindUp(i);
            }
        }
    }

    void blindDown(int i)
    {
        rooms[i].transform.position = new Vector3(rooms[i].transform.position.x, rooms[i].transform.position.y - 0.001f, rooms[i].transform.position.z);
        rooms[i].transform.localScale = new Vector3(rooms[i].transform.localScale.x, rooms[i].transform.localScale.y + 0.002f, rooms[i].transform.localScale.z);
    }
    
    void blindUp(int i)
    {
        rooms[i].transform.position = new Vector3(rooms[i].transform.position.x, rooms[i].transform.position.y + 0.001f, rooms[i].transform.position.z);
        rooms[i].transform.localScale = new Vector3(rooms[i].transform.localScale.x, rooms[i].transform.localScale.y - 0.002f, rooms[i].transform.localScale.z);

    }
}
