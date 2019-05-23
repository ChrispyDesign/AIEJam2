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

            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (player)
            {
                if (player.m_bloodSpray)
                {
                    ParticleSystem.MainModule main = player.m_bloodSpray.main;
                    main.startColor = m_color;
                }
            }
        }
    }
}
