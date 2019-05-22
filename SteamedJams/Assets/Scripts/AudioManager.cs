using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // singleton
    private static AudioManager m_instance;

    private AudioSource m_audioSource;

    // BGM relevant variables
    [Header("BPM")]
    [Range(0, 200)]
    [SerializeField] private float m_BPM = 128;
    [Header("Time Signature")]
    [SerializeField] private int m_base = 4; // time signature top
    [SerializeField] private int m_step = 4; // time signature bottom
    private int m_currentStep; // current beat
    private int m_currentMeasure; // no idea what this does lol
    private float m_interval;
    private float m_nextTime;

    [Header("Window of Opportunity")]
    [Range(0.001f, 0.5f)]
    [SerializeField] private float m_windowOfOpportunity = 0.1f;
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
    public float GetOpportunityScalar() { return m_isInWindow ? m_opportunityScalar : 0.0f; }

    #endregion

    #region setters
    
    public void SetBPM(float BPM) { m_BPM = BPM; }
    public void SetWindowOfOpportunity(float windowOfOpportunity) { m_windowOfOpportunity = windowOfOpportunity; }

    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="BGM"></param>
    public void SetBGM(AudioClip BGM)
    {
        if (m_audioSource == null)
            m_audioSource = GetComponent<AudioSource>();

        m_audioSource.clip = BGM;
        m_audioSource.Play();
        StartMetronome();
    }

    /// <summary>
    /// 
    /// </summary>
    private void StartMetronome()
    {
        StopCoroutine(BGM());

        m_currentStep = 1;
        float multiplier = m_base / 4.0f;
        float tempInterval = 60.0f / m_BPM;
        m_interval = tempInterval / multiplier;
        m_nextTime = Time.time;

        StartCoroutine(BGM());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator BGM()
    {
        float window = m_windowOfOpportunity;

        while (true)
        {
            StartCoroutine(Window());

            yield return new WaitForSeconds(m_nextTime - Time.time);
            
            m_nextTime += m_interval;

            m_currentStep++;
            if (m_currentStep > m_step)
            {
                m_currentStep = 1;
                m_currentMeasure++;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Window()
    {
        float timer = 0;
        float time = (m_nextTime - Time.time) * 0.95f;

        while (timer < time)
        {
            m_opportunityScalar = Mathf.Sin((timer / time) * Mathf.PI);

            if (m_opportunityScalar < m_windowOfOpportunity / m_interval)
                m_isInWindow = true;
            else
                m_isInWindow = false;

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
