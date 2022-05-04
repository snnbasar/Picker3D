using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;


    [SerializeField] private Material[] sceneRandomMaterials; //SetsOnInspector

    [SerializeField] private Renderer[] grounds; //SetsOnInspector
    [SerializeField] private Renderer[] earlyGameGrounds; //SetsOnInspector
    [SerializeField] private Renderer[] bridges; //I mean barriers. SetsOnInspector
    [SerializeField] private Renderer[] earlyGameBridges; //I mean barriers. SetsOnInspector
    [SerializeField] private Renderer ramp; //SetsOnInspector
    [SerializeField] private Renderer earlyGameRamp; //SetsOnInspector

    private List<Material> addedMaterials = new List<Material>();

    //public event Action changeColorsEvent;

    //[SerializeField] private Color[] skyColors;//SetsOnInspector



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateGroundMat();
        UpdateBarrierMat();
        UpdateRampMat();
    }
    public void ChangeMyColorToRandom(Renderer renderer)
    {
        renderer.material = GetRandomMat();
    }

    private Material GetRandomMat()
    {
        return sceneRandomMaterials[UnityEngine.Random.Range(0, sceneRandomMaterials.Length)];
    }


    public async Task<Material> GetUniqueMaterial()
    {
        Material mat = GetRandomMat();
        while (addedMaterials.Contains(mat))
        {
            mat = GetRandomMat();
            await Task.Yield();
        }
        return mat;
    }




    private async void UpdateGroundMat()
    {
        Material mat = await GetUniqueMaterial();
        addedMaterials.Add(mat);

        foreach (var ground in grounds)
        {
            ground.material = mat;
        }


        if (GameManager.instance.GetSettingsData().previousGround == null)
        {
            foreach (var ground in earlyGameGrounds)
            {
                ground.material = mat;
            }
        }
        else
        {
            foreach (var ground in earlyGameGrounds)
            {
                ground.material = GameManager.instance.GetSettingsData().previousGround;
            }
        }

        GameManager.instance.GetSettingsData().previousGround = mat;

    }

    private async void UpdateBarrierMat()
    {
        Material mat = await GetUniqueMaterial();
        addedMaterials.Add(mat);

        foreach (var bridge in bridges)
        {
            bridge.material = mat;
        }

        if (GameManager.instance.GetSettingsData().previousBridge == null)
        {
            foreach (var bridge in earlyGameBridges)
            {
                bridge.material = mat;
            }
        }
        else
        {
            foreach (var bridge in earlyGameBridges)
            {
                bridge.material = GameManager.instance.GetSettingsData().previousBridge;
            }
        }

        GameManager.instance.GetSettingsData().previousBridge = mat;
    }

    private async void UpdateRampMat()
    {
        Material mat = await GetUniqueMaterial();
        addedMaterials.Add(mat);

        ramp.material = mat;


        if (GameManager.instance.GetSettingsData().previousRamp == null)
        {
            earlyGameRamp.material = mat;
        }
        else
        {
            earlyGameRamp.material = GameManager.instance.GetSettingsData().previousRamp;
        }

        GameManager.instance.GetSettingsData().previousRamp = mat;

    }



    //private void ChangeSkyColor()
    //{
    //    Color color = skyColors[UnityEngine.Random.Range(0, skyColors.Length)];

    //    Camera.main.backgroundColor = color;
    //}
}
