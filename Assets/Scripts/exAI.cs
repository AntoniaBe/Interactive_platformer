using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exAI : MonoBehaviour {

    private Rigidbody rb;
    public float speed = 2f;
    public float rayLenght = 3f;
    private float jumpSpeed = 4f;
    private RaycastHit vision;
    private Vector3 to = new Vector3(1f, 0f, 0f);
    private float velY;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velY = rb.velocity.y;
    }
    

    private void FixedUpdate()
    {
        velY = rb.velocity.y;
        Debug.DrawRay(rb.transform.position, to * rayLenght, Color.red, 1.5f);

        if (Physics.Raycast(transform.position, to, out vision, rayLenght))
        {

            Debug.Log(vision.collider.tag);

            if (vision.collider.tag == "treeStump")
            {

                Debug.Log("JUMP");
                velY = rb.velocity.y;
                rb.velocity = new Vector3(speed, jumpSpeed, 0);

            }
            else
            {

                rb.velocity = new Vector3(speed, velY, 0);

            }

        }
        else
        {

            rb.velocity = new Vector3(speed, velY, 0);

        }
    }
}
