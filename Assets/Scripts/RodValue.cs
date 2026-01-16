using TMPro;
using UnityEngine;
using static BeadValue;

public class RodValue : MonoBehaviour
{
	[SerializeField] private TextMeshPro rodValueText;
	[SerializeField] public int digitPosition = 1;

	[SerializeField] private BeadValue upperBeadValue;
	[SerializeField] private BeadValue[] lowerBeadValues;

	[SerializeField] private ToggleState toggleState;
	[SerializeField] private GameObject backgroundObject;

	private bool _toggleIsOn;

	public int value = 0;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (toggleState == null) return;
		_toggleIsOn = toggleState.isOn;
		SetRodValueText(_toggleIsOn);
	}

	// Update is called once per frame
	void Update()
	{
		if (toggleState == null) return;
		CheckToggleState();
	}

	public void CalcRodValue()
	{
		value = 0;

		BeadValue[] beadValues = GetComponentsInChildren<BeadValue>();

		foreach (BeadValue beadValue in beadValues)
		{
			value += beadValue.value;
		}

		transform.parent.GetComponent<TotalValue>().CalcTotalValue();

		rodValueText.text = value.ToString();
	}

	public void SetRodValue(int _value)
	{
		value = _value % 10;
		rodValueText.text = value.ToString();

		if (value >= 5)
		{
			upperBeadValue.SetBeadPos(BeadPos.Low);
		}
		else
		{
			upperBeadValue.SetBeadPos(BeadPos.High);
		}
		for (int i = 0; i < lowerBeadValues.Length; i++)
		{
			if (i < (value % 5))
			{
				lowerBeadValues[i].SetBeadPos(BeadPos.High);
			}
			else
			{
				lowerBeadValues[i].SetBeadPos(BeadPos.Low);
			}
		}
	}

	private void CheckToggleState()
	{
		if (_toggleIsOn != toggleState.isOn)
		{
			_toggleIsOn = toggleState.isOn;
			SetRodValueText(_toggleIsOn);
		}
	}

	private void SetRodValueText(bool isOn)
	{
		if (isOn)
		{
			RodValueVisible();
		}
		else
		{
			RodValueInvisible();
		}
	}

	private void RodValueVisible()
	{
		backgroundObject.SetActive(true);
		rodValueText.gameObject.SetActive(true);
	}

	private void RodValueInvisible()
	{
		backgroundObject.SetActive(false);
		rodValueText.gameObject.SetActive(false);
	}
}
