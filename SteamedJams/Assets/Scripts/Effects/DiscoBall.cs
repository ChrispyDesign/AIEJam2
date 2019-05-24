using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : Effect
{
    private void Reset()
    {
        m_instant = false;
    }

    public override void OnPickup()
    {
        base.OnPickup();

        m_player.Invulnerable = true;
        m_player.m_godMode.Play();
    }
    public override void OnEffectEnd()
    {
        base.OnEffectEnd();

        m_player.Invulnerable = false;
        m_player.m_godMode.Stop();
    }

}
