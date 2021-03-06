﻿
using UnityEngine;
using UnityEngine.UI;

public class TextBehaviour : MonoBehaviour {

    public FloatVariable floatvar;
    [SerializeField]
    private Text _text;
  
    void Start()
    {
        floatvar.Value = 0;
        SetValue();
        if(_text == null)
            _text = GetComponent<Text>();
    }
    public void SetValue()
    {
        var rounded = Mathf.RoundToInt(floatvar.Value).ToString();
        _text.text = floatvar.name + " " + rounded;
    }
}
