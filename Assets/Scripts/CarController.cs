using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float turnSpeed = 90f;

    private bool _started, _finished, _moving, _turningLeft, _turningRight;

    private CarData _currentCarData;
    
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
            _started = false;
            GameController.Instance.OnCarDriven(false);
        }
        else if (other.collider.CompareTag("Checkpoint"))
        {
            GameController.Instance.OnCarDriven(other.transform == _currentCarData.EndPoint);
        }
    }

    public void ResetCar(int index)
    {
        _currentCarData = GameController.Instance.CurrentLevel.GetCarData(index);
        
        if (_currentCarData == null || !_currentCarData.StartPoint)
            return;
        
        transform.position = _currentCarData.StartPoint.position;
        transform.rotation = _currentCarData.StartPoint.rotation;
        
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
