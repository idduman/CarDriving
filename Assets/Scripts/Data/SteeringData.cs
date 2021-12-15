using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CarDriving
{
    public class SteeringData
    {
        private List<Tuple<float, TurnDirection>> _data;

        public SteeringData()
        {
            _data = new List<Tuple<float, TurnDirection>>();
        }

        public SteeringData Add(Tuple<float, TurnDirection> steer)
        {
            _data.Add(steer);
            return this;
        }

        public Tuple<float, TurnDirection> GetNext(float timeStamp)
        {
            return _data.FirstOrDefault(x => x.Item1 > timeStamp);
        }
    }
}

