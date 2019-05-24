using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCollider : MonoBehaviour
{
    public PlayerController m_player;
    public float m_pushStrength = 10;

    Collider m_collider;
    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Prop"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb)
            {
                rb.velocity += (other.transform.position - transform.position).normalized * m_pushStrength * Time.deltaTime;
            }
        }
    }
}
