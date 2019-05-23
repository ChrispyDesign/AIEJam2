using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShell : MonoBehaviour
{
    public int m_maxHealth;
    public Effect m_pickup;

    private int m_health;

    private void Start()
    {
        m_health = m_maxHealth;
    }

    public void TakeDamage(int damage, PlayerController player)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            if (m_pickup.transform.parent == transform)
            {
                m_pickup.transform.parent = null;
            }

            player.AddEffect(m_pickup);

            Destroy(gameObject);
        }
    }
}
