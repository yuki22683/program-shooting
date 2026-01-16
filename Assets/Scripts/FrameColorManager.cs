using UnityEngine;

public class FrameColorManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.material.color = DataManager.gameSettings.colorSettings.frameColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
