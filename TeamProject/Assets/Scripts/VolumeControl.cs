using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] string VolumeParam = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;

    void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        Start();
    }

    private void HandleSliderValueChanged(float value)
    {
        mixer.SetFloat(VolumeParam, Mathf.Log10(value) * multiplier);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(VolumeParam, slider.value);
    }

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(VolumeParam, slider.value);
        HandleSliderValueChanged(slider.value);
    }
}
