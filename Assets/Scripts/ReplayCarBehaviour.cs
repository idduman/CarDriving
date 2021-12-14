using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ReplayCarBehaviour : MonoBehaviour
{
    private CinemachinePath _path;
    private float _duration;

    private bool _driving;
    private float _currentPos, _speed;
    
    // Start is called before the first frame update
    void Start()
    {
        _driving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_driving)
            return;
        
        _currentPos += _speed * Time.deltaTime;
        
        transform.position = _path.EvaluatePosition(_currentPos);
        transform.rotation = Quaternion.Euler(_path.EvaluateTangent(_currentPos));
    }

    public void Initialize(CinemachinePath path, float duration)
    {
        _path = path;
        _duration = duration;
    }

    public void Drive()
    {
        _currentPos = _path.MinPos;
        _speed = (_path.MaxPos - _path.MinPos) / _duration;
        transform.position = _path.EvaluatePosition(_currentPos);
        transform.rotation = Quaternion.Euler(_path.EvaluateTangent(_currentPos));
        _driving = true;
    }
}
