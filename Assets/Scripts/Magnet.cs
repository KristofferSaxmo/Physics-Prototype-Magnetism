using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    
    public List<GameObject> poles = new();
    
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider collider;
    
    [SerializeField] protected float _maxForce;
    [SerializeField] protected float _maxRange;
    [SerializeField] protected bool _isStatic;
    
    public float MaxForce => _maxForce;
    public float MaxRange => _maxRange;
    public bool IsStatic => _isStatic;

    private void Start()
    {
        MagnetManager.Instance.AddMagnet(this);
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
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

