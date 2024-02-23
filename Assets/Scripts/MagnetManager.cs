using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MagnetManager : MonoBehaviour
{
    public static MagnetManager Instance { get; private set; }
    
    [HideInInspector] public List<Magnet> magnets = new();
    [HideInInspector] public List<Magnet> staticMagnets = new();
    
    private const float MagneticConstant = 4 * Mathf.PI * 1e-7f; // Magnetic constant (μ0)

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
        if (magnet.IsStatic)
        {
            staticMagnets.Add(magnet);
            return;
        }
        magnets.Add(magnet);
    }
    
    public void RemoveMagnet(Magnet magnet)
    {
        if (magnet.IsStatic)
        {
            staticMagnets.Remove(magnet);
            return;
        }
        magnets.Remove(magnet);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < magnets.Count - 1; i++)
        {
            Vector3 magnet1Position = magnets[i].transform.position;
            for (int j = i + 1; j < magnets.Count; j++)
            {
                float distance = Vector3.Distance(magnet1Position, magnets[j].transform.position);
                if (distance > magnets[i].MaxRange + magnets[j].MaxRange) continue;
                MagnetInteraction(magnets[i], magnets[j]);
            }
        }

        foreach (var magnet in magnets)
        {
            Vector3 magnet1Position = magnet.transform.position;
            foreach (var staticMagnet in staticMagnets)
            {
                float distance = Vector3.Distance(magnet1Position,
                    staticMagnet.collider.ClosestPoint(magnet1Position));
                if (distance > magnet.MaxRange + staticMagnet.MaxRange) continue;
                StaticMagnetInteraction(magnet, staticMagnet);
            }
        }
    }

    private void MagnetInteraction(Magnet magnet1, Magnet magnet2)
    {
        Vector3 totalForce = Vector3.zero;
            
        
        foreach (var pole1 in magnet1.poles)
        {
            bool pole1IsPositive = pole1.name.Contains("Positive");
            
            foreach (var pole2 in magnet2.poles)
            {
                bool pole2IsPositive = pole2.name.Contains("Positive");
                bool isOppositePoles = pole1IsPositive != pole2IsPositive;
                
                totalForce += CalculateForceBetweenPoles(magnet1, magnet2, pole1.transform.position, pole2.transform.position, isOppositePoles);
            }
        }
        
        magnet1.rb.AddForce(totalForce);
        
        magnet2.rb.AddForce(-totalForce);
    }
    
    private void StaticMagnetInteraction(Magnet magnet, Magnet staticMagnet)
    {
        Vector3 totalForce = Vector3.zero;
        
        foreach (var pole1 in magnet.poles)
        {
            bool pole1IsPositive = pole1.name.Contains("Positive");
            
            foreach (var pole2 in staticMagnet.poles)
            {
                bool pole2IsPositive = pole2.name.Contains("Positive");
                bool isOppositePoles = pole1IsPositive != pole2IsPositive;

                Vector3 position = pole1.transform.position;
                Vector3 pole2ClosestPoint = staticMagnet.collider.ClosestPoint(position);
                
                totalForce += CalculateForceBetweenPoles(magnet, staticMagnet, position, pole2ClosestPoint, isOppositePoles);
            }
        }
        
        magnet.rb.AddForce(totalForce);
    }

    private Vector3 CalculateForceBetweenPoles(Magnet magnet1, Magnet magnet2, Vector3 pole1, Vector3 pole2, bool isOppositePoles)
    {
        Vector3 direction = pole2 - pole1;
        float sqrDistance = direction.sqrMagnitude;
        if (sqrDistance == 0f) return Vector3.zero;
        
        // F = μ0 / 4π * (m1 * m2) / r^2 (Inverse square law of force between two magnetic poles)
        float forceMagnitude = MagneticConstant / 4 * Mathf.PI * (magnet1.MaxForce * magnet2.MaxForce) / sqrDistance;
        
        if (!isOppositePoles)
            forceMagnitude = -forceMagnitude;

        Vector3 forceDirection = direction.normalized;
    
        return forceDirection * forceMagnitude;
    }

}