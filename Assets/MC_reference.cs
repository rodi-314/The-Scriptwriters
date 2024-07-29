using UnityEngine;

public class MCReference : MonoBehaviour
{
    public RectTransform mapImageRectTransform; // Reference to the RectTransform of the map image
    public RectTransform markerRectTransform; // Reference to the RectTransform of the marker
    public Transform mcTransform; // Reference to the MC's transform

    void Update()
    {
        if (mcTransform != null && markerRectTransform != null)
        {
            // Convert MC's world position to the map's local position
            Vector2 mcPositionOnMap = WorldToMapPosition(mcTransform.position);
            markerRectTransform.anchoredPosition = mcPositionOnMap;

            Debug.Log($"MC World Position: {mcTransform.position}");
            Debug.Log($"Marker Position on Map: {mcPositionOnMap}");
        }
    }

    Vector2 WorldToMapPosition(Vector3 worldPosition)
    {
        // Convert world position to the map's local position
        // Adjust the conversion based on your map's coordinate system
        float mapWidth = 1909.883f;
        float mapHeight = 824.3221f;

        // Assuming world origin (0,0) maps to the bottom-left of the map
        float worldWidth = 220f; // Adjust based on your world size
        float worldHeight = 95f; // Adjust based on your world size

        // Convert world position to map position
        float x = (worldPosition.x / worldWidth) * mapWidth;
        float y = (worldPosition.y / worldHeight) * mapHeight;

        // No need to offset for map's anchored position in this case
        return new Vector2(x, y);
    }
}
