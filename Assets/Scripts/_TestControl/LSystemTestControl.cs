using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSystemTestControl : MonoBehaviour
{
    [SerializeField] LSystem _lSystem;
    [SerializeField] Text _text;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string str = _lSystem.Generate();
            _text.text = str;
        }
    }
}
