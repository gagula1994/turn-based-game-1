using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityScript : MonoBehaviour
{
    [Header("City Info:")]
    public string cityName;
    public int inConstruction = -1;
    public Text cityNameText;
    public Text constructingText;
    public Text populationNumberText;

    [Header ("Building Speed:")]
    public float buildingSpeed = 1;
    public float buildingSpeedBonus = 1;
    public float buildingModifier = 1;

    [Header ("Food:")]
    public float food;
    public float foodPerTurn = 1;
    public float foodLimit;
    public float foodBonus = 1;
    public float foodModifier = 1;

    [Header ("Gold:")]
    public float gold;
    public float goldBonus = 1;
    public float goldModifier = 1;

    [Header ("Happiness")]
    public float happinessBase = 50;
    public float happinessCurrent = 50;

    [Header ("Population:")]
    public float population = 1;
    public float populationLimit;

    [Header("Research:")]
    public float research;
    public float researchBonus = 1;
    public float researchModifier = 1;

    [Header ("")]
    public List<CityBuilding> buildings;
    public BuildingUIScript bus;
    public PlayerStats player;
    public DeletePanelScript panel;
    public CityBuilding tempBuilding;
    public Button tempButton;
    public int id;

    void Awake()
    {
        StatsModify();
    }

    void Start()
    {
        EndTurnScript.endTurn.cities.Add(this);
        cityNameText.GetComponent<Text>().text = cityName;
        if (inConstruction != -1) ConstructionUpdate(buildings[inConstruction]);
        else NoMoreConstuction();
        PopulationGUI();
    }

    public void BuildButton(CityBuilding building, Button button)
    {
        switch (building.status)
        {
            case 0:
                StartBuiding(building, button);
                break;
            case 1:
            case 2:
                tempBuilding = building;
                tempButton = button;
                panel.OpenDeletePanel();
                break;
            default:
                break;
        }
    }

    public void StartBuiding(CityBuilding building, Button button)
    {
        if (player.CheckTotal(building.cost, player.total))
        {
            player.ModifyTotal("gold", building.cost);
            building.status = 1;
            building.construction = building.buildTime;
            buildings[building.id] = building;
            button.GetComponentInChildren<Text>().text = "Destroy";
            inConstruction = building.id;
            ConstructionUpdate(building);

            for (int i = 0; i < bus.buildingsUI.Count; i++)
            {
                if (bus.buildingsUI[i].transform.Find("BuildingButton").GetComponentInChildren<Text>().text != "Destroy")
                    bus.buildingsUI[i].transform.Find("BuildingButton").GetComponent<Button>().interactable = false;
            }
        }
    }

    public void DestroyBuilding(CityBuilding building, Button button)
    {
        panel.CloseDeletePanel();
        float refund = -building.cost;
        if (building.status == 2) refund /= 2;
        player.ModifyTotal("gold", refund);
        building.status = 0;
        buildings[building.id] = building;
        button.GetComponentInChildren<Text>().text = "Build";
        NoMoreConstuction();
    }

    public void CheckTurn(CityBuilding building)
    {
        if (building.status == 1)
        {
            building.construction -= buildingSpeed;
            ConstructionUpdate(building);

            if (building.construction <= 0)
            {
                switch (building.type)
                {
                    case "buildingSpeed":
                        buildingSpeedBonus *= building.bonus;
                        break;
                    case "food":
                        foodBonus *= building.bonus;
                        break;
                    case "gold":
                        goldBonus *= building.bonus;
                        break;
                    case "research":
                        researchBonus *= building.bonus;
                        break;
                    case "population":
                        populationLimit *= building.bonus;
                        PopulationGUI();
                        break;
                    case "happiness":
                        happinessBase = (float) Math.Round(happinessBase * building.bonus);
                        break;
                    case "special":
                        EndTurnScript.endTurn.Special(building.id);
                        break;
                    default:
                        break;
                }

                StatsModify();
                building.status = 2;
                NoMoreConstuction();
            }

            buildings[building.id] = building;
        } 
    }

    public void NoMoreConstuction()
    {
        inConstruction = -1;
        constructingText.GetComponent<Text>().text = "";
        CanBuild();
    }

    public void CanBuild()
    {
        int tempReq;

        for (int i = 0; i < buildings.Count; i++)
        {
            if (buildings[i].requirement != "")
            {
                tempReq = Int32.Parse(buildings[i].requirement);
                if (buildings[tempReq].status == 2) bus.buildingsUI[i].transform.Find("BuildingButton").GetComponent<Button>().interactable = true;
            }
            else bus.buildingsUI[i].transform.Find("BuildingButton").GetComponent<Button>().interactable = true;
        }
    }

    public void ConstructionUpdate(CityBuilding building)
    {
        constructingText.GetComponent<Text>().text = building.buildingName + ": " + Math.Ceiling(building.construction / buildingSpeed) + " turns";
    }

    public float StatModify(float modifier, float bonus)
    {
        return modifier * population * happinessCurrent * bonus / 50;
    }

    public float RoundedStat(float modifier, float bonus)
    {
        return (float) Math.Round(StatModify(modifier, bonus));
    }

    public void StatsModify()
    {
        buildingSpeed = StatModify(buildingModifier, buildingSpeedBonus);
        if (inConstruction != -1) ConstructionUpdate(buildings[inConstruction]);

        foodPerTurn = RoundedStat(foodModifier, foodBonus);
        gold = RoundedStat(goldModifier, goldBonus);
        research = RoundedStat(researchModifier, researchBonus);
        player.PerTurnValue();
    }

    public void PopulationModify(float value = 0)
    {
        if (value != 0)
        {
            population -= value;
        }
        else
        {
            if (population < populationLimit)
            {
                food += foodPerTurn;

                if (food >= foodLimit)
                {
                    population++;
                    food = 0;
                    foodLimit += 5;
                }
            }
            else if (population > populationLimit)
            {
                population--;
                food = 0;
                foodLimit -= 5;
            }
        }

        PopulationGUI();
        player.ModifyTotal("population");
        StatsModify();
    }

    public void PopulationGUI()
    {
        populationNumberText.GetComponent<Text>().text = population.ToString();
        if (population == populationLimit) populationNumberText.GetComponent<Text>().color = new Color(1, 1, 1, 1);
        else if (population > populationLimit) populationNumberText.GetComponent<Text>().color = new Color(1, 0, 0, 1);
        else populationNumberText.GetComponent<Text>().color = new Color(0.08966714f, 0.6132076f, 0.1328834f, 1);
    }

    public void HappinessModify()
    {
        if (happinessCurrent < happinessBase) happinessCurrent++;
        else if (happinessCurrent > happinessBase) happinessCurrent--;
        StatsModify();
    }    
}

[System.Serializable]
public struct CityBuilding
{
    public string buildingName;
    public string description;
    public string type;
    public float cost;
    public float buildTime;
    public float bonus;
    public float status;
    public float construction;
    public string requirement;
    public int id;
}