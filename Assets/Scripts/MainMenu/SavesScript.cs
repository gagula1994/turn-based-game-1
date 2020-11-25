using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavesScript : MonoBehaviour
{
    public static SavesScript control;
    BinaryFormatter bf;

    void Awake()
    {
        if (control == null) control = this;
        else Destroy(gameObject);
        bf = new BinaryFormatter();
    }

    public void LoadMain()
    {
        string saveName = SaveFileName();
        SaveName.saveNameObj.saveName = saveName;
        SceneManager.LoadScene("GameScene");
    }

    public void Load(string saveMain = "")
    {
        string saveName;

        if (saveMain != "") saveName = saveMain;
        else saveName = SaveFileName();
        
        if (File.Exists(Application.persistentDataPath + "/saves/" + saveName))
        {
            // C:\Users\{name}\AppData\LocalLow\DefaultCompany\Turn based strategy 1
            FileStream file = File.Open(Application.persistentDataPath + "/saves/" + saveName, FileMode.Open);
            SaveGame save = (SaveGame)bf.Deserialize(file);
            Camera mCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            List<CityScript> cities = CitiesList();

            mCamera.orthographicSize = save.zoomLevel;

            playerStats.total = save.gold;
            playerStats.turnNumber = save.turnNumber;
            playerStats.totalResearch = save.research;
            playerStats.winCondition = save.winCondition;

            for (int i = 0; i < cities.Count; i++)
            {
                CityScript tempC = cities[i];
                tempC.id = i;

                tempC.inConstruction = save.inConstrunction[i];
                tempC.buildingSpeedBonus = save.buildingSpeedBonus[i];
                tempC.buildingModifier = save.buildingModifier[i];
                tempC.food = save.food[i];
                tempC.foodLimit = save.foodLimit[i];
                tempC.foodBonus = save.foodBonus[i];
                tempC.foodModifier = save.foodModifier[i];
                tempC.goldBonus = save.goldBonus[i];
                tempC.goldModifier = save.goldModifier[i];
                tempC.happinessBase = save.happinessBase[i];
                tempC.happinessCurrent = save.happinessCurrent[i];
                tempC.population = save.population[i];
                tempC.populationLimit = save.populationLimit[i];
                tempC.researchBonus = save.researchBonus[i];
                tempC.researchModifier = save.researchModifier[i];

                for (int j = 0; j < cities[0].buildings.Count; j++)
                {
                    CityBuilding temp = tempC.buildings[j];
                    temp.id = j;
                    temp = save.buildings[i, j];
                    tempC.buildings[temp.id] = temp;
                }

                tempC.StatsModify();
                tempC.PopulationGUI();
                if (tempC.inConstruction != -1) tempC.ConstructionUpdate(tempC.buildings[tempC.inConstruction]);
                else tempC.NoMoreConstuction();

                cities[tempC.id] = tempC;
            }

            playerStats.PerTurnValue();
            file.Close();
        }
    }

    public void Save()
    {
        FileStream file = File.Create(Application.persistentDataPath + "/saves/" + DateTime.Now.ToString("yyyy'-'M'-'d'-'H'-'m'-'s") + ".game");
        SaveGame save = new SaveGame();
        Camera mCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        List<CityScript> cities = CitiesList();

        save.SetSaveGame(mCamera.orthographicSize, playerStats, cities);
        bf.Serialize(file, save);
        file.Close();
    }

    public string SaveFileName()
    {
        GameObject[] savesToggle = GameObject.FindGameObjectsWithTag("SaveToggle");
        string saveName = "";

        foreach (GameObject save in savesToggle)
        {
            if (save.GetComponent<Toggle>().isOn)
            {
                saveName = save.GetComponentInChildren<Text>().text;
            }
        }

        return saveName;
    }

    public List<CityScript> CitiesList()
    {
        List<CityScript> cities = new List<CityScript>();

        GameObject[] citiesObject = GameObject.FindGameObjectsWithTag("City");
        foreach (GameObject city in citiesObject)
        {
            cities.Add(city.GetComponent<CityScript>());
        }

        return cities;
    }
}

[System.Serializable]
public class SaveGame
{
    public float zoomLevel;

    public float gold;
    public float research;
    public float turnNumber;
    public float winCondition;

    public int[] inConstrunction;
    public float[] buildingSpeedBonus;
    public float[] buildingModifier;
    public float[] food;
    public float[] foodLimit;
    public float[] foodBonus;
    public float[] foodModifier;
    public float[] goldBonus;
    public float[] goldModifier;
    public float[] happinessBase;
    public float[] happinessCurrent;
    public float[] population;
    public float[] populationLimit;
    public float[] researchBonus;
    public float[] researchModifier;
    public CityBuilding[,] buildings;

    public void SetSaveGame(float zoomLevel, PlayerStats player, List<CityScript> cities)
    {
        this.zoomLevel = zoomLevel;

        gold = player.total;
        research = player.totalResearch;
        turnNumber = player.turnNumber;
        winCondition = player.winCondition;

        inConstrunction = new int[cities.Count];
        buildingSpeedBonus = new float[cities.Count];
        buildingModifier = new float[cities.Count];
        food = new float[cities.Count];
        foodLimit = new float[cities.Count];
        foodBonus = new float[cities.Count];
        foodModifier = new float[cities.Count];
        goldBonus = new float[cities.Count];
        goldModifier = new float[cities.Count];
        happinessBase = new float[cities.Count];
        happinessCurrent = new float[cities.Count];
        population = new float[cities.Count];
        populationLimit = new float[cities.Count];
        researchBonus = new float[cities.Count];
        researchModifier = new float[cities.Count];
        buildings = new CityBuilding[cities.Count, cities[0].buildings.Count];

        for (int i = 0; i < cities.Count; i++)
        {
            inConstrunction[i] = cities[i].inConstruction;
            buildingSpeedBonus[i] = cities[i].buildingSpeedBonus;
            buildingModifier[i] = cities[i].buildingModifier;
            food[i] = cities[i].food;
            foodLimit[i] = cities[i].foodLimit;
            foodBonus[i] = cities[i].foodBonus;
            foodModifier[i] = cities[i].foodModifier;
            goldBonus[i] = cities[i].goldBonus;
            goldModifier[i] = cities[i].goldModifier;
            happinessBase[i] = cities[i].happinessBase;
            happinessCurrent[i] = cities[i].happinessCurrent;
            population[i] = cities[i].population;
            populationLimit[i] = cities[i].populationLimit;
            researchBonus[i] = cities[i].researchBonus;
            researchModifier[i] = cities[i].researchModifier;

            for (int j = 0; j < cities[0].buildings.Count; j++)
            {
                buildings[i, j] = cities[i].buildings[j];
            }
        }
    }
}