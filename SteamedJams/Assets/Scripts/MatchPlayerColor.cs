using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPlayerColor : MonoBehaviour
{
    public bool activateOnStart;
    public GameObject tarParent;

    public int matchPlayerNo;

    // Start is called before the first frame update
    void Start()
    {
        if (activateOnStart)
            ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeColor()
    {
        Color tarColor = GameObject.FindGameObjectsWithTag("Player")[matchPlayerNo].GetComponentInChildren<Renderer>().material.color;
        ParticleSystem[] parSystems = tarParent.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < parSystems.Length; i++)
        {
            var main = parSystems[i].main;
            main.startColor = tarColor;
        }

        PulseWithBeat[] pubSystems = tarParent.GetComponentsInChildren<PulseWithBeat>();
        for (int i = 0; i < pubSystems.Length; i++)
        {
            pubSystems[i].Two = tarColor;
        }

    }
}
