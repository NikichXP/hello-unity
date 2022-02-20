using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectPhysics : MonoBehaviour
{

    public float force;
    public Rigidbody Rigidbody;
    public Input Input;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody.velocity *= 0.999f;
        if (Input.GetKey("w"))
        {
            Rigidbody.velocity += new Vector3(force, 0, 0);
        }
        if (Input.GetKey("s"))
        {
            Rigidbody.velocity += new Vector3(-force, 0, 0);
        }
        if (Input.GetKey("a"))
        {
            Rigidbody.velocity += new Vector3(0, 0, force);
        }
        if (Input.GetKey("d"))
        {
            Rigidbody.velocity += new Vector3(0, 0, -force);
        }
        if (Input.GetKey("space"))
        {
            Rigidbody.velocity += new Vector3(0, force * 3, 0);
        }
    }
}
