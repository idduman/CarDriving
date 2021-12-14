using System.Collections;
using System.Collections.Generic;
using CarDriving;
using UnityEngine;

public class UIController : SingletonBehaviour<UIController>
{
    public void TurnLeft(bool pressed)
    {
        GameController.Instance.TurnLeft(pressed);
    }

    public void TurnRight(bool pressed)
    {
        GameController.Instance.TurnRight(pressed);
    }
}
