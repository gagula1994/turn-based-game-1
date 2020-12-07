using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIScript : MonoBehaviour
{
    public GameObject panel;
    public Text description;
    public Text requredBuilding;
    public Text requiredTech;
    public GameObject buildingUI;
    public List<GameObject> buildingsUI;
    public CityScript tempCity;

    void Awake()
    {
        ClosePanel();

        for (int i = 0; i < buildingUI.transform.childCount; i++)
        {
            buildingsUI.Add(buildingUI.transform.GetChild(i).gameObject);
        }
    }

    public void SetNameCostTime(CityScript city)
    {
        tempCity = city;

        for (int i = 0; i < city.buildings.Count; i++)
        {
            CityBuilding temp = city.buildings[i];
            temp.id = i;

            buildingsUI[i].transform.Find("BuildingText").GetComponent<Text>().text = city.buildings[i].buildingName;
            buildingsUI[i].transform.Find("CostText").GetComponent<Text>().text = "Cost: " + city.buildings[i].cost + " gold";
            buildingsUI[i].transform.Find("BuildTimeText").GetComponent<Text>().text = "Build time: " + Math.Ceiling(city.buildings[i].buildTime / city.buildingSpeed) + " turns";
            buildingsUI[i].transform.Find("BuildingInfoButton").GetComponent<Button>().onClick.RemoveAllListeners();
            buildingsUI[i].transform.Find("BuildingInfoButton").GetComponent<Button>().onClick.AddListener(() => {
                OpenPanel(temp);
            });
            buildingsUI[i].transform.Find("BuildingButton").GetComponent<Button>().onClick.RemoveAllListeners();
            buildingsUI[i].transform.Find("BuildingButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                temp.status = city.buildings[temp.id].status;
                city.BuildButton(temp, buildingsUI[temp.id].transform.Find("BuildingButton").GetComponent<Button>());
            });

            city.buildings[i] = temp;
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
