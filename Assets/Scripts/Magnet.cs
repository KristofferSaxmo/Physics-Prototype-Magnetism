using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Magnet : MonoBehaviour
{
    public List<GameObject> poles = new();
    
    [HideInInspector] public Rigidbody rb;
    
    [SerializeField] private float _maxForce;
    [SerializeField] private float _maxRange;
    
    public float MaxForce => _maxForce;
    public float MaxRange => _maxRange;

    private void Start()
    {
        MagnetManager.Instance.AddMagnet(this);
        rb = GetComponent<Rigidbody>();
    }
    
    public void Initialize(float maxForce, float maxRange)
    {
        _maxForce = maxForce;
        _maxRange = maxRange;
    }

    private void OnDestroy()
    {
        MagnetManager.Instance.RemoveMagnet(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _maxRange);
    }
}

