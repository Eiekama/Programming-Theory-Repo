using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHarvest : Crop // INHERITANCE
{
    bool hasGrownOnce;
    [SerializeField] float newGrowTime;

    public override void Harvest() // POLYMORPHISM
    {
        base.Harvest();
        hasGrownOnce = true;
        spriteRenderer.sprite = growthStages[growthStages.Length - 2];
        StartCoroutine(Growing());
    }

    protected override IEnumerator Growing() // POLYMORPHISM
    {
        if (!hasGrownOnce)
        {
            return base.Growing();
        } else
        {
            return NewGrowing();
        }
    }

    IEnumerator NewGrowing()
    {
        isGrowing = true;
        IsReady = false;

        var growTimeLeft = newGrowTime;
        while (growTimeLeft > 0)
        {
            if (isGrowing)
            {
                growTimeLeft -= GrowthSpeed * Time.deltaTime;
                yield return null;
            }
            else
            {
                yield return null;
            }
        }

        spriteRenderer.sprite = growthStages[growthStages.Length - 1];
        isGrowing = false;
        IsReady = true;
    }
}
