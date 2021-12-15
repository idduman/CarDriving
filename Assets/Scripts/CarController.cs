
using System;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CarDriving
{

    public class CarController : MonoBehaviour
    {
        private float _moveSpeed;
        private float _turnSpeed;

        private bool _started, _finished, _moving, _turningLeft, _turningRight;

        private TurnDirection _direction;
        private CarData _currentCarData;
        private float _timeStamp;
        
        public SteeringData SteeringData { get; private set; }

        private void Awake()
        {
            _moveSpeed = GameController.Instance.CarMovespeed;
            _turnSpeed = GameController.Instance.CarTurnspeed;
        }

        void Update()
        {
            if (!_started || _finished)
                return;

            if (_moving)
            {
                _timeStamp += Time.deltaTime;
                
                if (_direction == TurnDirection.Left)
                    transform.Rotate(Vector3.up, -_turnSpeed * Time.deltaTime);

                if (_direction == TurnDirection.Right)
                    transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

                transform.Translate(_moveSpeed * Time.deltaTime * Vector3.forward);
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
                SteeringData.Add(new Tuple<float, TurnDirection>(_timeStamp, TurnDirection.Stop));
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

            SteeringData = new SteeringData();

            _timeStamp = 0;
        }

        public void Drive()
        {
            _moving = true;
        }

        public void TurnLeft(bool pressed)
        {
            if (pressed)
            {
                if (_direction == TurnDirection.Straight)
                {
                    _direction = TurnDirection.Left;
                    SteeringData.Add(new Tuple<float, TurnDirection>(_timeStamp, TurnDirection.Left));
                }
            }
            else
            {
                if (_direction == TurnDirection.Left)
                {
                    _direction = TurnDirection.Straight;
                    SteeringData.Add(new Tuple<float, TurnDirection>(_timeStamp, TurnDirection.Straight));
                }
            }

        }

        public void TurnRight(bool pressed)
        {
            if (pressed)
            {
                if (_direction == TurnDirection.Straight)
                {
                    _direction = TurnDirection.Right;
                    SteeringData.Add(new Tuple<float, TurnDirection>(_timeStamp, TurnDirection.Right));
                }
            }
            else
            {
                if (_direction == TurnDirection.Right)
                {
                    _direction = TurnDirection.Straight;
                    SteeringData.Add(new Tuple<float, TurnDirection>(_timeStamp, TurnDirection.Straight));
                }
            }
        }
    }
}