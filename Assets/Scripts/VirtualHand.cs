using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualHand : MonoBehaviour
{
    IPointerClickHandler _currentClickHandler;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Left_Hand") && !collider.CompareTag("Right_Hand"))
        {
            _currentClickHandler = collider.gameObject.GetComponent<IPointerClickHandler>();
            foreach (IPointerEnterHandler ienter in collider.gameObject.GetComponents<IPointerEnterHandler>())
            {
                if (ienter != null)
                {
                    ienter.OnPointerEnter(new PointerEventData(EventSystem.current));
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Left_Hand") && !collider.CompareTag("Right_Hand"))
        {
            _currentClickHandler = null;
            foreach (IPointerExitHandler iexit in collider.gameObject.GetComponents<IPointerExitHandler>())
            {
                if (iexit != null)
                {
                    iexit.OnPointerExit(new PointerEventData(EventSystem.current));
                }
            }
        }
    }

    public void DoClick()
    {
        if (_currentClickHandler != null)
        {
            _currentClickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
}
