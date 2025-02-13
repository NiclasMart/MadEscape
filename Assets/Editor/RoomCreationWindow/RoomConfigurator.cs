using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

//the markers should scale with the timeline scale
//prevent placing markers on top of each other
//add a way of dragging markers
//add a way to remove markers
//add way to select markers and display data


public class RoomConfigurator : EditorWindow
{
    private List<TimelineMarker> markers = new List<TimelineMarker>();
    private VisualElement timelineContainer;
    private VisualElement markerContainer;
    private VisualElement subdivisionContainer;
    private TextField timelineLengthField;
    private float timelineLength = 100f;

    [MenuItem("Window/Room Configurator")]
    public static void ShowWindow()
    {
        RoomConfigurator wnd = GetWindow<RoomConfigurator>();
        wnd.titleContent = new GUIContent("Room Configurator");
        wnd.minSize = new Vector2(600, 200);
    }

    private void OnEnable()
    {
        // Load UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/RoomCreationWindow/RoomConfigurator.uxml");
        if (visualTree != null)
        {
            VisualElement root = visualTree.Instantiate();
            rootVisualElement.Add(root);

            // Load USS
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/RoomCreationWindow/RoomConfigurator.uss");
            if (styleSheet != null)
            {
                root.styleSheets.Add(styleSheet);
            }

            // Find UI elements
            timelineContainer = root.Q<VisualElement>("timeline");
            markerContainer = root.Q<VisualElement>("markerContainer");
            subdivisionContainer = root.Q<VisualElement>("subdivisionContainer");
            timelineLengthField = root.Q<TextField>("timelineLengthField");

            // Set initial timeline length
            timelineLengthField.value = timelineLength.ToString();
            timelineLengthField.RegisterValueChangedCallback(evt => OnTimelineLengthChanged(evt.newValue));

            // Add click event to timeline
            timelineContainer.RegisterCallback<MouseDownEvent>(evt => OnTimelineClick(evt));

            // Create initial subdivisions
            CreateSubdivisions();
        }
    }

    private void OnTimelineClick(MouseDownEvent evt)
    {
        // Calculate the position as a percentage of the timeline width
        float position = evt.localMousePosition.x / timelineContainer.contentRect.width * timelineLength;
        AddMarker(position);

    }

    private void AddMarker(float clickPosition)
    {
        int position = Mathf.RoundToInt(clickPosition);
        TimelineMarker newMarker = new TimelineMarker(position);
        markers.Add(newMarker);

        VisualElement marker = new VisualElement();
        marker.AddToClassList("marker");
        marker.style.left = new StyleLength(new Length(clickPosition / timelineLength * 100, LengthUnit.Percent));

        marker.RegisterCallback<MouseDownEvent>(evt =>
        {
            Debug.Log("Clicked marker");
        });

        markerContainer.Add(marker);
    }

    private void OnTimelineLengthChanged(string newLength)
    {
        if (float.TryParse(newLength, out float length))
        {
            timelineLength = length;
            UpdateMarkers();
            CreateSubdivisions();
        }
    }

    private void UpdateMarkers()
    {
        markerContainer.Clear();

        foreach (var markerData in markers)
        {
            VisualElement marker = new VisualElement();
            marker.AddToClassList("marker");
            Debug.Log(markerData.position);
            marker.style.left = new StyleLength(new Length(markerData.position / timelineLength * 100, LengthUnit.Percent));
            markerContainer.Add(marker);
        }
    }

    private void CreateSubdivisions()
    {
        subdivisionContainer.Clear();

        for (int i = 0; i <= timelineLength; i++)
        {
            VisualElement subdivision = new VisualElement();
            if (i % 5 == 0)
            {
                subdivision.AddToClassList("subdivision-5sec");
            }
            else
            {
                subdivision.AddToClassList("subdivision");
            }
            subdivision.style.left = new StyleLength(new Length(i / timelineLength * 100, LengthUnit.Percent));
            subdivisionContainer.Add(subdivision);
        }
    }
}

public class TimelineMarker
{
    public float position;
    public string type;
    public int enemyCount;

    public TimelineMarker(float position, string type = "Enemy", int enemyCount = 1)
    {
        Debug.Log(position);
        this.position = position;
        this.type = type;
        this.enemyCount = enemyCount;
    }
}
