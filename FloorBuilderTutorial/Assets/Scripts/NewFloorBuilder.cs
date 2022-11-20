using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewFloorBuilder : EditorWindow
{
    private Texture2D image;
    private Dictionary<Color, GameObject> colorToObj = new Dictionary<Color, GameObject>();
    // Add Pair
    private Color newColor = new Color();
    private GameObject newObj = null;

    [MenuItem("Window/FloorBuilder2")]
    private static void BuildWindow()
    {
        GetWindow<NewFloorBuilder>("Floor Builder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Welcome");
        DisplayPair();
        DisplayAddPair();
        DisplayImageSelection();

        bool shouldBuild = GUILayout.Button("Build Floor");
        if (shouldBuild)
        {
            if (image != null)
            {
                BuildFloor();
            }
            else
            {
                throw new System.Exception("No image was provided");
            }
        }
    }

    private void DisplayPair()
    {
        List<Color> colorKeys = new List<Color>(colorToObj.Keys);
        foreach (Color colorKey in colorKeys)
        {
            GUILayout.BeginHorizontal();

            // Color
            Color color = EditorGUILayout.ColorField(colorKey);
            if (colorToObj.ContainsKey(color))
            {
                color = colorKey;
            }
            else
            {
                colorToObj.Add(color, colorToObj[colorKey]);
                colorToObj.Remove(colorKey);
            }
            // GameObject
            colorToObj[color] = (GameObject)EditorGUILayout.ObjectField(colorToObj[color], typeof(GameObject));
            // Remove Pair
            if (GUILayout.Button("Remove Pair"))
            {
                colorToObj.Remove(color);
            }

            GUILayout.EndHorizontal();
        }
    }

    private void DisplayAddPair()
    {
        GUILayout.BeginHorizontal();

        newColor = EditorGUILayout.ColorField(newColor);

        newObj = (GameObject)EditorGUILayout.ObjectField(newObj, typeof(GameObject));

        if (GUILayout.Button("Add Pair"))
        {
            if (colorToObj.ContainsKey(newColor))
            {
                throw new System.Exception("Color is already here");
            }
            else
            {
                colorToObj.Add(newColor, newObj);
            }
        }

        GUILayout.EndHorizontal();
    }

    private void DisplayImageSelection()
    {
        image = (Texture2D)EditorGUILayout.ObjectField(image, typeof(Texture2D));
    }

    private void BuildFloor()
    {
        GameObject floorWrapper = new GameObject("Floor");
        floorWrapper.transform.position = new Vector3(0, 0, 0);

        for (int x = 0; x < image.width; x++)
        {
            for (int y = 0; y < image.height; y++)
            {
                Color pixel = image.GetPixel(x, y);
                if (colorToObj.ContainsKey(pixel))
                {
                    GameObject floorTile = colorToObj[pixel];
                    Instantiate(floorTile, floorWrapper.transform);
                    floorTile.transform.localPosition = new Vector3(x, 0, y);
                }
            }
        }
    }
}
