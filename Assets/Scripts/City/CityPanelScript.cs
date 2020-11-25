using System;
using UnityEngine;
using UnityEngine.UI;

public class CityPanelScript : MonoBehaviour
{
    public CityScript city;
    public GameObject panel;
    public MainCameraScript mCamera;
    public Text cityName;
    public Text food;
    public Text happiness;
    public Text cityGold;
    public Text cityResearch;
    public DestroyBuildingConfirm destroy;
    public BuildingUIScript buildingsUI;
    float temp;

    void Start()
    {
        panel.SetActive(false);
        temp = mCamera.zoomSpeed;
    }

    public void MouseClick()
    {
        panel.SetActive(true);
        mCamera.zoomSpeed = 0;
        cityName.GetComponent<Text>().text = city.cityName;
        food.GetComponent<Text>().text = "Food: " + city.food + "/" + city.foodLimit + " (+" + city.foodPerTurn + ")";
        happiness.GetComponent<Text>().text = "Happiness: " + city.happinessCurrent + "/" + city.happinessBase;
        cityGold.GetComponent<Text>().text = "Gold per turn: +" + city.gold;
        cityResearch.GetComponent<Text>().text = "Research per turn: +" + city.research;
        destroy.city = city;
        buildingsUI.SetNameCostTime(city);

        for (int i = 0; i < city.buildings.Count; i++)
        {
            if (city.inConstruction != -1)
            {
                if (city.buildings[i].status == 0)
                    city.buildingButton[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                int tempReq = -1;
                city.buildingButton[i].GetComponent<Button>().interactable = true;

                if (city.buildings[i].requirement != "" && city.buildings[i].status == 0)
                {
                    tempReq = Int32.Parse(city.buildings[i].requirement);
                    if (city.buildings[tempReq].status != 2) city.buildingButton[i].GetComponent<Button>().interactable = false;
                }
            }
            
            if (city.buildings[i].status == 0) city.buildingButton[i].GetComponentInChildren<Text>().text = "Build";
            else city.buildingButton[i].GetComponentInChildren<Text>().text = "Destroy";
        }

        for (int i = 0; i < city.buildings.Count; i++)
        {
            CityBuilding temp = city.buildings[i];
            temp.id = i;
            city.buildingButton[i].GetComponent<Button>().onClick.RemoveAllListeners();

            city.buildingButton[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                temp.status = city.buildings[temp.id].status;
                city.BuildButton(temp, city.buildingButton[temp.id].GetComponent<Button>());
            });

            city.buildings[i] = temp;
        }
    }

    public void ClosePanel()
    {
        mCamera.zoomSpeed = temp;
        panel.SetActive(false);
    }
}
