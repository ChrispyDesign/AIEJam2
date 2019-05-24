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
            //Renderer renderer = other.gameObject.GetComponent<Renderer>();

            //if (renderer)
            //{
            //    renderer.material.SetColor("_PlayerColour", m_color);
            //}

            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (player)
            {
                if (player.m_bloodSpray)
                {
                    ParticleSystem.MainModule main = player.m_bloodSpray.main;
                    main.startColor = m_color;
                }
                if (player.m_playerTrail)
                {
                    ParticleSystem.TrailModule main = player.m_playerTrail.trails;
                    main.colorOverLifetime = new Color(m_color.r, m_color.g, m_color.b, 90);
                }
                if (player.m_swordTrail)
                {
                    ParticleSystem.TrailModule main = player.m_swordTrail.trails;
                    main.colorOverLifetime = new Color(m_color.r, m_color.g, m_color.b, 90);
                }
                if (player.m_playerRenderer)
                {
                    player.m_playerRenderer.materials[1].SetColor("_PlayerColour", m_color);
                }
                if (player.m_swordRenderer)
                {
                    player.m_swordRenderer.material.SetColor("_PlayerColour", m_color);
                }
                if (player.m_hairRenderer)
                {
                    player.m_hairRenderer.material.SetColor("_PlayerColour", m_color);
                }

            }
        }
    }
}
