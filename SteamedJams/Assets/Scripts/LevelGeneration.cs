using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public bool m_generate = false; //TEMP REMOVE
    [Header("GameObjects")]
    public GameObject playerManager;

    [Header("Prefabs")]
    public List<GameObject> obs = new List<GameObject>();
    public List<GameObject> props = new List<GameObject>();
    public GameObject emptyPropHolder;
    
    [Header("Variables")]
    public Vector2 propChance;

    [HideInInspector]
    public List<Transform> freeBasePropHolders = new List<Transform>();

    public static LevelGeneration activeManager;

    PlayerManager m_playerManager;
    bool m_firstRun = true;

    // Use this for initialization
    void Start()
    {
        m_playerManager = PlayerManager.Instance;

        activeManager = this.GetComponent<LevelGeneration>();

        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_generate)
        {
            GenerateMap();
            m_generate = false;
        }
    }

    void BasePass ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int rotation = Random.Range(0, 5);
            GameObject GO = Instantiate(obs[Random.Range(0, obs.Count)], transform.GetChild(i).position, Quaternion.Euler(0, rotation * 90, 0), transform.GetChild(i));
            //playerManager.GetComponent<PlayerManager>().m_playerStartPositions[i] = GO.transform.GetChild(0);
            for (int j = 0; j < GO.transform.GetChild(0).childCount; j++)
            {
                freeBasePropHolders.Add(GO.transform.GetChild(0).GetChild(j));
            }
        }
        if (playerManager != null)
            playerManager.SetActive(true);
    }

    void PropPass ()
    {
        float propLikelihood = Random.Range(propChance.x, propChance.y);
        for (int i = 0; i < freeBasePropHolders.Count; i++)
        {
            float chance = Random.Range(0f, 100f);
            int propNo = Random.Range(0, props.Count);
            if (chance < propLikelihood)
            {
                Instantiate(props[propNo], freeBasePropHolders[i]);
                freeBasePropHolders.RemoveAt(i);
                i -= 1;
            }
        }
    }

    void LastPass()
    {
        for (int i = 0; i < freeBasePropHolders.Count; i++)
        {
            Instantiate(emptyPropHolder, freeBasePropHolders[i]);
        }
    }

    void PlayerPass()
    {
        if (m_playerManager)
        {
            int player1Index = Random.Range(0, freeBasePropHolders.Count);
            m_playerManager.m_players[0].transform.position = freeBasePropHolders[player1Index].transform.position;
            freeBasePropHolders.RemoveAt(player1Index);

            int player2Index = Random.Range(0, freeBasePropHolders.Count);
            m_playerManager.m_players[1].transform.position = freeBasePropHolders[player2Index].transform.position;
            freeBasePropHolders.RemoveAt(player2Index);
        }
    }

    void GenerateMap()
    {
        if (!m_firstRun)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform tmp = transform.GetChild(i).GetChild(0);
                if (tmp)
                    Destroy(tmp.gameObject);
            }
        }

        BasePass();
        PlayerPass();
        PropPass();
        LastPass();

        m_firstRun = false;
    }
}