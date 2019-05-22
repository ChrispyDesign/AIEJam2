using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempMovement : MonoBehaviour
{

    [Header("Variables")]
    public float speed;

    public int playerNo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (playerNo == 0)
        {
            if (Input.GetKey(KeyCode.W))
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.S))
                transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.A))
                transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            if (Input.GetKey(KeyCode.D))
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (playerNo == 1)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.DownArrow))
                transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            if (Input.GetKey(KeyCode.RightArrow))
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
}
