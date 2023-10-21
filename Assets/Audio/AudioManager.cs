using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] private AudioSource sfx_audio_source;
    //[SerializeField] private AudioClip[] sfx_clips;

    [SerializeField] private AudioMixer audio_mixer;

    [SerializeField] private AudioSource button_swipe;
    [SerializeField] private AudioSource button_click;
    [SerializeField] private AudioSource button_click_2;

    private void Start()
    {
        //audio_mixer.SetFloat("MasterVolume", -20.0f);
    }

    public void play_hover()
    {
        //sfx_audio_source.PlayOneShot(sfx_clips[0], 0.2f);
        button_swipe.Play();
    }

    public void play_click()
    {
        //sfx_audio_source.PlayOneShot(sfx_clips[1], 0.4f);
        button_click.Play();
    }

    public void play_click_2()
    {
        //sfx_audio_source.PlayOneShot(sfx_clips[1], 0.4f);
        button_click_2.Play();
    }

    private static float volume_slider_to_db(float slider_value)
    {
        float db_volume = Mathf.Log10(slider_value) * 20.0f;
        
        if (slider_value < 0.05f)
        {
            db_volume = -80.0f;
        }

        return db_volume;
    }

    public void update_volume()
    {
        float master_volume_slider = PlayerPrefs.GetFloat("MasterVolume");
		audio_mixer.SetFloat("MasterVolume", volume_slider_to_db(master_volume_slider));

        float music_volume_slider = PlayerPrefs.GetFloat("MusicVolume");
		audio_mixer.SetFloat("MusicVolume", volume_slider_to_db(music_volume_slider));

        float sfx_volume_slider = PlayerPrefs.GetFloat("SoundsVolume");
		audio_mixer.SetFloat("SFXVolume", volume_slider_to_db(sfx_volume_slider));
    }

    public void initialize_audio_mixer()
    {
        float master_volume_value = PlayerPrefs.GetFloat("MasterVolume");
        audio_mixer.SetFloat("MasterVolume", volume_slider_to_db(master_volume_value));

        float music_volume_value = PlayerPrefs.GetFloat("MusicVolume");
        audio_mixer.SetFloat("MusicVolume", volume_slider_to_db(music_volume_value));

        float sfx_volume_value = PlayerPrefs.GetFloat("SoundsVolume");
        audio_mixer.SetFloat("SFXVolume", volume_slider_to_db(sfx_volume_value));
    }
    
}
