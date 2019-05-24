using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSelect : MonoBehaviour
{
    public Color colorSel;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.GetComponentsInChildren<Renderer>().Length; i++)
        {
            this.GetComponentsInChildren<Renderer>()[i].material.color = colorSel;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", colorSel);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Renderer>().material.color = colorSel;
        }
    }
}
