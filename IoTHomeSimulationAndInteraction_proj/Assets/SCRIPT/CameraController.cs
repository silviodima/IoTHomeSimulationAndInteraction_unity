using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()

    {
        //
        float moverHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moverHorizontal, 0.0f, moverVertical);

        //il click del tasto sinistro del mouse stoppa immediatamente il movimento dell'oggetto
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector3.zero;
           // transform.LookAt(transform);
            //transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * speed);

        }

        //se non viene effettuato nessun click, l'oggetto si muove tramite la pressione dei tasti freccia
        else
        {
            rb.AddForce(movement * speed);
        }

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        //transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

    }

}
