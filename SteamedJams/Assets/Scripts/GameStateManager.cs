using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainMenu,
    RoundStart,
    Game,
    RoundEnd,
    Victory
}

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager m_instance;

    private GameState m_currentGameState;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_countdownPanel;
    [SerializeField] private GameObject m_gamePanel;
    [SerializeField] private GameObject m_victoryPanel;

    [Header("Round Time")]
    [SerializeField] private Text m_countdown;
    [SerializeField] private float m_defaultRoundTime = 100;
    private float m_roundTimer;

    [Header("Player Score")]
    [SerializeField] private Toggle m_player1toggle;
    [SerializeField] private Toggle m_player2toggle;
    private int m_player1score = 0;
    private int m_player2score = 0;

    [Header("Audio")]
    [SerializeField] private AudioTrack[] m_audioTracks;

    #region getters

    public static GameStateManager GetInstance()
    {
        return m_instance;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;

        m_currentGameState = GameState.RoundStart;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_currentGameState)
        {
            case GameState.MainMenu:
                // do nothing? maybe?
                break;

            case GameState.Game:
                UpdateGame();
                break;
                
            case GameState.Victory:
                UpdateVictory();
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void StartRound()
    {
        m_countdownPanel.SetActive(true);
        Time.timeScale = 1;

        m_roundTimer = m_defaultRoundTime;
        StartCoroutine(Countdown());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Countdown()
    {
        Debug.Log(Time.time);
        Debug.Log(Time.time - (int)Time.time);
        float countdown = 4 + (Time.time - (int)Time.time) - 0.5f;
        Text countdownText = m_countdownPanel.GetComponentInChildren<Text>();

        while (countdown > 1)
        {
            countdown -= Time.deltaTime;

            countdownText.text = ((int)countdown).ToString();

            yield return null;
        }

        AudioTrack randomTrack = m_audioTracks[Random.Range(0, m_audioTracks.Length - 1)];
        AudioManager audioManager = AudioManager.GetInstance();
        audioManager.SetBGM(randomTrack.m_AudioTrack, randomTrack.m_BPM);

        m_countdownPanel.SetActive(false);
        m_gamePanel.SetActive(true);
        m_currentGameState = GameState.Game;
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateGame()
    {
        m_roundTimer -= Time.deltaTime;
        m_countdown.text = ((int)m_roundTimer).ToString();

        if (m_roundTimer <= 0)
        {
            StartCoroutine(RoundEnd());
            m_currentGameState = GameState.RoundEnd;
        }
    }

    private IEnumerator RoundEnd()
    {
        m_gamePanel.SetActive(false);

        // perform slow-mo zoom in
        float oldMaxZoom = CameraMovement.activeCamera.zoomMax;
        CameraMovement.activeCamera.zoomMax = 4;
        Time.timeScale = 0.5f;

        //PlayerController[] players = PlayerManager.Instance.m_players;
        //PlayerController player1 = players[0];
        //PlayerController player2 = players[1];

        //// check highest health
        //if (player1.Health > player2.Health)
        //{
        //    m_player1score++; // player 1 won
        //    m_player2toggle.isOn = true;
        //}
        //else if (player1.Health < player2.Health)
        //{
        //    m_player2score++; // player 2 won
        //    m_player2toggle.isOn = true;
        //}
        //else
        //{
        //    // both won
        //    m_player1score++;
        //    m_player2score++;
        //    m_player1toggle.isOn = true;
        //    m_player2toggle.isOn = true;
        //}

        //if (m_player1score > 1 && m_player2score > 1)
        //{
        //    // draw/sudden death/no winner
        //}

        //if (m_player1score > 1)
        //{
        //    // player 1 wins
        //}
        //else if (m_player2score > 1)
        //{
        //    // player 2 wins
        //}

        yield return new WaitForSecondsRealtime(3);

        CameraMovement.activeCamera.zoomMax = oldMaxZoom;
        StartRound();
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateVictory()
    {

    }
}
