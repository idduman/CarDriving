using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace CarDriving
{
    public class LevelBehaviour : MonoBehaviour
    {
        [SerializeField] private List<CarData> _carData = new List<CarData>();

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
}