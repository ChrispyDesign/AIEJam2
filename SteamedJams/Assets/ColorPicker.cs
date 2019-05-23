using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public Color m_color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Renderer renderer = other.gameObject.GetComponent<Renderer>();

            if (renderer)
            {
                renderer.material.SetColor("_PlayerColour", m_color);
            }
        }
    }
}
