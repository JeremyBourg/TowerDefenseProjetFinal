using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectTour : MonoBehaviour
{
    public int selectedTower;

    public void SelectFirstTower()
    {
        selectedTower = 1;
    }

    public void SelectSecondTower()
    {
        selectedTower = 2;
    }
}
