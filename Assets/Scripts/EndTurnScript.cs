using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnScript : MonoBehaviour
{
    public PlayerStats player;
    public GameObject panel;
    public GameObject endGamePanel;
    public List<CityScript> cities;
    public static EndTurnScript endTurn;

    void Awake()
    {
        if (endTurn == null) endTurn = this;
        else Destroy(gameObject);

        endGamePanel.SetActive(false);
    }

    public void NewTurn()
    {
        player.turnNumber++;
        player.turnNumberText.text = "Turn: " + player.turnNumber;

        for (int i = 0; i < cities.Count; i++)
        {
            CityScript temp = cities[i];
            temp.id = i;
            if (temp.inConstruction != -1) temp.CheckTurn(temp.buildings[temp.inConstruction]);
            temp.PopulationModify();
            temp.HappinessModify();
            cities[temp.id] = temp;
        }

        player.PerTurn();
        panel.SetActive(false);

        if (player.turnNumber == 126)
        {
            endGamePanel.GetComponentInChildren<Text>().text += "Lost!";
            endGamePanel.SetActive(true);
        }

        if (player.winCondition == 11)
        {
            endGamePanel.GetComponentInChildren<Text>().text += "Won!";
            endGamePanel.SetActive(true);
        }
    }

    public void Special(int id)
    {
        for (int i = 0; i < cities.Count; i++)
        {
            CityScript temp = cities[i];
            temp.id = i;
            CityBuilding tempBuilding = temp.buildings[id];
            tempBuilding.status = 2;
            temp.buildingButton[id].SetActive(false);
            temp.buildings[id] = tempBuilding;
            cities[temp.id] = temp;
        }

        player.winCondition++;
        player.InfoRefresh();
    }
}
