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
    }
    public override void OnEffectEnd()
    {
        base.OnEffectEnd();

        m_player.Invulnerable = false;
    }

}
