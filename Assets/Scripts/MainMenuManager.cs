using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region === Variabili accessibili dall'Editor ===

    [SerializeField] private GameObject MainView;
    [SerializeField] private GameObject SettingsView;
    [SerializeField] private TMP_Dropdown ResolutionDropdown;
    [SerializeField] private Toggle FullscreenToggle;

    #endregion

    #region === Variabili private ===

    private Resolution[] _resolutions;
    private List<string> _resolutionOptions = new List<string>();
    private int _currentResolutionIndex = 0;
    private Vector2Int _currentResolution;
    private bool _isFullscreen;

    #endregion

    // Funzione chiamata quando il gioco comincia (solo se lo script/oggetto e' attivo)
    private void Start()
    {
        InitializeSettings();
    }

    // Inizializza il menu delle impostazioni
    private void InitializeSettings()
    {
        LoadSettings();
        _resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            _resolutionOptions.Add($"{_resolutions[i].width}x{_resolutions[i].height}"); // Formatta la resoluzione: ex. '1920x1080'

            if (_resolutions[i].width.Equals(_currentResolution.x) && _resolutions[i].height.Equals(_currentResolution.y))
            {
                _currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(_resolutionOptions);
        ResolutionDropdown.value = _currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    // Carica la scena del gioco
    public void StartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    // Mostra la schermata iniziale
    public void ShowMainView()
    {
        MainView.SetActive(true);
        SettingsView.SetActive(false);
    }

    // Mostra la schermata delle impostazioni
    public void ShowSettingsView()
    {
        MainView.SetActive(false);
        SettingsView.SetActive(true);
    }

    // Esci dal gioco (si utilizzano due metodi diversi a seconda da dov'e' chiamata la funzione)
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Imposta la nuova resoluzione / schermata intera
    public void OnResolutionChanged()
    {
        string newSelection = ResolutionDropdown.options[ResolutionDropdown.value].text;

        SetResolution(newSelection, FullscreenToggle.isOn);
    }

    private void SetResolution(string s, bool isFullscreen)
    {
        // Converte l'opzione da string a 2 integer (width, heigth)
        string[] newResolution = s.Split('x');
        int newWidth = int.Parse(newResolution[0]);
        int newHeight = int.Parse(newResolution[1]);

        Screen.SetResolution(newWidth, newHeight, isFullscreen);
        _currentResolution.x = newWidth;
        _currentResolution.y = newHeight;
        _isFullscreen = isFullscreen;
    }

    // Fai ritornare le opzioni al loro stato precedente
    public void RevertChanges()
    {
        InitializeSettings();
    }

    // Salva le impostazioni
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("SelectedResolutionWidth", _currentResolution.x);
        PlayerPrefs.SetInt("SelectedResolutionHeight", _currentResolution.y);
        PlayerPrefs.SetInt("IsFullscreen", FullscreenToggle.isOn ? 1 : 0);
    }

    // Carica le impostazioni salvate (se ce ne sono), altrimenti usa le impostazioni attuali
    public void LoadSettings()
    {
        _currentResolution.x = PlayerPrefs.GetInt("SelectedResolutionWidth", Screen.currentResolution.width);
        _currentResolution.y = PlayerPrefs.GetInt("SelectedResolutionHeight", Screen.currentResolution.height);
        _isFullscreen = PlayerPrefs.GetInt("IsFullscreen", Screen.fullScreen ? 1 : 0) == 1;
        FullscreenToggle.SetIsOnWithoutNotify(_isFullscreen);
        Screen.SetResolution(_currentResolution.x, _currentResolution.y, _isFullscreen);
    }

    // Funzione per pulire le impostazioni salvate del menu, utile per fare dei test
    public void ClearSettings()
    {
        PlayerPrefs.DeleteKey("SelectedResolutionWidth");
        PlayerPrefs.DeleteKey("SelectedResolutionHeight");
        PlayerPrefs.DeleteKey("IsFullscreen");
    }
}
