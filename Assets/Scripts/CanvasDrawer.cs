using UnityEngine;

public class CanvasDrawer : MonoBehaviour
{
    public static int LineCounter;
    [SerializeField] private bool UseMouse = true;

    [HideInInspector] public Vector2 CurrentCursorPosition;
    [HideInInspector] public bool IsActivated;

    [Header("Properties")]
    public Color SelectedColor = Color.black;
    public static float StrokeWidth = 0.5f;
    public static float StrokeEndWidthRatio = 0.8f;
    public float ThresholdDrawingDistance = 0.2f;
    public RectTransform TopToolArea;
    public RectTransform BottomToolArea;
    public float AreaOffset; // The top and bottom areas need some offset to be tight

    private Camera _mainCamera;
    private bool _isDrawing;
    private Vector2 _cursorPositionWorld;
    private Vector2 _currentScreenPos;
    private LineRenderer _canvasLineRenderer;
    private bool _prevActivateState;
    private Vector2 _prevLocation;
    private bool _hasLostConnection;

    void OnEnable()
    {
        LineCounter = 0;
        _mainCamera = Camera.main;
        _prevActivateState = false;
        IsActivated = false;
        _hasLostConnection = false;
    }

    void Update()
    {
        if (!TutorialHandler.Instance.IsGameStarted)
        {
            return;
        }

        if (UseMouse)
        {
            CurrentCursorPosition = Input.mousePosition;

            _cursorPositionWorld = _mainCamera.ScreenToWorldPoint(CurrentCursorPosition);
            _currentScreenPos = CurrentCursorPosition;
        }
        else
        {
            _cursorPositionWorld = CurrentCursorPosition;
            _currentScreenPos = new Vector2(_mainCamera.WorldToScreenPoint(CurrentCursorPosition).x, _mainCamera.WorldToScreenPoint(CurrentCursorPosition).y);
        }


        if (UseMouse)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartLine();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                FinishLine();
            }
        }
        else
        {
            if (IsActivated != _prevActivateState)
            {
                if (IsActivated)
                {
                    StartLine();
                }
                else
                {
                    FinishLine();
                }

                _prevActivateState = IsActivated;
            }
        }


        // Testing
        if (Input.GetKeyUp(KeyCode.C))
        {
            ClearDrawing();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            ChangeColor("Pink");
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            ChangeColor("Black");
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            ChangeColor("Blue");
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            ChangeColor("Green");
        }
    }

    void LateUpdate()
    {
        if (!TutorialHandler.Instance.IsGameStarted)
        {
            return;
        }

        // Drawing

        if (_isDrawing && _canvasLineRenderer != null && Vector2.Distance(_cursorPositionWorld, _prevLocation) >= ThresholdDrawingDistance)
        {
            if (_canvasLineRenderer.positionCount > 250)
            {
                StartLine();
            }

            if (_canvasLineRenderer != null && IsPointWithinDrawingArea(_currentScreenPos))
            {
                _canvasLineRenderer.positionCount++;
                _canvasLineRenderer.SetPosition(_canvasLineRenderer.positionCount - 1, _cursorPositionWorld);
                _prevLocation = _cursorPositionWorld;
            }
        }
    }

    bool IsPointWithinDrawingArea(Vector2 pt)
    {
        float normalizedPoint = Screen.height - pt.y;

        return normalizedPoint >= (TopToolArea.rect.height - AreaOffset) && normalizedPoint <= (Screen.height - (BottomToolArea.rect.height - AreaOffset));
    }

    public void StartLine(bool onlyReset = false)
    {
        if (onlyReset)
        {
            if (_hasLostConnection && _canvasLineRenderer != null && _canvasLineRenderer.positionCount < 1)
            {
                // Resume as nothing happened
                _isDrawing = true;
                _hasLostConnection = false;
            }
        }
        else if (IsPointWithinDrawingArea(_currentScreenPos))
        {
            _isDrawing = true;
            LineCounter++;
            _canvasLineRenderer =
                Instantiate(ColorPaletteHandler.Instance.CanvasBrushPrefab, Vector3.zero, Quaternion.identity,
                    ColorPaletteHandler.Instance.LinesContainer).GetComponent<LineRenderer>();
            _canvasLineRenderer.sortingOrder = LineCounter;

            _canvasLineRenderer.positionCount = 0;
            _canvasLineRenderer.startWidth = StrokeWidth;
            _canvasLineRenderer.endWidth = StrokeWidth * StrokeEndWidthRatio;

            _canvasLineRenderer.material.color = SelectedColor;
        }
    }

    public void ClearDrawing()
    {
        foreach (Transform child in ColorPaletteHandler.Instance.LinesContainer)
        {
            Destroy(child.gameObject);
        }
        LineCounter = 0;
    }

    public void ChangeColor(string colorName)
    {
        ChangeColor(GetAvailableColorByName(colorName));
    }

    void ChangeColor(Color c)
    {
        SelectedColor = c;
    }

    public void FinishLine(bool lostConnection = false)
    {
        _hasLostConnection = lostConnection;
        _isDrawing = false;
    }

    public void SetBrushStroke(float value)
    {
        StrokeWidth = value;
    }

    Color GetAvailableColorByName(string colorName)
    {
        return GetAvailableColorByName(colorName, Color.black);
    }

    Color GetAvailableColorByName(string colorName, Color defaultColor)
    {
        foreach (NamedColors nc in ColorPaletteHandler.Instance.AvailableColors)
        {
            if (nc.ColorName.Equals(colorName, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return nc.ColorValue;
            }
        }
        return defaultColor;
    }

}