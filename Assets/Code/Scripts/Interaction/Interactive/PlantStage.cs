using DonutDiner.ItemModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlantStage", menuName = "Items/Plant_Stage")]
public class PlantStage : ScriptableObject
{
    public Vector3 size;
    public ItemObject growObject;
    public ItemObject shrinkObject;
    public ItemObject attackObject;

    public PlantResponse Response (ItemObject _item)
    {
        if (_item == growObject) { return PlantResponse.grow; }
        if (_item == shrinkObject) { return PlantResponse.shrink; }
        if (_item == attackObject) { return PlantResponse.attack; }

        return PlantResponse.nothing;
    }

}
