using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] protected GameObject seedForm;
    [SerializeField] protected GameObject harvestForm;
    protected Field currentField;

    [SerializeField] float growTime;
    [SerializeField] float growthSpeed = 1.0f;

    [SerializeField] float waterLevel = 10;
    public float WaterLevel
    {
        get { return waterLevel; }
        set
        {
            if (value > 10)
            {
                waterLevel = 10;
            } else if (value < 0)
            {
                waterLevel = 0;
            } else
            {
                waterLevel = value;
            }
        }
    }
    [SerializeField] float waterLossRate = 1.0f;

    public bool IsReady { get; private set; }
    bool isGrowing;

    void Awake()
    {
        currentField = GetComponentInParent<Field>();
    }

    protected virtual void Start()
    {
        seedForm.SetActive(true);
        harvestForm.SetActive(false);
        StartCoroutine(ManageFertiliser());
        StartCoroutine(ManageWater());
        StartCoroutine(Growing());
    }

    protected IEnumerator Growing()
    {
        isGrowing = true;
        IsReady = false;

        var growTimeLeft = growTime;
        while (growTimeLeft > 0)
        {
            if (isGrowing)
            {
                growTimeLeft -= growthSpeed * Time.deltaTime;
                yield return null;
            } else
            {
                yield return null;
            }
        }

        seedForm.SetActive(false);
        harvestForm.SetActive(true);
        isGrowing = false;
        IsReady = true;
        currentField.icons[0].SetActive(true);
    }

    public virtual void Harvest()
    {
        Debug.Log("Crop was harvested");
        foreach (var icon in currentField.icons)
        {
            if (icon.activeInHierarchy)
            {
                icon.SetActive(false);
            }
        }
    }

    IEnumerator ManageFertiliser()
    {
        var originalGrowthSpeed = growthSpeed;
        while (true)
        {
            if (currentField.isFertilised && isGrowing)
            {
                if (currentField.fertiliserTimeLeft > 0)
                {
                    if (originalGrowthSpeed == growthSpeed)
                    {
                        growthSpeed *= 2;
                    }
                    currentField.fertiliserTimeLeft -= Time.deltaTime;
                    yield return null;
                } else
                {
                    growthSpeed = originalGrowthSpeed;
                    currentField.fertiliserTimeLeft = currentField.fertiliserTimer;
                    currentField.isFertilised = false;
                    yield return null;
                }
            } else
            {
                yield return null;
            }
        }
    }

    IEnumerator ManageWater()
    {
        var originalGrowthSpeed = growthSpeed;
        while (true)
        {
            if (waterLevel > 0)
            {
                if (!isGrowing)
                {
                    isGrowing = true;
                }
                if (waterLevel >= 4 && growthSpeed != originalGrowthSpeed)
                {
                    growthSpeed = originalGrowthSpeed;
                    currentField.icons[1].SetActive(false);
                    currentField.icons[2].SetActive(false);
                } else if (waterLevel < 4 && originalGrowthSpeed == growthSpeed)
                {
                    growthSpeed /= 2;
                    currentField.icons[1].SetActive(true);
                }
                WaterLevel -= waterLossRate * Time.deltaTime;
                yield return null;
            } else
            {
                if (isGrowing)
                {
                    isGrowing = false;
                    currentField.icons[1].SetActive(false);
                    currentField.icons[2].SetActive(true);
                }
                yield return null;
            }
        }
    }
}
