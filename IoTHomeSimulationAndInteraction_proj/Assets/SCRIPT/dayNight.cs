using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNight : MonoBehaviour
{
    public float speed = 100.0f;
    Vector3 angle;
    float rotation = 0f;
    float setup = 0f;
    public enum Axis { X, Y, Z }
    //Nel caso ho toppato con l'asse, cambialo qui
    public Axis axis = Axis.X;
    public bool direction = true;


    // Start is called before the first frame update
    void Start()
    {
        angle = transform.localEulerAngles;
        //per defaul la luce è posizionata nella posizione (0,0,0) con rotazione (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //nel caso ho toppato con l'asse, questo switch permette di eseguire la rotazione in ogni caso.
        switch (axis)
        {
            case Axis.X:
                transform.localEulerAngles = new Vector3(Rotation(), setup, setup);
                break;
            case Axis.Y:
                transform.localEulerAngles = new Vector3(setup, Rotation(), setup);
                break;
            case Axis.Z:
                transform.localEulerAngles = new Vector3(setup, setup, Rotation());
                break;
        }
    }
    float Rotation()
    {
        //se secondo te c'è bisogno di rallentare o velocizzare la rotazione, modifica il numero dopo lo slash.
        rotation += (speed * Time.deltaTime) / 45;
        if (rotation >= 360f) rotation -= 360f;
        return direction ? rotation : -rotation;
    }
}
