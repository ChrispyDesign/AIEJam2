using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    [Header("GameObjects")]
    public GameObject playerManager;
    [Header("Prefabs")]
    public List<GameObject> obs = new List<GameObject>();
    public List<GameObject> props = new List<GameObject>();
    [Header("Variables")]
    public Vector2 propChance;

    private List<Transform> basePropHolders = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        BasePass();
        PropPass();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BasePass ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject GO = Instantiate(obs[Random.Range(0, obs.Count)], transform.GetChild(i).position, transform.GetChild(i).rotation, transform.GetChild(i));
            //playerManager.GetComponent<PlayerManager>().m_playerStartPositions[i] = GO.transform.GetChild(0);
            for (int j = 0; j < GO.transform.GetChild(0).childCount; j++)
            {
                basePropHolders.Add(GO.transform.GetChild(0).GetChild(j));
            }
        }
        if (playerManager != null)
            playerManager.SetActive(true);
    }

    void PropPass ()
    {
        float propLikelihood = Random.Range(propChance.x, propChance.y);
        for (int i = 0; i < basePropHolders.Count; i++)
        {
            float chance = Random.Range(0f, 100f);
            int propNo = Random.Range(0, props.Count);
            if (chance > propLikelihood)
                Instantiate(props[propNo], basePropHolders[i]);
        }
    }
}