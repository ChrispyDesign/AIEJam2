using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : Effect
{
    public float m_speedIncrease;

    private void Reset()
    {
        m_instant = false;
    }

    public override void OnPickup()
    {
        base.OnPickup();

        m_player.SpeedMultiplier += m_speedIncrease;
    }
    public override void OnEffectEnd()
    {
        base.OnEffectEnd();

        m_player.SpeedMultiplier -= m_speedIncrease;
    }
}
