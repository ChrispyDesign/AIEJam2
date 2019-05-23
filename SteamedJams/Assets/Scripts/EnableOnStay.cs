using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStay : MonoBehaviour
{
    public List<GameObject> activeOnStay = new List<GameObject>();
    public List<GameObject> deactiveOnStay = new List<GameObject>();

    private List<GameObject> playersInArea = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playersInArea.Count != 0)
        {
            for (int i = 0; i < activeOnStay.Count; i++)
            {
                activeOnStay[i].SetActive(true);
            }
            for (int i = 0; i < deactiveOnStay.Count; i++)
            {
                deactiveOnStay[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < activeOnStay.Count; i++)
            {
                activeOnStay[i].SetActive(false);
            }
            for (int i = 0; i < deactiveOnStay.Count; i++)
            {
                deactiveOnStay[i].SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playersInArea.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playersInArea.Remove(other.gameObject);
        }
    }
}
