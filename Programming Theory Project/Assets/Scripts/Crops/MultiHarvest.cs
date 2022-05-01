using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHarvest : Crop
{
    bool hasGrownOnce;
    [SerializeField] float newGrowTime;

    public override void Harvest()
    {
        base.Harvest();
        hasGrownOnce = true;
        spriteRenderer.sprite = growthStages[growthStages.Length - 2];
        StartCoroutine(Growing());
    }

    protected override IEnumerator Growing()
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
                growTimeLeft -= growthSpeed * Time.deltaTime;
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
