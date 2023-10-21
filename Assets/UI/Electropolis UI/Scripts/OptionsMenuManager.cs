using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject pause_menu_panel;
    [SerializeField] private GameObject options_panel;

    public void close_options_panel()
    {
        options_panel.SetActive(false);
        pause_menu_panel.SetActive(true);
    }
    
    [SerializeField] private AudioManager audio_manager;

    [SerializeField] private Slider master_volume_slider;
    [SerializeField] private Slider music_volume_slider;
    [SerializeField] private Slider sfx_volume_slider;

    public void change_master_volume()
    {
        PlayerPrefs.SetFloat("MasterVolume", master_volume_slider.value);

        audio_manager.update_volume();
    }

    public void change_music_volume()
    {
        PlayerPrefs.SetFloat("MusicVolume", music_volume_slider.value);

        audio_manager.update_volume();
    }

    public void change_sfx_volume()
    {
        PlayerPrefs.SetFloat("SoundsVolume", sfx_volume_slider.value);

        audio_manager.update_volume();
    }

    [SerializeField] GameObject game_options_subpanel;
    [SerializeField] GameObject video_options_subpanel;

    public void show_game_options_subpanel()
    {
        game_options_subpanel.SetActive(true);
        video_options_subpanel.SetActive(false);
    }

    public void show_video_options_subpanel()
    {
        game_options_subpanel.SetActive(false);
        video_options_subpanel.SetActive(true);
    }

    [SerializeField] private TMP_Text rain_text;

    [SerializeField] private GameObject rain_effect;

    public void change_rain_status()
    {
        if (PlayerPrefs.GetInt("Rain") == 0)
        {
            PlayerPrefs.SetInt("Rain", 1);
            rain_text.text = "on";

            rain_effect.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("Rain") == 1)
        {
            PlayerPrefs.SetInt("Rain", 0);
            rain_text.text = "off";

            rain_effect.SetActive(false);
        }

    }

    [SerializeField] private TMP_Text pixel_effects_text;

    [SerializeField] private UniversalRenderPipelineAsset pipeline_asset;
    [SerializeField] private ScriptableRendererFeature pixel_renderer_feature;

    public void change_pixel_effect_status()
    {
        if (PlayerPrefs.GetInt("PixelEffects") == 0)
        {
            PlayerPrefs.SetInt("PixelEffects", 1);
            pixel_effects_text.text = "on";

            pixel_renderer_feature.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("PixelEffects") == 1)
        {
            PlayerPrefs.SetInt("PixelEffects", 0);
            pixel_effects_text.text = "off";

            pixel_renderer_feature.SetActive(false);
        }
    }

    public void initialize_player_prefs()
    {
        if (PlayerPrefs.GetInt("PixelEffects") == 1)
        {
            rain_effect.SetActive(true);
            pixel_effects_text.text = "on";
        }
        else
        {
            rain_effect.SetActive(false);
            pixel_effects_text.text = "off";
        }

        if (PlayerPrefs.GetInt("PixelEffects") == 1)
        {
            pixel_renderer_feature.SetActive(true);
            pixel_effects_text.text = "on";
        }
        else
        {
            pixel_renderer_feature.SetActive(false);
            pixel_effects_text.text = "off";
        }

        float master_volume_value = PlayerPrefs.GetFloat("MasterVolume");
        master_volume_slider.value = master_volume_value;

        float music_volume_value = PlayerPrefs.GetFloat("MusicVolume");
        music_volume_slider.value = master_volume_value;

        float sfx_volume_value = PlayerPrefs.GetFloat("SoundsVolume");
        sfx_volume_slider.value = master_volume_value;

    }

    void Start()
    {
        initialize_player_prefs();
    }

}
