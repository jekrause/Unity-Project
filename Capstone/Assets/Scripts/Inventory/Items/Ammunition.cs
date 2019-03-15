﻿using UnityEngine;
using System.Collections;

public class Ammunition
{
    public int Amount;
    public readonly int MAX_CAPACITY = 200;

    public Ammunition(int amount)
    {
        if (amount >= 0 && amount <= MAX_CAPACITY)
        {
            this.Amount = amount;
        }
        else
        {
            throw new System.ArgumentException("Ammunition amount is out of bound: Must be [0-200]");
        }
    }

    
    public void Add(int amount)
    {
        Amount = Amount + amount >= MAX_CAPACITY ? MAX_CAPACITY : Amount;
    }

   
}
