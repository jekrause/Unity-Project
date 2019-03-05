using UnityEngine;
public class MedKit : FirstAid
{
    private const float MED_KIT_POINTS = 100f;

    public MedKit() {
        HP_Points = MED_KIT_POINTS;
        type = Type.HEALING_ITEM;
    }

}
