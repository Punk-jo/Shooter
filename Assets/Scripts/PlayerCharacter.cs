using System;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 90;
    [SerializeField] private float _minHeadAngle = -90;
    [SerializeField] private float _jumpForce = 50f;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private float _jumpDelay = .2f;
    private float _inputH;
    private float _inputV;
    private float _rotateY;

    private float _currentRotateX;
    private float _jumpTime;

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

    
    private void FixedUpdate()
    {
        Move();
        RotateY();
    }

    public void SetInput(float h, float v, float rotateY)
    {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
    }
    private void Move()
    {
        //Vector3 direction = new Vector3(_inputH, 0, _inputV).normalized;
        //transform.position += direction * Time.deltaTime * _speed;

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * speed;
        velocity.y = _rigidbody.linearVelocity.y;
        this.velocity = velocity;
        _rigidbody.linearVelocity = velocity;
    }

    private void RotateY()
    {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    public void RotateX(float value)
    {
        _currentRotateX = Math.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(-_currentRotateX, 0, 0);
    }

    private bool _isFly = true;

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = _rigidbody.linearVelocity;

        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
        
    }
    private void OnCollisionStay(Collision collision)
    {
        var contactPoints = collision.contacts;
        for(int i=0; i<contactPoints.Length; i++)
        {
            if(contactPoints[i].normal.y > .45f) _isFly = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isFly = true;
    }
    public void Jump()
    {
        if(_checkFly.IsFly) return;

        if(Time.time - _jumpTime < _jumpDelay) return;

        _jumpTime = Time.time;
        _rigidbody.AddForce(0,_jumpForce,0, ForceMode.VelocityChange);
    }
}
