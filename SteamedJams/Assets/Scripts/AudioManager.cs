using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioUI))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // singleton
    private static AudioManager m_instance;

    [SerializeField] private Material[] m_materials;

    private AudioUI m_audioUI;
    private AudioSource m_audioSource;

    // BGM relevant variables
    [SerializeField] private AudioClip m_startBGM;
    [Header("BPM")]
    [Range(0, 200)]
    [SerializeField] private float m_BPM = 128;
    private AudioClip m_BGM;
    [Header("Time Signature")]
    [SerializeField] private int m_base = 4; // time signature top
    [SerializeField] private int m_step = 4; // time signature bottom
    private int m_currentStep; // current beat
    private int m_currentMeasure; // no idea what this does lol
    private float m_interval;
    private float m_nextTime;

    [Header("Window of Opportunity")]
    [Range(0.1f, 1)]
    [SerializeField] private float m_windowOfOpportunity = 0.5f;
    private float m_window;
    private float m_opportunityScalar;
    private bool m_isInWindow = false;

    #region getters

    public static AudioManager GetInstance()
    {
        if (m_instance == null)
            m_instance = FindObjectOfType<AudioManager>();

        return m_instance;
    }
    
    /// <summary>
    /// use this to determine the musical window of opportunity
    /// </summary>
    /// <returns>true if inside the window of opportunity (near the beat), false otherwise</returns>
    public bool IsInWindowOfOpportunity() { return m_isInWindow; }
    
    /// <summary>
    /// use this to get a scalar factor for how close to the window of opportunity you are
    /// </summary>
    /// <returns>if outside the window of opportunity, returns 0, otherwise returns a multiplier between 0 and 1</returns>
    public float GetOpportunityScalarClamped() { return m_isInWindow ? m_opportunityScalar : 0.0f; }

    public float GetOpportunityScalarRaw() { return m_opportunityScalar; }
    public float GetIntervalTime() { return m_interval; }

    #endregion

    #region setters
    
    public void SetBPM(float BPM) { m_BPM = BPM; }
    public void SetWindowOfOpportunity(float windowOfOpportunity) { m_window = windowOfOpportunity; }
    public void SetPlayerColours(Color color, int playerID) { if (playerID == 1) m_audioUI.SetPlayer1Colour(color); else m_audioUI.SetPlayer2Colour(color); }

    #endregion

    private void Start()
    {
        if (m_startBGM != null)
            SetBGM(m_startBGM, 128);

        for (int i = 0; i < m_materials.Length; i++)
            m_materials[i].SetFloat("_BPM", 0);

        if (m_audioUI == null)
            m_audioUI = GetComponent<AudioUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            StartFeedbackTextRoutine(1);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            StartFeedbackTextRoutine(2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="BGM"></param>
    public void SetBGM(AudioClip BGM, float BPM)
    {
        if (m_audioUI == null)
            m_audioUI = GetComponent<AudioUI>();

        if (m_audioSource == null)
            m_audioSource = GetComponent<AudioSource>();

        m_BPM = BPM;
        m_BGM = BGM;

        StartMetronome();
    }

    /// <summary>
    /// 
    /// </summary>
    private void StartMetronome()
    {
        StopCoroutine(PlayBGM());

        m_currentStep = 1;
        float multiplier = m_base / 4.0f;
        float tempInterval = 60.0f / m_BPM;
        m_window = m_windowOfOpportunity * tempInterval;
        m_interval = tempInterval / multiplier;
        m_nextTime = Time.time;

        StartCoroutine(PlayBGM());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayBGM()
    {
        m_audioSource.Stop();
        m_audioSource.clip = m_BGM;
        m_audioSource.Play();
        bool tick = false;
        
        while (true)
        {
            float beatTime = m_nextTime - Time.time;
            StartCoroutine(Window(beatTime));
            StartCoroutine(m_audioUI.Tick());

            for (int i = 0; i < m_materials.Length; i++)
            {
                if (tick)
                    m_materials[i].SetFloat("_Tick", 1);
                else
                    m_materials[i].SetFloat("_Tick", 0);
            }

            tick = !tick;

            yield return new WaitForSeconds(beatTime);
            
            m_nextTime += m_interval;

            m_currentStep++;
            if (m_currentStep > m_step)
            {
                m_currentStep = 1;
                m_currentMeasure++;
            }
        }
    }

    private IEnumerator Window(float beatTime)
    {
        float halfSuccessWindow = m_windowOfOpportunity * 0.5f;

        m_isInWindow = true;
        StartCoroutine(MeasureOpportunityScalarDown(beatTime * halfSuccessWindow));
        
        yield return new WaitForSeconds(beatTime * halfSuccessWindow);

        m_isInWindow = false;

        yield return new WaitForSeconds(beatTime * (1 - m_windowOfOpportunity));

        m_isInWindow = true;
        StartCoroutine(MeasureOpportunityScalarUp(beatTime * halfSuccessWindow));

        yield return new WaitForSeconds(beatTime * halfSuccessWindow);
    }

    private IEnumerator MeasureOpportunityScalarDown(float window)
    {
        float timer = window;

        while (timer > 0)
        {
            m_opportunityScalar = (timer / window);

            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MeasureOpportunityScalarUp(float window)
    {
        float timer = 0;

        while (timer < window)
        {
            m_opportunityScalar = (timer / window);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    //private IEnumerator Window()
    //{
    //    float timer = 0;
    //    float time = (m_nextTime - Time.time) * 0.95f;

    //    while (timer < time)
    //    {
    //        m_opportunityScalar = 1 - Mathf.Sin((timer / time) * Mathf.PI);
    //        //Debug.Log(m_opportunityScalar + "   " + (1 - m_windowOfOpportunity));

    //        if (m_opportunityScalar > 1 - m_windowOfOpportunity)
    //            m_isInWindow = true;
    //        else
    //            m_isInWindow = false;

    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    public void StartFeedbackTextRoutine(int player)
    {
        if (m_isInWindow)
        {
            string feedback = "Nice";
            if (m_opportunityScalar > 0.9f)
                feedback = "Perfect!";
            else if (m_opportunityScalar > 0.5f)
                feedback = "Great!";

            StartCoroutine(m_audioUI.Feedback(player, feedback));
        }
    }
}
