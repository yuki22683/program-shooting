using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class KeyPadValue : MonoBehaviour
{
    private string _valueString = string.Empty;

    private string valueString
    {
        get { return _valueString; }
        set {
            if (_valueString.Length < digit)
            {
                if (_valueString == "0")
                {
                    _valueString = "";
                }

                _valueString += value;
				textMeshPro.text = _valueString;
			}
        }
    }

    [SerializeField] private int digit;

    [SerializeField] private TextMeshPro textMeshPro;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearValue();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetValue()
    {
        return int.Parse(valueString);
    }

    public string GetValueString()
    {
        return valueString;
    }

    public void ClearValue()
    {
        _valueString = "0";
		textMeshPro.text = _valueString;
	}

    public void SetValue(int value)
    {
        valueString = value.ToString();
	}

    public void BackSpace()
    {
        if (valueString.Length <= 1)
        {
			_valueString = "0";
			textMeshPro.text = _valueString;
		}
		else
        {
			_valueString = _valueString.Remove(_valueString.Length - 1);
			textMeshPro.text = _valueString;
		}
    }
}
