using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHarvest : Crop // INHERITANCE
{
    public override void Harvest() // POLYMORPHISM
    {
        base.Harvest();
        currentField.CurrentCrop = null;
        Destroy(gameObject);
    }
}
