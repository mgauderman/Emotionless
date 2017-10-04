using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{

    float currentMusicTime;
    public AudioMixerSnapshot norm, happy, sad, angry;
    public AudioMixer m;
    public AudioSource[] soundSources;
    public AudioSource currentSource;
    private float bpm = 90;
    private int level;
    private float masterVolume = 0;

    private float m_TransitionIn, m_QuarterNote;


    void Start()
    {
        m_QuarterNote = 60 / bpm;
        m_TransitionIn = m_QuarterNote * 1.5f;
        //m_TransitionOut = m_QuarterNote * 20;
        soundSources = new AudioSource[4];
        AudioSource[] sources = gameObject.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource s in sources)
        {
            if (s.clip.name.Contains("Ambient"))
                soundSources[0] = s;
            else if (s.clip.name.Contains("Happy"))
                soundSources[1] = s;
            else if (s.clip.name.Contains("Sad"))
                soundSources[2] = s;
            else if (s.clip.name.Contains("Angry"))
                soundSources[3] = s;
        }
        currentSource = soundSources[0];
    }

    public void SetMusicLvl(float vol)
    {
        masterVolume = vol;
        m.SetFloat("masterVol", vol);
    }

    public void SetSfxLvl(float vol)
    {
        m.SetFloat("sfxVol", vol);
    }

    public void Mute(bool soundOn)
    {
        float vol = soundOn ? masterVolume : -80f;
        m.SetFloat("masterVol", vol);
    }

    void transitionTo(AudioMixerSnapshot audio, AudioSource source)
    {
//        Debug.Log(audio.name + " vs " + m.FindSnapshot(audio.name).name);
//        print(!m.FindSnapshot(audio.name).name.Contains(audio.name));
        float value = 0;
        bool result = m.GetFloat(audio.name.ToLower() + "Vol", out value);
        //print(result + " " + (result ? value + "" : "N/A"));
        if (result && Mathf.Approximately(-80, value))
        {
            audio.TransitionTo(m_TransitionIn);
            if (!source.isPlaying)
            {
                source.Play();
                currentSource = source;
            }
        }
    }

    void Update()
    {
        int temp = level;
        level = MetricManager.c_currentLevel;
        if (temp != level)
        {
            switch (level)
            {
                case 0:
                case 1:
                case 6:
                    transitionTo(norm, soundSources[0]);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    transitionTo(happy, soundSources[1]);
                    break;
                case 7:
                case 8:
                case 9:
                    transitionTo(sad, soundSources[2]);
                    break;
                case 10:
                case 11:
                case 12:
                    transitionTo(angry, soundSources[3]);
                    break;
            }
        }
    }
}