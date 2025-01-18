using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    
    [SerializeField, Tooltip("Speed of the object following mouse")]
    private float speed = 8.0f;
    [SerializeField, Tooltip("Adjusted to appear on screen correctly")]
    private float distanceFromCamera = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse position from input 
        // TODO: Adjust to different input methods
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamera;
        
        // Determine mouse position's world point relative to screen's bounds
        Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        
        // Gradually move new position towards current mouse position determined by an exponent of speed
        Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * Time.deltaTime));
        transform.position = position;
    }
    
    // Preparation function to modify "mouse sensitivity"
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
