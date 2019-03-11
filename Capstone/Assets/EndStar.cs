using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStar : MonoBehaviour
{
    // for ID when there are multiple EndStars to warp to
    int number;

    void SetNumber(int num)
    {
        number = num;
    }

    // allows EndStar to be found in other classes
    public static explicit operator EndStar(GameObject v)
    {
        throw new NotImplementedException();
    }

    int getNumber()
    {
        return number;
    }

}
