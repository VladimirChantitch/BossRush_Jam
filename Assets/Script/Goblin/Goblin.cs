using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.UI;

public class Goblin : HubInteractor
{
    List<Recipies> recipies = new List<Recipies>();
    internal void Init(List<Recipies> recipies)
    {
        this.recipies = recipies;
    }
    
    //TODO -- Start a dialogue

}
