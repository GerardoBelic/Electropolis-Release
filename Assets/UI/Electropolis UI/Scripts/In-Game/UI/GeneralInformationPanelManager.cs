using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GeneralInformationPanelManager : MonoBehaviour
{

    /// Text and sliders of the info showed in the general information panel
    [Header("Money")]
    [SerializeField] private TMP_Text money_balance_text;
    [SerializeField] private TMP_Text money_change_text;

    [Header("Population")]
    [SerializeField] private Image population_status_image;
    [SerializeField] private TMP_Text current_population_text;
    [SerializeField] private TMP_Text population_change_text;

    [Header("Date")]
    [SerializeField] private TMP_Text current_date_text;
    [SerializeField] private Image day_progress_slider;

    [Header("RCI")]
    [SerializeField] private Image positive_residence_demand_slider;
    [SerializeField] private Image negative_residence_demand_slider;
    [SerializeField] private Image positive_commerce_demand_slider;
    [SerializeField] private Image negative_commerce_demand_slider;
    [SerializeField] private Image positive_industry_demand_slider;
    [SerializeField] private Image negative_industry_demand_slider;

    [Header("CEOs")]
    [SerializeField] private Image ceo_1_status_image;
    [SerializeField] private Image ceo_1_positive_reputation_slider;
    [SerializeField] private Image ceo_1_negative_reputation_slider;
    [SerializeField] private Image ceo_2_status_image;
    [SerializeField] private Image ceo_2_positive_reputation_slider;
    [SerializeField] private Image ceo_2_negative_reputation_slider;
    [SerializeField] private Image ceo_3_status_image;
    [SerializeField] private Image ceo_3_positive_reputation_slider;
    [SerializeField] private Image ceo_3_negative_reputation_slider;
    [SerializeField] private Image ceo_4_status_image;
    [SerializeField] private Image ceo_4_positive_reputation_slider;
    [SerializeField] private Image ceo_4_negative_reputation_slider;


    #region Money

    private void update_money_balance_text(float money)
    {
        money_balance_text.text = "$" + ((int)money).ToString();
    }

    private void update_money_change_text(float money_change)
    {
        money_change_text.text = "+$" + ((int)money_change).ToString();
    }

    #endregion

    #region Population

    private void update_population_status_image()
    {

    }

    private void update_current_population_text(int current_population)
    {
        current_population_text.text = current_population.ToString("N0");
    }

    private void update_population_change_text(int population_change)
    {
        population_change_text.text = population_change.ToString("N0");
    }

    #endregion

    #region Date

    private void update_current_date_text(DateTime day)
    {
        current_date_text.text = day.ToString("dd/MM/yyyy");
    }

    private void update_day_progress_slider(double day_time)
    {
        day_progress_slider.fillAmount = (float)day_time;
    }

    #endregion

    #region RCI

    private void update_residence_demand_sliders()
    {

    }

    private void update_commerce_demand_sliders()
    {

    }

    private void update_industry_demand_sliders()
    {

    }

    #endregion

    #region CEOs

    private void update_ceo_status_image(int ceo_index)
    {
        Image ceo_status_image;

        switch (ceo_index)
        {
        case 1:
            ceo_status_image = ceo_1_status_image;
            break;

        case 2:
            ceo_status_image = ceo_2_status_image;
            break;

        case 3:
            ceo_status_image = ceo_3_status_image;
            break;

        case 4:
            ceo_status_image = ceo_4_status_image;
            break;

        default:
            Debug.LogError("CEO not recognized");
            return;
        }
    }

    private void update_ceo_reputation_slider(int ceo_index)
    {
        Image ceo_positive_reputation_slider;
        Image ceo_negative_reputation_slider;

        switch (ceo_index)
        {
        case 1:
            ceo_positive_reputation_slider = ceo_1_positive_reputation_slider;
            ceo_negative_reputation_slider = ceo_1_negative_reputation_slider;
            break;

        case 2:
            ceo_positive_reputation_slider = ceo_2_positive_reputation_slider;
            ceo_negative_reputation_slider = ceo_2_negative_reputation_slider;
            break;

        case 3:
            ceo_positive_reputation_slider = ceo_3_positive_reputation_slider;
            ceo_negative_reputation_slider = ceo_3_negative_reputation_slider;
            break;

        case 4:
            ceo_positive_reputation_slider = ceo_4_positive_reputation_slider;
            ceo_negative_reputation_slider = ceo_4_negative_reputation_slider;
            break;

        default:
            Debug.LogError("CEO not recognized");
            return;
        }
    }

    #endregion

    private float money_generated = 0;

    #region Unity methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [Header("Info Location")]
    [SerializeField] private BuildingDictionary building_dictionary;
    [SerializeField] private TimeManager time_manager;

    // Update is called once per frame
    void Update()
    {
        /// TODO: poll money, population, RCI and CEOs reputation with some listeners in their respective manager

        /// Poll money
        float money = building_dictionary.get_money_generation_per_hour() * Time.deltaTime;
        money_generated += money;

        update_money_balance_text(money_generated);
        update_money_change_text(money);

        /// Poll population
        int population = building_dictionary.get_population_count();

        update_current_population_text(population);

        /// Poll date
        DateTime current_day = time_manager.get_current_date();
        double current_daytime = time_manager.get_current_time();

        update_current_date_text(current_day);
        update_day_progress_slider(current_daytime);

    }

    #endregion
}
