using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioTrack", menuName = "Audio Track")]
public class AudioTrack : ScriptableObject
{
    public float m_BPM;
    public AudioClip m_AudioTrack;
}
