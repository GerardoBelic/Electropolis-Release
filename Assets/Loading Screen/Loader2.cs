using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Loader2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /// Adjust waves sound volume

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
            yield return null;
        }
    }
}
