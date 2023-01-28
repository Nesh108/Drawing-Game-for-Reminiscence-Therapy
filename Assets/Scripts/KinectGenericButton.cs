using UnityEngine;
using UnityEngine.Events;

public class KinectGenericButton : MonoBehaviour
{
    public UnityEvent Event;

    public void OnClick()
    {
        Event.Invoke();
    }
}
