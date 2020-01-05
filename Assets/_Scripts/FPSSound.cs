using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSound : MonoBehaviour
{
    public float m_volume;
    public AudioClip m_ambience;

    public AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource.clip = m_ambience;
        m_audioSource.volume = m_volume;
        m_audioSource.loop = true;
        m_audioSource.Play();
    }
}
