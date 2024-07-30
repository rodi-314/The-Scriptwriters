using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour
{
    public Canvas mapCanvas; // Reference to the Canvas component
    public RectTransform mapImageRectTransform; // Reference to the RectTransform of the map image
    public float zoomSpeed = 0.1f; // Speed of zooming in and out
    public float minZoom = 0.5f; // Minimum zoom level
    public float maxZoom = 2.0f; // Maximum zoom level

    private bool visible = false;
    private Vector3 initialScale;
    private bool isDragging = false;
    private Vector3 dragStartPosition;

    void Start()
    {
        // Ensure mapCanvas is set
        if (mapCanvas == null)
        {
            Debug.LogError("mapCanvas is not assigned.");
            enabled = false;
            return;
        }

        // Ensure mapImageRectTransform is set
        if (mapImageRectTransform == null)
        {
            Debug.LogError("mapImageRectTransform is not assigned.");
            enabled = false;
            return;
        }

        // Set the initial state to be hidden
        mapCanvas.gameObject.SetActive(false);
        initialScale = mapImageRectTransform.localScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            visible = !visible;
            mapCanvas.gameObject.SetActive(visible);

            if (!visible)
            {
                ResetDragState();
            }
        }

        if (visible)
        {
            HandleZoom();
            HandleDrag();
        }
    }

    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (scrollData != 0.0f)
        {            
            Vector3 newScale = mapImageRectTransform.localScale * (1 + scrollData * zoomSpeed);
           
            newScale.x = Mathf.Clamp(newScale.x, initialScale.x * minZoom, initialScale.x * maxZoom);
            newScale.y = Mathf.Clamp(newScale.y, initialScale.y * minZoom, initialScale.y * maxZoom);

            mapImageRectTransform.localScale = newScale;
        }
    }

    void HandleDrag()
    {
        // Check if the right mouse button is pressed over the map image before starting the drag
        if (Input.GetMouseButtonDown(1) && RectTransformUtility.RectangleContainsScreenPoint(mapImageRectTransform, Input.mousePosition))
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition;
            Debug.Log("Drag Start Position: " + dragStartPosition);
        }

        // Stop dragging when the right mouse button is released
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            Debug.Log("Drag End Position: " + Input.mousePosition);
        }

        // Continue dragging while the right mouse button is held down
        if (isDragging)
        {
            Vector3 difference = Input.mousePosition - dragStartPosition;
            mapImageRectTransform.anchoredPosition += new Vector2(difference.x, difference.y);
            dragStartPosition = Input.mousePosition;
            Debug.Log("Dragging: " + difference);
        }
    }

    void ResetDragState()
    {
        isDragging = false;
        dragStartPosition = Vector3.zero;
    }

    public void OnPlayButton()
    {
        // Logic for the play button can be implemented here
        visible = !visible;
        mapCanvas.gameObject.SetActive(visible);
        dragStartPosition = Vector3.zero;
        Debug.Log("Play button pressed.");
    }
}
