using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public int m_damage;
    public Team m_team;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) { return; }

        if (other.tag == "Player")
        {
            PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();

            if (otherPlayer && otherPlayer.m_team != m_team)
            {
                otherPlayer.TakeDamage(m_damage);
            }
        }
    }
}
