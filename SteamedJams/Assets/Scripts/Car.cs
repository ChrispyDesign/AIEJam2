using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Vector2 speedLimits;
    private float speed;
    public List<Material> carColor = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude < speed)
            GetComponent<Rigidbody>().AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    public void Reset()
    {
        speed = Random.Range(speedLimits.x, speedLimits.y);
        GetComponentInChildren<MeshRenderer>().material = carColor[Mathf.RoundToInt(Random.Range(0, carColor.Count))];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > speed/1.1f)
            Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Car" && collision.transform.GetComponent<Car>().speed != 0)
        {
            speed = 0;
            GetComponent<Rigidbody>().mass = 0;
        }
    }
}
