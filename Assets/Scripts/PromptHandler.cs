using UnityEngine;

public class PromptHandler : MonoBehaviour
{
    [SerializeField] private string[] PromptCollection;
    [SerializeField] private TMPro.TMP_Text TutorialPromptText;
    [SerializeField] private TMPro.TMP_Text GamePromptText;
    [SerializeField] private string SelectedPrompt;

    private void OnEnable()
    {
        SelectedPrompt = PromptCollection[Random.Range(0, PromptCollection.Length)];
        TutorialPromptText.text = SelectedPrompt;
        GamePromptText.text = SelectedPrompt;
    }
}
