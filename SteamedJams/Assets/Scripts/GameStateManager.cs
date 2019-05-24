using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

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

    [Header("Round Start")]
    [SerializeField] private GameObject[] m_roundNumber;
    [SerializeField] private Text m_roundCountdown;

    [Header("Game")]
    [SerializeField] private Image[] m_player1rounds;
    [SerializeField] private Image[] m_player2rounds;
    [SerializeField] private Text m_countdown;
    [SerializeField] private float m_defaultRoundTime = 100;
    private float m_roundTimer;

    [Header("Round End")]
    [SerializeField] private GameObject m_player1wins;
    [SerializeField] private GameObject m_player2wins;
    [SerializeField] private GameObject m_draw;
 
    private PlayerController m_player1;
    private PlayerController m_player2;
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

        Invoke("LateStart", 0.001f);
    }
    void LateStart ()
    {
        PlayerController[] players = PlayerManager.Instance.m_players;
        m_player1 = players[0];
        m_player2 = players[1];

        m_currentGameState = GameState.RoundStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentGameState == GameState.Game)
            UpdateGame();
        else if (m_currentGameState == GameState.Victory)
        {
            if (XCI.GetButtonDown(XboxButton.A))
            {
                ResetUIElements();
                StartRound();
            }
            else if (XCI.GetButtonDown(XboxButton.B))
            {
                Time.timeScale = 1;

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void ResetUIElements()
    {
        m_player1wins.SetActive(false);
        m_player2wins.SetActive(false);
        m_draw.SetActive(false);

        m_victoryPanel.SetActive(false);

        m_player1score = m_player2score = 0;

        for (int i = 0; i < m_player1rounds.Length; i++)
            m_player1rounds[i].color = new Color(1, 1, 1, 1);
        for (int i = 0; i < m_player2rounds.Length; i++)
            m_player2rounds[i].color = new Color(1, 1, 1, 1);

    }

    /// <summary>
    /// 
    /// </summary>
    public void StartRound()
    {
        m_currentGameState = GameState.RoundStart;

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
        float countdown = 4 + (Time.time - (int)Time.time) - 0.5f;
        int roundNumber = m_player1score + m_player2score;

        m_roundNumber[roundNumber].SetActive(true);

        while (countdown > 0)
        {
            countdown -= Time.deltaTime;

            if (countdown > 1)
                m_roundCountdown.text = ((int)countdown).ToString(); // 3 2 1 
            else
                m_roundCountdown.text = "FIGHT!"; // fight!

            yield return null;
        }

        AudioTrack randomTrack = m_audioTracks[Random.Range(0, m_audioTracks.Length - 1)];
        AudioManager audioManager = AudioManager.GetInstance();
        audioManager.SetBGM(randomTrack.m_AudioTrack, randomTrack.m_BPM);
        
        m_roundNumber[roundNumber].SetActive(false);
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
            StartCoroutine(RoundEnd());
        
        // check highest health
        if (m_player1.Health < 0 && m_player2.Health < 0)
        {
            // draw/sudden death/no winner
            m_player1rounds[m_player1score].color = Color.yellow;
            m_player2rounds[m_player2score].color = Color.yellow;
            m_player1score++;
            m_player2score++;
            m_draw.SetActive(true);
            StartCoroutine(RoundEnd());
        }
        else if (m_player2.Health < 0)
        {
            m_player1rounds[m_player1score].color = Color.yellow;
            m_player1score++; // player 1 won
            m_player1wins.SetActive(true);
            StartCoroutine(RoundEnd());
        }
        else if (m_player1.Health < 0)
        {
            m_player2rounds[m_player2score].color = Color.yellow;
            m_player2score++; // player 2 won
            m_player2wins.SetActive(true);
            StartCoroutine(RoundEnd());
        }
    }

    private IEnumerator RoundEnd()
    {
        m_gamePanel.SetActive(false);
        m_currentGameState = GameState.RoundEnd;

        // perform slow-mo zoom in
        float oldMaxZoom = CameraMovement.activeCamera.zoomMax;
        CameraMovement.activeCamera.zoomMax = 4;
        Time.timeScale = 0.5f;
        
        yield return new WaitForSecondsRealtime(3);

        CameraMovement.activeCamera.zoomMax = oldMaxZoom;

        m_player1.Health = 100;
        m_player2.Health = 100;

        if (m_player1score > 1)
            Victory(0);
        else if (m_player2score > 1)
            Victory(1);
        else
        {
            m_player1wins.SetActive(false);
            m_player2wins.SetActive(false);
            m_draw.SetActive(false);

            StartRound();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    private void Victory(int player)
    {
        m_currentGameState = GameState.Victory;
        
        if (player == 0)
            m_player1wins.SetActive(true);
        else
            m_player2wins.SetActive(true);

        m_victoryPanel.SetActive(true);
    }
}
