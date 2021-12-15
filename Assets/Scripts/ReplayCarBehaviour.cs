using System;
using System.Collections;
using System.Collections.Generic;
using CarDriving;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ReplayCarBehaviour : MonoBehaviour
{
    private SteeringData _steeringData;
    private TurnDirection _direction;
    private float _moveSpeed, _turnSpeed, _timeStamp;
    private bool _driving;

    private Tuple<float, TurnDirection> _nextTurn;

    private void Awake()
    {
        _driving = false;
        _moveSpeed = GameController.Instance.CarMovespeed;
        _turnSpeed = GameController.Instance.CarTurnspeed;
    }
    
    void Update()
    {
        if (!_driving)
            return;
        
        _timeStamp += Time.deltaTime;

        if (_nextTurn != null && _timeStamp >= _nextTurn.Item1)
        {
            _direction = _nextTurn.Item2;
            _nextTurn = _steeringData.GetNext(_timeStamp);
        }

        if (_direction == TurnDirection.Left)
            transform.Rotate(Vector3.up, -_turnSpeed * Time.deltaTime);

        if (_direction == TurnDirection.Right)
            transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        transform.Translate(_moveSpeed * Time.deltaTime * Vector3.forward);
    }

    public void Initialize(CarData carData, SteeringData steeringData)
    {
        if (carData == null || steeringData == null)
            return;
        
        _steeringData = steeringData;
        transform.position = carData.StartPoint.position;
        transform.rotation = carData.StartPoint.rotation;
        _timeStamp = 0f;
        _nextTurn = steeringData.GetNext(_timeStamp);
        _direction = TurnDirection.Straight;
    }

    public void Drive()
    {
        if (_steeringData == null)
        {
            Debug.LogError($"No steering data found in {name}");
        }
        _driving = true;
    }
}
