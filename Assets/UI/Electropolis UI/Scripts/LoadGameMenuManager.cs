using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class LoadGameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pause_menu_panel;
    [SerializeField] private GameObject load_game_panel;

    public void close_load_game_panel()
    {
        load_game_panel.SetActive(false);
        pause_menu_panel.SetActive(true);
    }

    [SerializeField] private GameObject layout_load_game_saves;
    [SerializeField] private GameObject prefab_load_game_save_template;

    /// This function add the save files to the vertical layout (names and last played dates)
    void populate_saves_panel()
    {

        /// First we delete the saves that we may previously loaded, EXCEPT for the save template
        foreach (Transform child in layout_load_game_saves.transform)
        {
            if (child.name != "Save_Template")
            {
                Destroy(child.gameObject);
            }
        }

        SaveInfo[] saves_info = SaveInfo.get_stored_saves_info();

        foreach(SaveInfo save in saves_info)
        {
            GameObject save_button = Instantiate(prefab_load_game_save_template, layout_load_game_saves.transform);

            TMP_Text save_button_name = save_button.transform.Find("Title").GetComponent<TMP_Text>();
            TMP_Text save_button_date = save_button.transform.Find("Date").GetComponent<TMP_Text>();

            string dir_name = save.save_directory.Name;

            //save_button_name.text = Path.GetFileNameWithoutExtension(save.name);
            save_button_name.text = dir_name;
            save_button_date.text = save.date.ToString();

            SaveInfo save_button_info = save_button.GetComponent<SaveInfo>();
            save_button_info.replace_values(save);

            save_button.SetActive(true);
        }

    }

    [SerializeField] private GameObject subpanel_load_game_preview;
    [SerializeField] private GameObject button_load_game_load;
    [SerializeField] private GameObject image_load_game_preview_city_portrait;
    [SerializeField] private GameObject text_load_game_preview_city_name;
    [SerializeField] private GameObject text_load_game_preview_population;
    [SerializeField] private GameObject text_load_game_preview_money;

    private SaveInfo save_info_selected = null;

    public void show_load_game_save_preview(SaveInfo save_to_preview)
    {
        /// 1.- Show the preview subpanel and the load button
        subpanel_load_game_preview.SetActive(true);
        button_load_game_load.SetActive(true);

        /// 2.- Load portrait from save directory (if it exists)
        Texture2D image_texture = new Texture2D(1920, 1080);
        Color[] default_pixels = Enumerable.Repeat(Color.red, image_texture.width * image_texture.height).ToArray();
        image_texture.SetPixels(default_pixels);
        image_texture.Apply();
        if (save_to_preview.portrait != null)
        {
            byte[] image_bytes = File.ReadAllBytes(save_to_preview.portrait.FullName);
            image_texture.LoadImage(image_bytes);
        }
        
        RawImage preview_portrait = image_load_game_preview_city_portrait.GetComponent<RawImage>();
        preview_portrait.texture = image_texture;
        AspectRatioFitter preview_portrait_fitter = image_load_game_preview_city_portrait.GetComponent<AspectRatioFitter>();
        float new_aspect_ratio = (float)image_texture.width / image_texture.height;
        preview_portrait_fitter.aspectRatio = new_aspect_ratio;

        /// 3.- Display save stats (city name, population and money)
        TMP_Text save_city_name = text_load_game_preview_city_name.GetComponent<TMP_Text>();
        TMP_Text save_population = text_load_game_preview_population.GetComponent<TMP_Text>();
        TMP_Text save_money = text_load_game_preview_money.GetComponent<TMP_Text>();

        /// TODO: load this fields from the file
        save_city_name.text = "???";
        save_population.text =  "???";
        save_money.text = "???";

        /// 4.- Update save_info_selected to be loaded later
        save_info_selected = save_to_preview;
    }

    public void button_load_game_load_pressed()
    {
        LoaderSaverManager.load_save_name = save_info_selected.save_directory.Name;
        LoaderSaverManager.loaded = false;

        Time.timeScale = 1.0f;
        SceneManager.LoadScene(3);
    }

    void OnEnable()
    {
        populate_saves_panel();
    }
}
