using UnityEngine;
using System.Collections.Generic;
using TMPro;

using UnityEngine.UI; // Required when Using UI elements.

public class Panel {
    
    public Panel(string name, GameObject panel) {
        this.name = name;
        this.panel = panel;
    }
    
    public GameObject panel;
    public string name;
}

public class MenuManager : MonoBehaviour
{
    public GameObject colorSlider;
    public GameObject rowSlider;
    public GameObject columnSlider;
    
    public GameObject MainMenuP;
    public GameObject OptionP;
    public GameObject GameP;
    public GameObject PauseP;
    public GameObject GameOverP;
    
    private static List<Panel> panels;

    void Start() {
        panels = new List<Panel>();
        addAllPanel();
        SetAllSlidersValues();
    }

    private void SetAllSlidersValues() {
        //Debug.LogWarning("color slider ="+colorSlider.GetComponentInParent<Slider>().minValue);
        ColorSlider(colorSlider.GetComponentInParent<Slider>().minValue);
        RowSlider(rowSlider.GetComponentInParent<Slider>().minValue);
    }
    
    private void addAllPanel() {
        panels.Add(new Panel("MainMenuPanel", MainMenuP));
        panels.Add(new Panel("OptionPanel", OptionP));
        panels.Add(new Panel("PausePanel", PauseP));
        panels.Add(new Panel("GamePanel", GameP));
        panels.Add(new Panel("GameOverPanel", GameOverP));
    }
    
    
    public static GameObject GetPanel (string name) {
        foreach (var cPanel in panels) {
            if (cPanel.name == name) {
                return cPanel.panel;
            }
        }
        return null;
    }
    
    public void GoToMainMenu() {
        Debug.LogWarning("go to main menu called");
        FindObjectOfType<GameManager>().EndGame();
        CloseAllPanel();
        OpenPanel("MainMenuPanel");
    }

    public void OpenPause() {
        OpenPanel("PausePanel");
    }
    
    public void ClosePause() {
        ClosePanel("PausePanel");
    }
    
    public void OpenOptions() {
        OpenPanel("OptionPanel");
    }
    
    public void CloseOptions() {
        ClosePanel("OptionPanel");
    }
    
    public void OpenGameover() {
        OpenPanel("GameOverPanel");
    }
    
    public void StartGame() {
        CloseAllPanel();
        OpenPanel("GamePanel");
        FindObjectOfType<GameManager>().StartGame();
    }
    
    public void ColorSlider(float value) {
        GameSettings.ColorRange = (int)value;
        colorSlider.GetComponent<TextMeshProUGUI>().text = value.ToString();
    }
    
    public void RowSlider(float value) {
        GameSettings.row = (int)value;
        GameSettings.column = (int)value;
        rowSlider.GetComponent<TextMeshProUGUI>().text = value.ToString();
    }
    
    public void ColumnSlider(float value) {
        GameSettings.column = (int)value;
        columnSlider.GetComponent<TextMeshProUGUI>().text = value.ToString();
    }
    
    private void CloseAllPanel() {
        panels.ForEach(cPanel => cPanel.panel.SetActive(false));
    }
    
    private void ClosePanel(string name) {
        foreach (var cPanel in panels) {
            if (cPanel.name == name) {
                cPanel.panel.SetActive(false);
            }
        }
    }
    
    private void OpenPanel(string name) {
        foreach (var cPanel in panels) {
            if (cPanel.name == name) {
                cPanel.panel.SetActive(true);
            }
        }
    }
    
}

