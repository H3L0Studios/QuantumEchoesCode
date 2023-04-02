//PURPOSE: manages the activities in the option menu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    [SerializeField] GameObject optionsOverlay;
    [SerializeField] GameObject visualsPanel;
    [SerializeField] GameObject soundPanel;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject difficultyPanel;
    [SerializeField] GameObject savePanel;
    [SerializeField] GameObject fogStorm;
    [SerializeField] Dropdown aaList;
    public Slider brightnessSlider;
    public Toggle fogToggle;
    [SerializeField] PostProcessLayer fogPPL;
    private bool fogOnOff = true;
    public Slider ambiantLevel;
    public Slider sfxLevel;
    public AudioMixer ambientMixer;
    public AudioMixer sfxMixer;

    void Start()
    {
        optionsOverlay.SetActive(false);
        visualsPanel.SetActive(false);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        savePanel.SetActive(false);
        fogPPL.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
        aaList.value = 1;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsOverlay.activeSelf == false)
            {
                optionsOverlay.SetActive(true);
                visualsPanel.SetActive(true);
                soundPanel.SetActive(false);
                controlsPanel.SetActive(false);
                difficultyPanel.SetActive(false);
                savePanel.SetActive(false);
                Time.timeScale = 0f;
                Cursor.visible = true;
                SaveScript.inOptions = true;
            }
            else
            {
                optionsOverlay.SetActive(false);
                Time.timeScale = 1f;
                Cursor.visible = false;
                SaveScript.inOptions = false;
            }
        }
    }

    public void antiAliasSwitch()
    {
        if (aaList.value == 0) //none
        {
            fogPPL.antialiasingMode = PostProcessLayer.Antialiasing.None;
        }
        else if (aaList.value == 1) //TAA
        {
            fogPPL.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
        }
        else if (aaList.value == 2) //FXAA
        {
            fogPPL.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
        }
        else if (aaList.value == 3) //SMAA
        {
            fogPPL.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        }
    }

    public void AmbiantVolume()
    {
        ambientMixer.SetFloat("Volume", ambiantLevel.value);
    }

    public void SFXVolume()
    {
        sfxMixer.SetFloat("Volume", sfxLevel.value);
    }


    public void lightValue()
    {
        RenderSettings.ambientIntensity = brightnessSlider.value;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);   
    }

    public void fogSwitch()
    {
        if (fogToggle.isOn == true)
        {
            if (fogOnOff == true)
            {
                fogPPL.fog.enabled = false;
                fogOnOff = false;
                fogStorm.SetActive(false);
            }
            else if (fogOnOff == false)
            {
                fogPPL.fog.enabled = true;
                fogOnOff = true;
                fogStorm.SetActive(true);
            }
        }
        if (fogToggle.isOn==false)
        {
            if (fogOnOff==true)
            {
                fogPPL.fog.enabled = false;
                fogOnOff = false;
                fogStorm.SetActive(false);
            }
            else if (fogOnOff == false)
            {
                fogPPL.fog.enabled = true;
                fogOnOff = true;
                fogStorm.SetActive(true);
            }
        }
    }

    public void VisualsOn()
    {
        visualsPanel.SetActive(true);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        savePanel.SetActive(false);
    }
    public void SoundOn()
    {
        visualsPanel.SetActive(false);
        soundPanel.SetActive(true);
        controlsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        savePanel.SetActive(false);
    }
    public void ControlsOn()
    {
        visualsPanel.SetActive(false);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(true);
        difficultyPanel.SetActive(false);
        savePanel.SetActive(false);
    }
    public void DifficultyOn()
    {
        visualsPanel.SetActive(false);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        savePanel.SetActive(false);
    }
    public void SaveOn() 
    {
        visualsPanel.SetActive(false);
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        savePanel.SetActive(true);
    }
}
