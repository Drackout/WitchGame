using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UICardDissolve_Runtime : MonoBehaviour
{
    [SerializeField] public Material material;
    public Slider SliderDissolver;


    private void Start()
    {
        material = GetComponent<Renderer>().sharedMaterial;
    }

    private void Update()
    {
        material.SetFloat("_Level", SliderDissolver.value);
    }
}
