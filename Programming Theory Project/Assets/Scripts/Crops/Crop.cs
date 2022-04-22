using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public enum Hydration
    {
        Hydrated,
        NeedWater,
        Dehydrated
    }
    public Hydration hydration;

    [SerializeField] float growTime;
    float growthSpeed = 1.0f;
    bool isGrowing;
}
