using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;

    void Start()
    {
        SetValue(maxValue);
    }

    public void SetValue(float value)
    {
        curValue = Mathf.Clamp(curValue + value, 0f, maxValue);

        // UI 업데이트
        uiBar.fillAmount = curValue / maxValue;
    }
}
