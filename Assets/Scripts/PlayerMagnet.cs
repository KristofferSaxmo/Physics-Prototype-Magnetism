using UnityEngine;

public class PlayerMagnet : Magnet
{
    [SerializeField] private Material positiveMaterial;
    [SerializeField] private Material neutralMaterial;
    [SerializeField] private Material negativeMaterial;
    
    private Renderer _renderer;
    
    private void Awake()
    {
        _renderer = poles[0].GetComponent<Renderer>();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _maxForce = 10000;
            poles[0].name = "Positive";
            _renderer.material = positiveMaterial;
        }
        else if (Input.GetMouseButton(1))
        {
            _maxForce = 10000;
            poles[0].name = "Negative";
            _renderer.material = negativeMaterial;
        }
        else
        {
            _maxForce = 0;
            _renderer.material = neutralMaterial;
        }
    }
}
