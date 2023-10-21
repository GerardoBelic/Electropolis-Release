using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject audio_source;
    // Start is called before the first frame update
    void Start()
    {
        /// Adjust waves sound volume
        audio_source.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");

        StartCoroutine(LoadAsync());
    }

    [SerializeField] private Slider progress;

    IEnumerator LoadAsync()
    {
        yield return new WaitForSeconds(1.0f);
        
        AsyncOperation async_load = SceneManager.LoadSceneAsync(2/*, LoadSceneMode.Additive*/);
        //async_load.allowSceneActivation = false;

        while (!async_load.isDone)
        {
            progress.value = async_load.progress;

            yield return null;
        }
    }
}
