using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [Min(0)]
    public float m_duration;

    protected PlayerController m_player;
    protected bool m_instant;

    public PlayerController Player
    {
        get { return m_player; }
        set { m_player = value; }
    }

    public bool Instant
    {
        get { return m_instant; }
    }
    public virtual void OnPickup()
    {

    }

    public virtual void OnEffectEnd()
    {

    }

    public virtual bool OnEffectUpdate()
    {
        m_duration -= Time.deltaTime;

        if (m_duration < 0)
        {
            OnEffectEnd();
            return false;
        }

        return true;
    }
}
