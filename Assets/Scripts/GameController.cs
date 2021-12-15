using System;
using System.Collections;
using System.Collections.Generic;
using CarDriving;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace CarDriving
{

    public class GameController : SingletonBehaviour<GameController>
    {
        [SerializeField] private CarController _playerCar;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private ReplayCarBehaviour _replayCarPrefab;
        [SerializeField] private float _carMovespeed;
        [SerializeField] private float _carTurnspeed;
        [SerializeField] private Transform _arrowStart;
        [SerializeField] private Transform _arrowEnd;

        private List<SteeringData> _currentSteeringData = new List<SteeringData>();
        private List<ReplayCarBehaviour> _currentReplayCars = new List<ReplayCarBehaviour>();

        private bool _driving;

        public LevelBehaviour CurrentLevel { get; private set; }

        public int CurrentCar { get; private set; }

        public int PlayerLevel { get; private set; }

        public float CarMovespeed => _carMovespeed;

        public float CarTurnspeed => _carTurnspeed;

        void Start()
        {
            PlayerLevel = PlayerPrefs.GetInt("Level", 0);
            CurrentCar = 0;
            Load();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _playerCar.Drive();
                DriveReplayCars();
            }
        }

        public void Load()
        {
            CurrentCar = 0;
            _levelController.Load(PlayerLevel);
        }

        public void OnLevelLoaded(LevelBehaviour level)
        {
            if (!_playerCar)
            {
                Debug.LogError("No car found in level");
                return;
            }

            CurrentLevel = level;
            _currentSteeringData.Clear();
            InitializeArrows();
            _playerCar.ResetCar(CurrentCar);
            _driving = false;
        }

        public void OnCarDriven(bool success)
        {
            if (success)
            {
                CurrentCar++;
                if (CurrentCar >= CurrentLevel.GetCarCount())
                {
                    Finish(true);
                    return;
                }
                _currentSteeringData.Add(_playerCar.SteeringData);
            }
            InitializeArrows();
            InitializeReplayCars();
            _playerCar.ResetCar(CurrentCar);
        }

        public void DriveReplayCars()
        {
            foreach (var rCar in _currentReplayCars)
            {
                rCar.Drive();
            }
        }

        public void TurnLeft(bool pressed)
        {
            if (!_playerCar)
                return;

            _playerCar.TurnLeft(pressed);
        }

        public void TurnRight(bool pressed)
        {
            if (!_playerCar)
                return;

            _playerCar.TurnRight(pressed);
        }

        private void Finish(bool success)
        {
            Debug.Log("Finished");
            if (success)
            {
                PlayerLevel++;
            }

            StartCoroutine(FinishRoutine(success));
        }

        private void InitializeReplayCars()
        {
            foreach (var car in _currentReplayCars)
            {
                Destroy(car.gameObject);
            }
            _currentReplayCars.Clear();
            
            for (int i = 0; i < _currentSteeringData.Count; i++)
            {
                var car = Instantiate(_replayCarPrefab);
                car.Initialize(CurrentLevel.GetCarData(i), _currentSteeringData[i]);
                _currentReplayCars.Add(car);
            }
        }

        private void InitializeArrows()
        {
            var carData = CurrentLevel.GetCarData(CurrentCar);
            var positions = carData.GetCheckpointPositions(3f);
            var rotations = carData.GetCheckpointRotations();
            
            _arrowStart.position = new Vector3(positions.Item1.x, _arrowStart.position.y, positions.Item1.z);
            _arrowEnd.position = new Vector3(positions.Item2.x, _arrowEnd.position.y, positions.Item2.z);

            _arrowStart.rotation = rotations.Item1;
            _arrowEnd.rotation = rotations.Item2;
        }

        private IEnumerator FinishRoutine(bool success)
        {
            yield return new WaitForEndOfFrame();
            Load();
        }
    }
}
