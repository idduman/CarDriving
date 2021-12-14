using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro.EditorUtilities;
using UnityEngine;

[Serializable]
public class CarData
{
    public Transform StartPoint;
    public Transform EndPoint;
}

public class LevelBehaviour : MonoBehaviour
{
    [SerializeField] private List<CarData> _carData;
    
    private List<CinemachinePath> _completedPaths;

    public CarData GetCarData(int index)
    {
        if (index >= _carData.Count)
        {
            Debug.LogError($"Car data has no data at index {index}");
            return null;
        }

        return _carData[index];
    }

    public int GetCarCount()
    {
        return _carData.Count;
    }
}