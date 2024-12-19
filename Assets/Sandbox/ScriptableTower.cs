using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Tower_name",menuName="Add Tower Data/Tower")]
public class ScriptableTower : ScriptableObject
{
    public float nbPointsVies;
    public float nbPointsDommages;
    public float tempsRecharge;
    public float vitesseDeplacement;
    public float vitesseMissile;  
}
