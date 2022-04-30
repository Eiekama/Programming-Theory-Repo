using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHarvest : Crop
{
    public override void Harvest()
    {
        base.Harvest();
        spriteRenderer.sprite = growthStages[growthStages.Length - 2];
        StartCoroutine(Growing());
    }
}
