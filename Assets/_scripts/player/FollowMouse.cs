using System.Collections;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the object following mouse")]
    private float speed = 8.0f;

    [SerializeField, Tooltip("Height above ground")]
    private float cameraDistanceBuffer = .3f;

    [SerializeField, Tooltip("Ray lifetime")]
    private float particleLifeTime = .3f;

    private float _distanceFromCamera;

    [Header("Debug"), Tooltip("Set to True to draw the hit circle"), SerializeField]
    private bool debug;

    private void Start()
    {
        _distanceFromCamera = Camera.main.transform.position.y - cameraDistanceBuffer;
    }

    // Update is called once per frame
    public void Update()
    {
        FollowMousePosition();
    }

    private void FollowMousePosition()
    {
        // Get mouse position from input 
        // TODO: Adjust to different input methods
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _distanceFromCamera;

        // Determine mouse position's world point relative to screen's bounds
        Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

        // Gradually move new position towards current mouse position determined by an exponent of speed
        Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld,
            1.0f - Mathf.Exp(-speed * Time.deltaTime));
        transform.position = position;
    }

    // Preparation function to modify "mouse sensitivity"
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}