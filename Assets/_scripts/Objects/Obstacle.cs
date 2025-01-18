using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float moveSpeed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("chosen").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Vector3.Lerp(transform.position, _target.position, moveSpeed * Time.deltaTime);
        transform.position = position;
    }
}
