using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace SlimUI.ModernMenu{
	public class CheckMusicVolume : MonoBehaviour {

		[SerializeField] private AudioMixer audio_mixer;

		private static float volume_slider_to_db(float slider_value)
		{
			float db_volume = Mathf.Log10(slider_value) * 20.0f;
			
			if (slider_value < 0.05f)
			{
				db_volume = -80.0f;
			}

			return db_volume;
		}

		public void  Start (){
			// remember volume level from last time
			//GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");

			
			
		}

		public void UpdateMasterVolume (){
			//GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			float volume_slider = PlayerPrefs.GetFloat("MasterVolume");
			audio_mixer.SetFloat("MasterVolume", volume_slider_to_db(volume_slider));
		}

		public void UpdateMusicVolume (){
			//GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			float volume_slider = PlayerPrefs.GetFloat("MusicVolume");
			audio_mixer.SetFloat("MusicVolume", volume_slider_to_db(volume_slider));
		}

		public void UpdateSoundsVolume (){
			//GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			float volume_slider = PlayerPrefs.GetFloat("SoundsVolume");
			audio_mixer.SetFloat("SFXVolume", volume_slider_to_db(volume_slider));
		}
	}
}