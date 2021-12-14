using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = 60f;
    // Start is called before the first frame update
    private bool _started, _finished, _moving, _turningLeft, _turningRight;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_started || _finished)
            return;

        if (Input.GetMouseButtonDown(0) && !_moving)
        {
            _moving = true;
            return;
        }

        if (_moving)
        {
            if (_turningLeft)
                transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
            
            if (_turningRight)
                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
            
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Obstacle"))
        {
            _finished = true;
        }
            
    }

    public void OnLevelLoaded()
    {
        _started = true;
        _moving = false;
    }

    public void TurnLeft(bool pressed)
    {
        _turningLeft = pressed;
    }

    public void TurnRight(bool pressed)
    {
        _turningRight = pressed;
    }
}
