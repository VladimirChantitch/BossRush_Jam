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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
