using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;

    private ToggleBool toggleBool;
    private bool _toggleBool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggleBool = GetComponent<ToggleBool>();
        _toggleBool = toggleBool.flg;
    }

    // Update is called once per frame
    void Update()
    {
        if (_toggleBool != toggleBool.flg)
        {
            _toggleBool = toggleBool.flg;
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(_toggleBool);
            }
        }
    }
}
