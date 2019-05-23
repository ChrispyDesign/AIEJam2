using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public int m_damage;
    public Team m_team;
    public PlayerController m_player;

    private Collider m_collider;

    public Collider Collider
    {
        get { return m_collider; }
    }


    private void Start()
    {
        m_collider = GetComponent<Collider>();
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
        if (other.tag == "Pickup")
        {
            PickupShell pickup = other.gameObject.GetComponent<PickupShell>();

            if (pickup)
            {
                pickup.TakeDamage(m_damage, m_player);
            }
        }
    }
}
