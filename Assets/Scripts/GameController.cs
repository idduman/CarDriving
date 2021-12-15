using System;
using System.Collections;
using System.Collections.Generic;
using CarDriving;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace CarDriving
{
    /// <summary>
    /// Main controller script for the game,
    /// parameters are given as serialized fields from the editor
    /// </summary>
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

        public LevelBehaviour CurrentLevel { get; private set; } // Reference to the currently loaded level

        public int CurrentCar { get; private set; } // Current player stage number in the level

        public int PlayerLevel { get; private set; } // Current player level, increments indefinitely but loops in level loader

        public float CarMovespeed => _carMovespeed; // parameter auto method for public access

        public float CarTurnspeed => _carTurnspeed; // parameter auto method for public access

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

        // Method for loading a new or existing level
        public void Load()
        {
            CurrentCar = 0;
            _levelController.Load(PlayerLevel);
        }

        //This method is called whenever the level completes its loading routine
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

        // this method is called whenever player car crashes into something
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

        //Method for starting the movement of all replay cars
        public void DriveReplayCars()
        {
            foreach (var rCar in _currentReplayCars)
            {
                rCar.Drive();
            }
        }

        // Steering event trigger function
        public void TurnLeft(bool pressed)
        {
            if (!_playerCar)
                return;

            _playerCar.TurnLeft(pressed);
        }

        // Steering event trigger function
        public void TurnRight(bool pressed)
        {
            if (!_playerCar)
                return;

            _playerCar.TurnRight(pressed);
        }

        // This method is called on level is completed or failed
        private void Finish(bool success)
        {
            Debug.Log("Finished");
            if (success)
            {
                PlayerLevel++;
            }
            
            DestroyReplayCars();
            StartCoroutine(FinishRoutine(success));
        }

        //This method initializes all replay cars that are present in current level
        private void InitializeReplayCars()
        {
            DestroyReplayCars();
            
            for (int i = 0; i < _currentSteeringData.Count; i++)
            {
                var car = Instantiate(_replayCarPrefab);
                car.Initialize(CurrentLevel.GetCarData(i), _currentSteeringData[i]);
                _currentReplayCars.Add(car);
            }
        }

        //This method desrtoys all replay cars that are present in current level
        private void DestroyReplayCars()
        {
            foreach (var car in _currentReplayCars)
            {
                Destroy(car.gameObject);
            }
            _currentReplayCars.Clear();
        }

        // This method sets the positions and rotations of the indicator arrows relative to the checkpoints
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

        //coroutine for new level load
        private IEnumerator FinishRoutine(bool success)
        {
            yield return new WaitForEndOfFrame();
            Load();
        }
    }
}
