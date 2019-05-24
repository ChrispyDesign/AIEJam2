using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider m_p1Scroll;
    public Slider m_p2Scroll;

    PlayerManager m_playerManager;
    // Start is called before the first frame update
    void Start()
    {
        m_playerManager = PlayerManager.Instance;
        m_p1Scroll.maxValue = m_playerManager.m_players[0].m_maxHealth;
        m_p2Scroll.maxValue = m_playerManager.m_players[1].m_maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        m_p1Scroll.value = m_playerManager.m_players[0].Health;
        m_p2Scroll.value = m_playerManager.m_players[1].Health;
    }
}
