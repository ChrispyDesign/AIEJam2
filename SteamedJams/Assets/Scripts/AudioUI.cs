using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    private AudioManager m_audioManager;

    private Color m_player1colour;
    private Color m_player2colour;

    [Header("UI Elements")]
    [SerializeField] private Image m_centreIcon;
    [SerializeField] private RectTransform m_bar;
    [SerializeField] private Transform m_barAnchorLeft;
    [SerializeField] private Transform m_barAnchorRight;
    [SerializeField] private Transform m_textAnchorLeft;
    [SerializeField] private Transform m_textAnchorRight;

    [Header("Feedback")]
    [SerializeField] private GameObject m_feedbackIconPrefab;
    [SerializeField] private GameObject m_feedbackTextPrefab;
    [SerializeField] private int m_feedbackPoolSizes = 8;
    [SerializeField] private float m_feedbackDecayTime = 2;
    [SerializeField] private float m_feedbackTextSpeed = 50;
    private Queue<GameObject> m_feedbackIconPool = new Queue<GameObject>();
    private Queue<GameObject> m_feedbackTextPool = new Queue<GameObject>();

    [Header("Ticks")]
    [SerializeField] private GameObject m_tickPrefab;
    [SerializeField] private int m_tickPoolSize = 20;
    private Queue<GameObject> m_tickPool = new Queue<GameObject>();

    #region setters

    public void SetPlayer1Colour(Color player1) { m_player1colour = player1; }
    public void SetPlayer2Colour(Color player2) { m_player2colour = player2; }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_audioManager = AudioManager.GetInstance();

        // initialise feedback text pool
        for (int i = 0; i < m_feedbackPoolSizes; i++)
        {
            GameObject feedbackIcon = Instantiate(m_feedbackIconPrefab, m_centreIcon.transform);
            feedbackIcon.SetActive(false);
            m_feedbackIconPool.Enqueue(feedbackIcon);

            GameObject feedbackText = Instantiate(m_feedbackTextPrefab);
            feedbackText.SetActive(false);
            m_feedbackTextPool.Enqueue(feedbackText);
        }

        // initialise tick pool
        for (int i = 0; i < m_tickPoolSize; i++)
        {
            GameObject tick = Instantiate(m_tickPrefab, m_bar);
            tick.SetActive(false);
            m_tickPool.Enqueue(tick);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Tick()
    {
        GameObject tick1 = m_tickPool.Dequeue();
        GameObject tick2 = m_tickPool.Dequeue();

        Vector3 tick1start = tick1.transform.position = m_barAnchorLeft.transform.position;
        Vector3 tick2start = tick2.transform.position = m_barAnchorRight.transform.position;

        tick1.SetActive(true);
        tick2.SetActive(true);

        float timer = 0;
        Vector3 barMidpoint = m_bar.transform.position;

        while (tick1.transform.position.x != barMidpoint.x)
        {
            timer += Time.deltaTime / m_audioManager.GetIntervalTime();

            tick1.transform.position = Vector3.Lerp(tick1start, barMidpoint, timer / 3);
            tick2.transform.position = Vector3.Lerp(tick2start, barMidpoint, timer / 3);

            yield return null;
        }

        tick1.SetActive(false);
        tick2.SetActive(false);

        m_tickPool.Enqueue(tick1);
        m_tickPool.Enqueue(tick2);
    }

    public IEnumerator Feedback(int player, string feedbackMessage)
    {
        GameObject feedbackIcon = m_feedbackIconPool.Dequeue();
        GameObject feedbackText = m_feedbackTextPool.Dequeue();
        Image feedbackIconUI = feedbackIcon.GetComponent<Image>();
        Color feedbackColour;
        Text feedbackTextUI = feedbackText.GetComponent<Text>();
        feedbackTextUI.text = feedbackMessage;
        
        float timer = 0;
        Transform textAnchor;

        if (player == 1)
        {
            feedbackColour = m_player1colour;
            textAnchor = m_textAnchorLeft;
        }
        else
        {
            feedbackColour = m_player2colour;
            textAnchor = m_textAnchorRight;
        }

        feedbackIcon.SetActive(true);
        feedbackText.transform.position = textAnchor.transform.position;
        feedbackText.transform.SetParent(textAnchor);
        feedbackText.SetActive(true);

        while (timer < m_feedbackDecayTime)
        {
            timer += Time.deltaTime;
            float scale = 1 + (timer / m_feedbackDecayTime);
            float transparency = 1 - (timer / m_feedbackDecayTime);

            feedbackIcon.transform.localScale = new Vector3(scale, scale, 0);
            feedbackText.transform.Translate(new Vector3(0, m_feedbackTextSpeed * Time.deltaTime, 0));

            Color color = new Color(feedbackColour.r, feedbackColour.g, feedbackColour.b, transparency);
            feedbackIconUI.color = color;
            feedbackTextUI.color = color;

            yield return null;
        }

        feedbackIcon.SetActive(false);
        feedbackText.SetActive(false);
        m_feedbackIconPool.Enqueue(feedbackIcon);
        m_feedbackTextPool.Enqueue(feedbackText);
    }
}
