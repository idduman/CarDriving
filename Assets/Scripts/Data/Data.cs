using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarDriving
{
    [Serializable]
    public class CarData
    {
        public Transform StartPoint;
        public Transform EndPoint;
    }

    public enum TurnDirection
    {
        Straight,
        Left,
        Right,
        Stop,
    }
}