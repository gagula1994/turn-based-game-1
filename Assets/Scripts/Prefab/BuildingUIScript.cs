using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIScript : MonoBehaviour
{
    public GameObject panel;
    public Text description;
    public Text requredBuilding;
    public Text requiredTech;
    public GameObject[] buildingsName;
    public GameObject[] buildingsCost;
    public GameObject[] buildingsTime;
    public GameObject[] buildingsInfo;
    public CityScript tempCity;

    void Awake()
    {
        ClosePanel();
        buildingsName = GameObject.FindGameObjectsWithTag("BuildingText");
        buildingsCost = GameObject.FindGameObjectsWithTag("CostText");
        buildingsTime = GameObject.FindGameObjectsWithTag("BuildTimeText");
        buildingsInfo = GameObject.FindGameObjectsWithTag("BuildingInfoButton");
    }

    public void SetNameCostTime(CityScript city)
    {
        tempCity = city;

        for (int i = 0; i < city.buildings.Count; i++)
        {
            buildingsName[i].GetComponent<Text>().text = city.buildings[i].buildingName;
            buildingsCost[i].GetComponent<Text>().text = "Cost: " + city.buildings[i].cost + " gold";
            buildingsTime[i].GetComponent<Text>().text = "Build time: " + Math.Ceiling(city.buildings[i].buildTime / city.buildingSpeed) + " turns";
            CityBuilding temp = city.buildings[i];
            temp.id = i;
            buildingsInfo[i].GetComponent<Button>().onClick.RemoveAllListeners();
            buildingsInfo[i].GetComponent<Button>().onClick.AddListener(() => {
                OpenPanel(temp);
            });
        }
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void OpenPanel(CityBuilding building)
    {
        description.text = building.description;
        requredBuilding.text = "Required building: " + BuildingName(building.requirement);
        requiredTech.text = "Required techonology: ";
        panel.SetActive(true);
    }

    public string BuildingName(string requirement)
    {
        if (requirement != "") return tempCity.buildings[Int32.Parse(requirement)].buildingName;
        return "";
    }
}
