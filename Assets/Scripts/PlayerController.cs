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
    
    
    [SerializeField] private float walkSpeed = 0f;
    [SerializeField] private float runSpeed = 0f;
    [SerializeField] private float applySpeed = 0f;
    [SerializeField] private float jumpForce = 0f;

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
    
    void Start()
    {
        _camera = Camera.main;
        _playerRig = GetComponent<Rigidbody>();
        _playerCol = GetComponent<CapsuleCollider>();
        _gunController = FindObjectOfType<GunController>();
        applySpeed = walkSpeed;
        _originPosY = _camera.transform.localPosition.y;
        _applyCrouchPosY = _originPosY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void Update()
    {
        IsGrounded();
        TryJump();
        TryCrouch();
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
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
 
        _playerRig.velocity = transform.up * jumpForce;
    }

    private void IsGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerCol.bounds.extents.y + 0.1f);
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
            _isRun = true;
        }
        else
        {
            RunningCancle();
        }
    }

    private void RunningCancle()
    {
        _isRun = false;
        applySpeed = walkSpeed;
    }

    private void Running()
    {
        if(_isCrouch) Crouch();

        _gunController.CancleFineSight();
        _isRun = true;
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
