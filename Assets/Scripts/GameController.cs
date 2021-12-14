using System.Collections;
using System.Collections.Generic;
using CarDriving;
using UnityEngine;

public class GameController : SingletonBehaviour<GameController>
{
    [SerializeField] private LevelController _levelController;
    [SerializeField] private CarBehaviour _car;
    
    public int PlayerLevel { get; private set; }
    void Start()
    {
        PlayerLevel = PlayerPrefs.GetInt("Level", 0);
        Load();
    }

    public void Load()
    {
        _levelController.Load(PlayerLevel);
    }

    public void OnLevelLoaded()
    {
        _car = _levelController.CurrentLevel.GetCar();
        if (!_car)
        {
            Debug.LogError("No car found in level");
            return;
        }
        
        _car.OnLevelLoaded();
    }

    public void TurnLeft(bool pressed)
    {
        if (!_car)
            return;
        
        _car.TurnLeft(pressed);
    }
    
    public void TurnRight(bool pressed)
    {
        if (!_car)
            return;
        
        _car.TurnRight(pressed);
    }
}
