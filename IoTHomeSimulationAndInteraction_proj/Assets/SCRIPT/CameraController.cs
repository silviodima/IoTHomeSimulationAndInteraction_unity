using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditorInternal;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody rb;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    float rotation = 0f;
    float x, y, z;
    public bool direction = true;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    //array di float che indica in quale stanza si trova la telecamera (1 se c'è in quella stanza, 0 altrimenti)
    public static float[] movementValues;
    public static bool moving;

    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        movementValues = new float[5];
        moving = false;
        x = transform.position.x;
        y = transform.position.y;
        z =0;
       }
    void Update()
    {
        
            if (Input.GetKey(KeyCode.RightArrow))
            {
                y = Rotation(1);
                transform.localEulerAngles=new Vector3(x, y, z);
                
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                y = -Rotation(-1);
                transform.localEulerAngles=new Vector3(x, y, z);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                x = x + speed;
                transform.Translate(new Vector3(x, y, z));
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                x = x - speed;
                transform.Translate(new Vector3(x, y, z));
            }
        

        // float moverHorizontal = Input.GetAxis("Horizontal");
        //float moverVertical = Input.GetAxis("Vertical");

        // Vector3 movement = new Vector3(moverHorizontal, 0.0f, moverVertical);


        //il click del tasto sinistro del mouse stoppa immediatamente il movimento dell'oggetto
        //if (Input.GetMouseButtonDown(0))
        //{
        //   rb.velocity = Vector3.zero;
        // transform.LookAt(transform);
        //transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * speed);

        // }

        //se non viene effettuato nessun click, l'oggetto si muove tramite la pressione dei tasti freccia
        //  else
        //  {
        //      rb.AddForce(movement * speed);
        //  }

        //yaw += speedH * Input.GetAxis("Mouse X");
        //pitch -= speedV * Input.GetAxis("Mouse Y");

        //transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        //  float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        //Vector3 move = transform.right * x + transform.forward * z;

        //controller.Move(move * speed * Time.deltaTime);
        isMoving();
    }

    float Rotation(float y)
    {
        //se secondo te c'è bisogno di rallentare o velocizzare la rotazione, modifica il numero dopo lo slash.
        rotation += (speed * 10);
        if (rotation >= 360f) rotation -= 360f;
        return direction ? y : -y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("soggiornoCollider"))
        {
            movementValues[0] = 1;
        }
        if (other.CompareTag("stanzaCollider"))
        {
            movementValues[1] = 1;
        }
        if (other.CompareTag("cucinaCollider"))
        {
            movementValues[2] = 1;
        }
        if (other.CompareTag("bagnoCollider"))
        {
            movementValues[3] = 1;
        }
        if (other.CompareTag("ingressoCollider"))
        {
            movementValues[4] = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("soggiornoCollider"))
        {
            movementValues[0] = 0;
        }
        if (other.CompareTag("stanzaCollider"))
        {
            movementValues[1] = 0;
        }
        if (other.CompareTag("cucinaCollider"))
        {
            movementValues[2] = 0;
        }
        if (other.CompareTag("bagnoCollider"))
        {
            movementValues[3] = 0;
        }
        if (other.CompareTag("ingressoCollider"))
        {
            movementValues[4] = 0;
        }
    }

    public bool isMoving()
    {
        if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.01f)
        {
            moving = true;
        }

        else
        {
            moving = false;
        }

        return moving;
    }

}
