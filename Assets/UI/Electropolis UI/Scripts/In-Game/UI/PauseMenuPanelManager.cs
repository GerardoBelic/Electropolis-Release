using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
    Manager for the pause menu :v
    TODO: when we open the pause menu, the game has to stop
*/

public class PauseMenuPanelManager : MonoBehaviour
{

    [SerializeField] private GameObject pause_menu_panel;
    [SerializeField] private GameObject load_save_menu_panel;
    [SerializeField] private GameObject options_menu_panel;

    [SerializeField] private GameObject quit_message_panel;

    [SerializeField] private GameObject pause_overlay;

    private bool is_paused = false;

    #region Pause game

    public void pause_game()
    {
        if (is_paused && load_save_menu_panel.activeSelf)
        {
            gameObject.SetActive(true);
            load_save_menu_panel.SetActive(false);

            return;
        }
        else if (is_paused && options_menu_panel.activeSelf)
        {
            gameObject.SetActive(true);
            options_menu_panel.SetActive(false);

            return;
        }
        else if (is_paused)
        {
            resume_button_pressed();
            
            return;
        }

        is_paused = true;

        Time.timeScale = 0.0f;

        pause_menu_panel.SetActive(true);

        pause_overlay.SetActive(true);
    }

    #endregion
    
    #region Resume game

    public void resume_button_pressed()
    {
        /// TODO: unpause game

        is_paused = false;

        Time.timeScale = 1.0f;

        pause_menu_panel.SetActive(false);

        pause_overlay.SetActive(false);
    }

    #endregion

    #region  Save game

    [SerializeField] private LoaderSaverManager loader_saver;

    public void save_game_button_pressed()
    {
        loader_saver.save_state();

        resume_button_pressed();
    }

    #endregion

    #region Load game

    public void load_save_button_pressed()
    {
        load_save_menu_panel.SetActive(true);
        pause_menu_panel.SetActive(false);
    }

    #endregion

    #region Options

    public void options_button_pressed()
    {
        options_menu_panel.SetActive(true);
        pause_menu_panel.SetActive(false);
    }

    #endregion

    #region Quit

    public void quit_button_pressed()
    {
        quit_message_panel.SetActive(true);
        pause_menu_panel.SetActive(false);
    }

    public void quit_panel_no_button_pressed()
    {
        quit_message_panel.SetActive(false);
        pause_menu_panel.SetActive(true);
    }

    public void quit_panel_yes_button_pressed()
    {
        /*#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif*/

        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    #endregion

}
