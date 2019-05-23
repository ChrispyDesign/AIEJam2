using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private PlayerController m_player1;
    [SerializeField] private PlayerController m_player2;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_countdownPanel;
    [SerializeField] private GameObject m_gamePanel;
    [SerializeField] private GameObject m_victoryPanel;

    [Header("Round Time")]
    [SerializeField] private Text m_countdown;
    [SerializeField] private float m_defaultRoundTime = 100;
    private float m_roundTimer;

    private int m_player1score = 0;
    private int m_player2score = 0;

    [Header("Audio")]
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
        m_countdown.text = ((int)m_roundTimer).ToString();

        if (m_roundTimer <= 0)
        {
            // check highest health
            if (m_player1.Health > m_player2.Health)
                m_player1score++; // player 1 won
            else if (m_player1.Health < m_player2.Health)
                m_player2score++; // player 2 won
            else
            {
                // both won
                m_player1score++;
                m_player2score++;
            }

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
