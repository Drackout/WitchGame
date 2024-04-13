using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UICardDissolve_Runtime : MonoBehaviour
{
    [SerializeField] public Material material;
    public Slider SliderDissolver;


    private void Update()
    {
        //Debug.Log(SliderDissolver.value);
        material.SetFloat("_Level", SliderDissolver.value);
    }
}
