using UnityEngine;
using System.Collections;

public class Ammunition
{
    public int Amount;
    public readonly int MAX_CAPACITY = 500; 

    public Ammunition(int amount)
    {
        if (amount >= 0 && amount <= MAX_CAPACITY)
        {
            this.Amount = amount;
        }
        else
        {
            Debug.Log("Ammunition Constructor(): Amount is out of bound: Must be [0-" + MAX_CAPACITY +"]");
            this.Amount = MAX_CAPACITY; // set it to default
        }
    }

    
    public bool Add(int amount)
    {
        if (this.Amount == MAX_CAPACITY) return false;
        else
        {
            Amount = Amount + amount >= MAX_CAPACITY ? MAX_CAPACITY : (Amount + amount);
            return true;
        }
       
    }

   
}
