using UnityEngine;
using UnityEngine.UI;
using Kinect = Windows.Kinect;

public class KinectController : MonoBehaviour
{
    public static KinectController Instance;
    [SerializeField] private BodySourceManager BodyManager;
    [SerializeField] private int TrackedBodiesCount;

    [Header("Left Hand")]
    [SerializeField] public Kinect.HandState LeftHandState;
    [SerializeField] private Vector3 LeftHandPosition;
    [SerializeField] private Image LeftHandSpriteImage;
    [SerializeField] private Sprite OpenLeftHandSprite;
    [SerializeField] private Sprite ClosedLeftHandSprite;
    [SerializeField] public CanvasDrawer LeftCanvasDrawer;
    [SerializeField] public VirtualHand LeftVirtualHand;
    [Header("Right Hand")]
    [SerializeField] public Kinect.HandState RightHandState;
    [SerializeField] private Vector3 RightHandPosition;
    [SerializeField] private Image RightHandSpriteImage;
    [SerializeField] private Sprite OpenRightHandSprite;
    [SerializeField] private Sprite ClosedRightHandSprite;
    [SerializeField] public CanvasDrawer RightCanvasDrawer;
    [SerializeField] public VirtualHand RightVirtualHand;

    public Kinect.Body[] Bodies { get; private set; } = null;

    private Kinect.CameraSpacePoint _leftHandPos;
    private Kinect.CameraSpacePoint _rightHandPos;
    private int _trackedBodyCount;
    private Kinect.HandState _prevValidRightHandState, _prevValidLeftHandState;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Update colors of each hand
        LeftHandSpriteImage.color = LeftCanvasDrawer.SelectedColor;
        RightHandSpriteImage.color = RightCanvasDrawer.SelectedColor;


        // Handle movement hands
        _trackedBodyCount = 0;
        if (BodyManager == null)
        {
            Debug.LogWarning("Body Manager is not assigned, did you forget?");
            return;
        }

        Bodies = BodyManager.GetData();
        if (Bodies == null || Bodies.Length == 0)
        {
            TrackedBodiesCount = 0;
            return;
        }

        foreach (var body in Bodies)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                _leftHandPos = body.Joints[Kinect.JointType.HandLeft].Position;
                _rightHandPos = body.Joints[Kinect.JointType.HandRight].Position;
                LeftHandPosition = new Vector3(_leftHandPos.X, _leftHandPos.Y, _leftHandPos.Z);
                RightHandPosition = new Vector3(_rightHandPos.X, _rightHandPos.Y, _rightHandPos.Z);

                if (body.HandLeftState != _prevValidLeftHandState && _prevValidLeftHandState.Equals(Kinect.HandState.Open) && body.HandLeftState.Equals(Kinect.HandState.Closed))
                {
                    LeftVirtualHand.DoClick();
                }

                if (body.HandRightState != _prevValidRightHandState && _prevValidRightHandState.Equals(Kinect.HandState.Open) && body.HandRightState.Equals(Kinect.HandState.Closed))
                {
                    RightVirtualHand.DoClick();
                }

                LeftHandState = body.HandLeftState;
                RightHandState = body.HandRightState;

                _trackedBodyCount++;
                UpdateHandSpritesState();
                UpdateHandSpritesLocation();
            }
        }
        TrackedBodiesCount = _trackedBodyCount;
    }

    void UpdateHandSpritesState()
    {
        switch (LeftHandState)
        {
            case Kinect.HandState.Open:
                LeftHandSpriteImage.sprite = OpenLeftHandSprite;
                LeftCanvasDrawer.IsActivated = false;
                break;
            case Kinect.HandState.Closed:
                LeftHandSpriteImage.sprite = ClosedLeftHandSprite;
                LeftCanvasDrawer.IsActivated = true;
                break;
        }

        switch (RightHandState)
        {
            case Kinect.HandState.Open:
                RightHandSpriteImage.sprite = OpenRightHandSprite;
                RightCanvasDrawer.IsActivated = false;
                break;
            case Kinect.HandState.Closed:
                RightHandSpriteImage.sprite = ClosedRightHandSprite;
                RightCanvasDrawer.IsActivated = true;
                break;
        }
    }

    bool IsHandStateValid(Kinect.HandState state)
    {
        return state.Equals(Kinect.HandState.Open) || state.Equals(Kinect.HandState.Closed);
    }

    void UpdateHandSpritesLocation()
    {
        if (IsHandStateValid(RightHandState))
        {
            _prevValidRightHandState = RightHandState;
            RightHandSpriteImage.transform.localPosition = GetPositionOnScreenFromKinect(RightHandPosition);
            RightCanvasDrawer.CurrentCursorPosition = RightHandSpriteImage.transform.position;
            if (RightHandState.Equals(Kinect.HandState.Closed))
            {
                RightCanvasDrawer.StartLine(true);
            }
            else
            {
                RightCanvasDrawer.FinishLine(true);
            }
        }
        else
        {
            //RightCanvasDrawer.FinishLine(true);
        }

        if (IsHandStateValid(LeftHandState))
        {
            _prevValidLeftHandState = LeftHandState;
            LeftHandSpriteImage.transform.localPosition = GetPositionOnScreenFromKinect(LeftHandPosition);
            LeftCanvasDrawer.CurrentCursorPosition = LeftHandSpriteImage.transform.position;
            if (LeftHandState.Equals(Kinect.HandState.Closed))
            {
                LeftCanvasDrawer.StartLine(true);
            }
            else
            {
                LeftCanvasDrawer.FinishLine(true);
            }
        }
        else
        {
            //LeftCanvasDrawer.FinishLine(true);
        }

    }

    Vector2 GetPositionOnScreenFromKinect(Vector3 kinectPosition)
    {
        var newV = new Vector2((kinectPosition.x * Screen.currentResolution.width),
            (kinectPosition.y * Screen.currentResolution.height));
        return newV;
    }
}
