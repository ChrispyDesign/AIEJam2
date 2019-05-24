using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealth : Effect
{
    public int m_healthIncrease;

    private void Reset()
    {
        m_instant = true;
    }

    public override void OnPickup()
    {
        base.OnPickup();

        m_player.Health += m_healthIncrease;
        m_player.m_healing.Play();
    }
}
