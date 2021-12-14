using System.Collections;
using System.Collections.Generic;
using CarDriving;
using UnityEngine;

public class GameController : SingletonBehaviour<GameController>
{
    [SerializeField] private CarController _playerCar;
    [SerializeField] private LevelController _levelController;
    
    public LevelBehaviour CurrentLevel { get; private set; }
    
    public int CurrentCar { get; private set; }

    public int PlayerLevel { get; private set; }
    void Start()
    {
        PlayerLevel = PlayerPrefs.GetInt("Level", 0);
        CurrentCar = 0;
        Load();
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
        _playerCar.ResetCar(CurrentCar);
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
        }
        
        _playerCar.ResetCar(CurrentCar);
    }

    public void OnCheckpointReached()
    {
        
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
        Load();
    }
}
