using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    public static MagnetManager Instance { get; private set; }
    
    [HideInInspector] public List<Magnet> magnets = new();
    
    [SerializeField] private float constant = 4 * Mathf.PI * 1e-7f; // Magnetic constant (μ0)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void AddMagnet(Magnet magnet)
    {
        magnets.Add(magnet);
    }
    
    public void RemoveMagnet(Magnet magnet)
    {
        magnets.Remove(magnet);
    }
    
    private void FixedUpdate()
    {
        for (int i = 0; i < magnets.Count - 1; i++)
        {
            for (int j = i + 1; j < magnets.Count; j++)
            {
                float distance = Vector3.Distance(magnets[i].transform.position, magnets[j].transform.position);
                if (distance > magnets[i].MaxRange + magnets[j].MaxRange) continue;
                ApplyForce(magnets[i], magnets[j]);
            }
        }
    }
    
    void ApplyForce(Magnet magnet1, Magnet magnet2)
    {
        Vector3 m1p1 = magnet1.poles[0].transform.position;
        Vector3 m1p2 = magnet1.poles[1].transform.position;
        Vector3 m2p1 = magnet2.poles[0].transform.position;
        Vector3 m2p2 = magnet2.poles[1].transform.position;
        
        
        Vector3 totalForce = Vector3.zero;
        
        totalForce += GetForceVector(magnet1, magnet2, m1p1, m2p1, false);
        totalForce += GetForceVector(magnet1, magnet2, m1p2, m2p2, false);
        totalForce += GetForceVector(magnet1, magnet2, m1p1, m2p2, true);
        totalForce += GetForceVector(magnet1, magnet2, m1p2, m2p1, true);
        
        magnet1.rb.AddForce(totalForce);
        magnet2.rb.AddForce(-totalForce);
    }

    private Vector3 GetForceVector(Magnet magnet1, Magnet magnet2, Vector3 pole1, Vector3 pole2, bool isOppositePoles)
    {
        float distance = Vector3.Distance(pole1, pole2);
        if (distance == 0f) return Vector3.zero;

        float sqrDistance = distance * distance;
        
        // F = μ0 / 4π * (m1 * m2) / r^2 (Inverse square law of force between two magnetic poles)
        float forceMagnitude = (constant / 4 * Mathf.PI) * (magnet1.MaxForce * magnet2.MaxForce) / sqrDistance;
        
        if (!isOppositePoles)
            forceMagnitude = -forceMagnitude;

        Vector3 forceDirection = (pole2 - pole1).normalized;
    
        return forceDirection * forceMagnitude;
    }

}