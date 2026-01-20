using UnityEngine;

public class RodColorManager : MonoBehaviour
{
    [SerializeField] private int id;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = DataManager.gameSettings.colorSettings.rodColors[id];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
