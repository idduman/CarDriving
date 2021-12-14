using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    [SerializeField] private CarBehaviour _car;

    public CarBehaviour GetCar()
    {
        return _car;
    }
}