using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    
    private bool _isRun = false;
    private bool _isGrounded = true;
    private bool _isCrouch = false;
    private bool _isWalk = false;


    [SerializeField] private float walkSpeed = 0f;
    [SerializeField] private float runSpeed = 0f;
    [SerializeField] private float applySpeed = 0f;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private float swimSpeed = 0f;
    [SerializeField] private float swimfastSpeed = 0f;
    [SerializeField] private float upSwimSpeed = 0f;

    private Vector3 _lastPos; // 움직임 체크 변수

    [SerializeField] private float crouchSpeed = 0f;
    //앉았을때 얼만큼 앉을지 결정
    [SerializeField] private float crouchPosY = 0f;
    private float _originPosY = 0f;
    private float _applyCrouchPosY = 0f;

    private float lookSensitivity = 2f;
    private float _cameraRotationLimit = 45;
    private float _currentCameraRotationX = 1f;

    private Camera _camera;
    private Rigidbody _playerRig;
    private CapsuleCollider _playerCol;
    private GunController _gunController;
    private Crosshair _crosshair;
    public StatusController _statusController;
    
    void Start()
    {
        _camera = Camera.main;
        _playerRig = GetComponent<Rigidbody>();
        _playerCol = GetComponent<CapsuleCollider>();
        _gunController = FindObjectOfType<GunController>();
        _crosshair = FindObjectOfType<Crosshair>();
        _statusController = FindObjectOfType<StatusController>();
        applySpeed = walkSpeed;
        _originPosY = _camera.transform.localPosition.y;
        _applyCrouchPosY = _originPosY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.canPlayerMove)
        {
            WaterCheck();
            if (!GameManager.isWater)
            {
                TryRun();
            }
            Move();
            MoveCheck();
            CameraRotation();
            CharacterRotation();
        }
        
    }

    private void Update()
    {
        if (GameManager.canPlayerMove)
        {
            IsGrounded();
            TryJump();
            TryCrouch();
        }
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        _isCrouch = !_isCrouch;
        _crosshair.CrouchingAnimation(_isCrouch);
        if (_isCrouch)
        {
            applySpeed = crouchSpeed;
            _applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            _applyCrouchPosY = _originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = _camera.transform.localPosition.y;
        int count = 0;

        while (_posY != _applyCrouchPosY)
        {
            _posY = Mathf.Lerp(_posY, _applyCrouchPosY, 0.1f);
            _camera.transform.localPosition = new Vector3(0, _posY, 0);
            count++;
            if(count > 15) break;
            yield return null;
        }
        _camera.transform.localPosition = new Vector3(0, _posY, 0);
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && _statusController.GetCurSP() > 0 && !GameManager.isWater) Jump();
        else if (Input.GetKey(KeyCode.Space) && GameManager.isWater) UpSwim();
    }

    private void Jump()
    {
        if(_isCrouch) Crouch(); //앉은 상태에서 점프시 앉은 상태 해제
        _statusController.DecStamina(100);
        _playerRig.velocity = transform.up * jumpForce;
    }

    private void IsGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerCol.bounds.extents.y + 0.1f);
        _crosshair.JumpAnimation(!_isGrounded);
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _statusController.GetCurSP() > 0)
        {
            Running();
            _isRun = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || _statusController.GetCurSP() <= 0)
        {
            RunningCancle();
        }
    }

    private void RunningCancle()
    {
        _isRun = false;
        _crosshair.RunningAnimation(_isRun);
        applySpeed = walkSpeed;
    }

    private void Running()
    {
        if(_isCrouch) Crouch();

        _gunController.CancleFineSight();
        _isRun = true;
        _crosshair.RunningAnimation(_isRun);
        _statusController.DecStamina(10);
        applySpeed = runSpeed;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxis("Horizontal");
        float _moveDirY = Input.GetAxis("Vertical");
        

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirY;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        
        _playerRig.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    
    private void MoveCheck()
    {
        if (!_isRun && !_isCrouch && _isGrounded)
        {
            if (Vector3.Distance(_lastPos, transform.position) >= 0.01f) _isWalk = true;
            else _isWalk = false;
            _crosshair.WalkingAnimation(_isWalk);
            _lastPos = transform.position;
        }

    }
    
    private void WaterCheck()
    {
        if (GameManager.isWater)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) applySpeed = swimfastSpeed;
            else applySpeed = swimSpeed;
        }
    }
    
    private void UpSwim()
    {
        _playerRig.velocity = transform.up * upSwimSpeed;
    }

    
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        _currentCameraRotationX -= _cameraRotationX;
        _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraRotationLimit, _cameraRotationLimit);

        _camera.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0, 0);
        
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _chracterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        _playerRig.MoveRotation(_playerRig.rotation * Quaternion.Euler(_chracterRotationY));
    }
}
