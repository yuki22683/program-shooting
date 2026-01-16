using System;
using UnityEngine;

public class TotalValue : MonoBehaviour
{
    public int value = 0;

    [SerializeField] private RodValue[] rodValues;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CalcTotalValue()
    {
        value = 0;

        RodValue[] rodValues = GetComponentsInChildren<RodValue>();

        foreach (RodValue rodValue in rodValues)
        {
            value += (int)Math.Pow(10, rodValue.digitPosition) * rodValue.value;
        }
    }

    public void SetTotalValue(int _value)
    {
        value = _value;

        for (int i = 0; i < rodValues.Length; i++)
        {
            int rodValue = ((value / (int)Math.Pow(10, rodValues[i].digitPosition)) % 10);
            rodValues[i].SetRodValue(rodValue);
		}
    }
}
