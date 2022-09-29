using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private SphereCollider _playerDetectionSphereCollider;
    [SerializeField] private LayerMask _ballLayerMask;

    private PlayerInputActions _playerInputActions;
    private bool _isCursorLocked = true;

    private Collider[] _ballColliders;
    private int _ballsInPlayerDetectionRadius;

    private const string Getweburl = "https://publishersclearinghouseentrytest.azurewebsites.net/api/testing";
    private const string Postweburl = "https://publishersclearinghouseentrytest.azurewebsites.net/api/testing";

    public static event Action OnPlayerKick;
    public static event Action<string> OnPlayerGetOrSetWebRequest;

    private void Awake()
    {
        InitPlayerInputActions();

        //only ever one ball in scene
        _ballColliders = new Collider[1];
    }

    private void Update()
    {
        _ballsInPlayerDetectionRadius =
            CheckForBallWithinDetectionArea(_playerDetectionSphereCollider, _ballColliders, _ballLayerMask);
    }

    private int CheckForBallWithinDetectionArea(SphereCollider collider, Collider[] ballColliers,
        LayerMask ballLayerMask)
    {
        return Physics.OverlapSphereNonAlloc(collider.transform.position,
            collider.radius,
            ballColliers, ballLayerMask);
    }

    private void InitPlayerInputActions()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += Kick;
        _playerInputActions.Player.WebRequestGet.performed += WebRequestGet;
        _playerInputActions.Player.WebRequestPost.performed += WebRequestPost;
        _playerInputActions.Player.Escape.performed += (context) =>
        {
            _isCursorLocked = !_isCursorLocked;
            Cursor.lockState = _isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        };
    }

    private void Kick(InputAction.CallbackContext callbackContext)
    {
        if (_ballsInPlayerDetectionRadius > 0)
        {
            //only one ball exists so this is fine
            if (!_ballColliders[0].TryGetComponent(out Ball ball)) return;

            var calculateKickDirection = CalculateKickDirection(ball.transform);
            ball.KickBall(calculateKickDirection);
            OnPlayerKick?.Invoke();
        }
    }

    private Vector3 CalculateKickDirection(Transform ballTransform)
    {
        Vector3 forwardDirection = (transform.position - ballTransform.position).normalized;
        Vector3 upwardDirection = Vector3.up;
        return forwardDirection + upwardDirection;
    }

    private void WebRequestGet(InputAction.CallbackContext callbackContext)
    {
        APIWebRequests.Get(Getweburl,
            OnWebRequestError,
            OnWebRequestSuccess);
    }

    private void WebRequestPost(InputAction.CallbackContext callbackContext)
    {
        var test = new Testing("this is a post test", ScoreTracker.Score);
        APIWebRequests.PostJson(Postweburl,
            JsonUtility.ToJson(test),
            OnWebRequestError,
            OnWebRequestSuccess);
    }

    private void OnWebRequestError(string error)
    {
        Debug.Log($"Error: {error}");
    }

    private void OnWebRequestSuccess(string response)
    {
        Debug.Log($"Server Response: {response}");
        OnPlayerGetOrSetWebRequest?.Invoke(response);
    }
}