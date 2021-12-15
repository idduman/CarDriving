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

        public Tuple<Vector3, Vector3> GetCheckpointPositions(float offset = 0f)
        {
            return new Tuple<Vector3, Vector3>
            (StartPoint.position + offset * StartPoint.forward,
                EndPoint.position + offset * EndPoint.forward);
        }
        
        public Tuple<Quaternion, Quaternion> GetCheckpointRotations()
        {
            return new Tuple<Quaternion, Quaternion>
            (StartPoint.rotation, EndPoint.rotation);
        }
    }

    public enum TurnDirection
    {
        Straight,
        Left,
        Right,
        Stop,
    }
}