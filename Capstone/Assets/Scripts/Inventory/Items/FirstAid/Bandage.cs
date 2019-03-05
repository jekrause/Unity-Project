using UnityEngine;
public class Bandage : FirstAid
{
    private const float BANDAGE_POINTS = 50f;

    public Bandage() {
        HP_Points = BANDAGE_POINTS;
        type = Type.HEALING_ITEM;
    }
}
