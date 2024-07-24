using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform cam;
    public float speed = 100.0f;
    public Rigidbody rb;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(-cam.right.z * speed * Time.deltaTime, 0, cam.right.x * speed * Time.deltaTime);
            //rb.AddForce(cam.right.x * speed * Time.deltaTime, 0, cam.right.z * speed * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(cam.right.z * speed * Time.deltaTime, 0, -cam.right.x * speed * Time.deltaTime);
            //rb.AddForce(-cam.forward.x * speed * Time.deltaTime , -cam.forward.y * speed * Time.deltaTime * 0, -cam.forward.z * speed * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(-cam.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(cam.right * speed * Time.deltaTime);
        }

        // Debug.Log("Forward : " + cam.forward + " Right : " + cam.right + " Up : " + cam.up +" Result: " + cam.forward.x + cam.right.y);
    }
}
