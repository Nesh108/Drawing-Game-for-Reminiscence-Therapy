using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawingPrinter : MonoBehaviour
{
    public static int ID = 0;

    public void GoToNextLevel()
    {
        PrintView();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void PrintView()
    {
        ID += 1;
        string scrName = $"screenshot_{ID}.png";
        if (!System.IO.File.Exists(scrName))
        {
            ScreenCapture.CaptureScreenshot(scrName);
        }
        else
        {
            // Retry
            PrintView();
        }
    }
}
