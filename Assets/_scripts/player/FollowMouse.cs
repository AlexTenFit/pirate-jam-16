using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;

public class FollowMouse : MonoBehaviour
{
    
    [SerializeField, Tooltip("Speed of the object following mouse")]
    private float speed = 8.0f;

    [SerializeField, Tooltip("Height above ground")]
    private float cameraDistanceBuffer = .3f;
    
    [SerializeField, Tooltip("Ray lifetime")]
    private float particleLifeTime = .3f;

    [SerializeField, Tooltip("Ray lifetime")]
    private LayerMask enemyRayCastMask;

    [SerializeField, Tooltip("Ray radius")]
    private float rayCastRadius =  1f;
    
    private float _distanceFromCamera;

    [Header("Debug"), Tooltip("Set to True to draw the hit circle"), SerializeField] private bool debug;
    
    private void Start()
    {
        _distanceFromCamera = Camera.main.transform.position.y - cameraDistanceBuffer;
    }

    // Update is called once per frame
    public void Update()
    {
        FollowMousePosition();
        RayCastCollision();
    }
    
    private void RayCastCollision()
    {
        // Determine a trajectory depending on the mouse's position on the screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        StartCoroutine(CastRay(ray));
    }

    private IEnumerator CastRay(Ray ray)
    {
        var particleLifeTimer = 0f;
        while (particleLifeTimer < particleLifeTime)
        {
            if (debug)
            {
                DebugExtension.DebugWireSphere(ray.origin + (ray.direction * _distanceFromCamera), Color.green, rayCastRadius);
                Debug.Log("Ray origin: " + ray.origin);
            }
            if (Physics.SphereCast(ray, rayCastRadius, out var hit, _distanceFromCamera, enemyRayCastMask))
            {
                Destroy(hit.transform.gameObject);
            }
            
            particleLifeTimer += Time.deltaTime;
        }

        yield return null;
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
        Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * Time.deltaTime));
        transform.position = position;
    }

    // Preparation function to modify "mouse sensitivity"
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
