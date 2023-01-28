using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
