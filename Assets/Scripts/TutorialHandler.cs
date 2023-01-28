using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public static TutorialHandler Instance;
    [SerializeField] private GameObject TutorialView;
    [SerializeField] private GameObject StoryTellerView;
    [SerializeField] private GameObject ArtistView;
    [SerializeField] private GameObject CountdownView;
    [SerializeField] private TMPro.TMP_Text CountdownText;
    [SerializeField] private GameObject CountdownControls;
    [SerializeField] private PromptHandler PromptHandler;

    public bool IsGameStarted;

    private float _timer;
    private bool _isCountdownStarted;

    void Awake()
    {
        Instance = this;

    }

    void OnEnable()
    {
        IsGameStarted = !TutorialView.activeSelf;
        _timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isCountdownStarted)
        {
            _timer -= Time.deltaTime;
            CountdownText.text = $"{(int)_timer}...";

            if(_timer <= 0)
            {
                CountdownText.text = "GO!";
                if(_timer <= -1)
                {
                    GoToGame();
                }
            }
        }
    }

    public void StartTutorial()
    {
        _isCountdownStarted = false;
        _timer = 0f;
        TutorialView.SetActive(true);
        StoryTellerView.SetActive(true);
        ArtistView.SetActive(false);
        CountdownView.SetActive(false);
    }

    public void GoToArtistView()
    {
        TutorialView.SetActive(true);
        StoryTellerView.SetActive(false);
        ArtistView.SetActive(true);
        CountdownView.SetActive(false);
    }

    public void GoToCountdown()
    {
        TutorialView.SetActive(true);
        StoryTellerView.SetActive(false);
        ArtistView.SetActive(false);
        CountdownView.SetActive(true);
        CountdownControls.SetActive(true);
        CountdownText.text = "Ready?";
    }

    public void StartCountdown()
    {
        _isCountdownStarted = true;
        CountdownControls.SetActive(false);
        _timer = 4;
    }

    public void GoToGame()
    {
        _isCountdownStarted = false;
        _timer = 0f;
        TutorialView.SetActive(false);
        StoryTellerView.SetActive(false);
        ArtistView.SetActive(false);
        CountdownView.SetActive(false);
        IsGameStarted = true;
    }
}
