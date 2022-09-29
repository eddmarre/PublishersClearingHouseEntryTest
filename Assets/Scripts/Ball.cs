using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody _ballRigidbody;
    
    public void KickBall(Vector3 direction)
    {
        float forceMultiplier = 5f;
        _ballRigidbody.AddForce(direction * forceMultiplier, ForceMode.Impulse);
    }
}