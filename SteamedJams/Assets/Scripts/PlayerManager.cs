using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager m_instance;

    public PlayerController[] m_players;

    public static PlayerManager Instance
    {
        get { return m_instance; }
    }


    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;
    }


}
