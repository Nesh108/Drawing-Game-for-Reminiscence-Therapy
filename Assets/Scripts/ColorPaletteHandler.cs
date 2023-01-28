using UnityEngine;
using UnityEngine.UI;

public class ColorPaletteHandler : MonoBehaviour
{
    public static ColorPaletteHandler Instance;

    public NamedColors[] AvailableColors;
    public GameObject CanvasBrushPrefab;
    public Transform LinesContainer;
    public GameObject ColorPrefab;
    public Transform ColorPaletteContainer;

    void Awake()
    {
        Instance = this;
        InstantiateColorPalette();
    }

    void InstantiateColorPalette()
    {
        GameObject go;
        Button b;
        int index = 0;
        foreach(NamedColors nc in AvailableColors)
        {
            go = Instantiate(ColorPrefab, Vector3.zero, Quaternion.identity, ColorPaletteContainer);
            go.transform.SetSiblingIndex(index);
            go.name = nc.ColorName;
            b = go.GetComponentInChildren<Button>();
            b.GetComponent<Image>().color = nc.ColorValue;
            b.GetComponent<KinectColorButton>().ColorName = nc.ColorName;
            index++;
        }
    }
}

[System.Serializable]
public class NamedColors
{
    [SerializeField] public string ColorName;
    [SerializeField] public Color ColorValue;
}