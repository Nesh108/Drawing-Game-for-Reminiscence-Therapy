using UnityEngine;

public class KinectColorButton : MonoBehaviour
{
    public bool IsLeftHandHovering;
    public bool IsRightHandHovering;
    public string ColorName;

    void OnEnable()
    {
        IsLeftHandHovering = false;
        IsRightHandHovering = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Left_Hand"))
        {
            IsLeftHandHovering = true;
        }
        else if (collision.CompareTag("Right_Hand"))
        {
            IsRightHandHovering = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Left_Hand"))
        {
            IsLeftHandHovering = false;
        }
        else if (collision.CompareTag("Right_Hand"))
        {
            IsRightHandHovering = false;
        }
    }

    public void OnClick()
    {
        if (IsLeftHandHovering)
        {
            KinectController.Instance.LeftCanvasDrawer.ChangeColor(ColorName);
        }
        if (IsRightHandHovering)
        {
            KinectController.Instance.RightCanvasDrawer.ChangeColor(ColorName);
        }
/*        else
        {
            KinectController.Instance.LeftCanvasDrawer.ChangeColor(ColorName);
            KinectController.Instance.RightCanvasDrawer.ChangeColor(ColorName);
        }*/
    }
}
