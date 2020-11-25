using System;
using UnityEngine;

public class DestroyBuildingConfirm : MonoBehaviour
{
    public CityScript city;

    public void DestroyBuildingConfirmation()
    {
        city.DestroyBuilding(city.tempBuilding, city.tempButton);

        if (city.tempBuilding.status == 2)
        {
            switch (city.tempBuilding.type)
            {
                case "buildingSpeed":
                    city.buildingSpeedBonus /= city.tempBuilding.bonus;
                    break;
                case "food":
                    city.foodBonus /= city.tempBuilding.bonus;
                    break;
                case "gold":
                    city.goldBonus /= city.tempBuilding.bonus;
                    break;
                case "research":
                    city.researchBonus /= city.tempBuilding.bonus;
                    break;
                case "population":
                    city.populationLimit /= city.tempBuilding.bonus;
                    city.PopulationGUI();
                    break;
                case "happiness":
                    city.happinessBase = (float)Math.Round(city.happinessBase / city.tempBuilding.bonus); ;
                    break;
                default:
                    break;
            }
            city.StatsModify();
        }
    }
}
