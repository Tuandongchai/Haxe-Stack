using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [Header("Elements")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [Header("BackGround")]
    [SerializeField] private AudioSource[] bgSources;
    [Header("Sound Effect")]
    [SerializeField] private AudioSource[] sfxSources;
    [Header("Sound Clip Effect")]
    [SerializeField] private GameObject soundEffectSource;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("VolumeMusic"))
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            foreach (AudioSource source in bgSources)
            {
                source.volume = savedVolume;
                musicSlider.value = savedVolume;
            }
        }
        else
        {
            foreach (AudioSource source in bgSources)
            {
                source.volume = 1f;
                musicSlider.value = 1f;
            }
        }
        if (PlayerPrefs.HasKey("VolumeSFX"))
        {
            float savedVolume = PlayerPrefs.GetFloat("VolumeSFX");
            foreach (AudioSource source in sfxSources)
            {
                source.volume = savedVolume;
                SFXSlider.value = savedVolume;
            }

        }
        else
        {
            foreach (AudioSource source in sfxSources)
            {
                source.volume = 1f;
                SFXSlider.value = 1f;
            }

        }

        musicSlider.onValueChanged.AddListener(ChangeVolumeBG);
        SFXSlider.onValueChanged.AddListener(ChangeVolumeSFX);
    }
    private void ChangeVolumeBG(float value)
    {
        for (int i=0; i<bgSources.Length; i++)
        {
            bgSources[i].volume = value;

        }
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
    private void ChangeVolumeSFX(float value)
    {

        for (int i = 0; i < sfxSources.Length; i++)
        {
            sfxSources[i].volume = value;

        }
        PlayerPrefs.SetFloat("VolumeSFX", value);
        PlayerPrefs.Save();
    }

    /*public void PlaySoundEffect(int i) => sfxSources[i].PlayOneShot(sfxSources[i].clip);*/
    public void PlaySoundEffect(int i) 
    {
        AudioSource newSource = soundEffectSource.AddComponent<AudioSource>();
        newSource.clip = sfxSources[i].clip;
        newSource.Play();

        Destroy(newSource, 0.3f);
    } 
    public void StopSoundEffect(int i) => sfxSources[i].Stop(); 


    public void BGSoundOn(int i)
    {
        foreach(AudioSource audio in bgSources)
            if (audio.isPlaying)
                audio.Stop();
        bgSources[i].Play();
    }
    public void BGSoundOff(int i) => bgSources[i].Stop();
}
