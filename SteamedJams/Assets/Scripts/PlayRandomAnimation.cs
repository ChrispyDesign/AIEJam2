using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayRandomAnimation : MonoBehaviour
{
    public int m_animationCount;

    Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetInteger("Random", Random.Range(0, m_animationCount));
    }  
}
