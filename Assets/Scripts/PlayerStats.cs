using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Gold:")]
    public float total;
    public float perTurn;
    public Text gold;

    [Header("Research:")]
    public float totalResearch;
    public float perTurnResearch;
    public Text research;

    [Header("Turns:")]
    public Text turnNumberText;
    public float turnNumber = 0;

    [Header("Win Condition:")]
    public float winCondition = 0;
    public Text progress;

    [Header ("")]
    public List<CityScript> cities;

    void Awake()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
            // C:\Users\{name}\AppData\LocalLow\DefaultCompany\Turn based strategy 1
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/");

        GameObject[] citiesObject = GameObject.FindGameObjectsWithTag("City");
        cities = new List<CityScript>();

        foreach (GameObject cityObject in citiesObject)
        {
            cities.Add(cityObject.GetComponent<CityScript>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SaveName.saveNameObj.saveName != "")
        {
            SavesScript.control.Load(SaveName.saveNameObj.saveName);
            SaveName.saveNameObj.saveName = "";
        }

        PerTurnValue();
    }

    public void PerTurn()
    {
        total += perTurn;
        totalResearch += perTurnResearch;
        InfoRefresh();
    }

    public void ModifyTotal(string type, float cost = 0)
    {
        switch (type)
        {
            case "gold":
                total -= cost;
                gold.text = "Gold: " + total + " (+" + perTurn + ")";
                break;
            case "research":
                totalResearch -= cost;
                research.text = "Research: " + totalResearch + " (+" + perTurnResearch + ")";
                break;
            default:
                break;
        }
    }

    public bool CheckTotal(float cost, float value)
    {
        if (value < cost) return false;
        else return true;
    }

    public void PerTurnValue()
    {
        perTurn = 0;
        perTurnResearch = 0;

        foreach (CityScript city in cities)
        {
            perTurn += city.gold;
            perTurnResearch += city.research;
        }

        InfoRefresh();
    }

    public void InfoRefresh()
    {
        gold.text = "Gold: " + total + " (+" + perTurn + ")";
        research.text = "Research: " + totalResearch + " (+" + perTurnResearch + ")";
        turnNumberText.text = "Turn: " + turnNumber;
        progress.text = "Progress: " + winCondition + "/11";
    }
}
