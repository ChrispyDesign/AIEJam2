using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private float m_defaultRoundTime = 99;
    private float m_roundTimer;

    private int m_player1score = 0;
    private int m_player2score = 0;

    [SerializeField] private AudioTrack[] m_audioTracks;

    // Start is called before the first frame update
    void Start()
    {
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        m_roundTimer -= Time.deltaTime;

        if (m_roundTimer <= 0)
        {
            // check highest health

            if (m_player1score > 1 && m_player2score > 1)
            {
                // draw/sudden death/no winner
            }

            if (m_player1score > 1)
            {
                // player 1 wins
            }
            else if (m_player2score > 1)
            {
                // player 2 wins
            }
        }
    }

    public void StartRound()
    {
        AudioTrack randomTrack = m_audioTracks[Random.Range(0, m_audioTracks.Length - 1)];
        AudioManager audioManager = AudioManager.GetInstance();

        audioManager.SetBPM(randomTrack.m_BPM);
        audioManager.SetBGM(randomTrack.m_AudioTrack);

        m_roundTimer = m_defaultRoundTime;
    }
}
