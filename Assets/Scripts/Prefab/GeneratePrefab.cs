using System.Collections.Generic;
using UnityEngine;

public class GeneratePrefab : MonoBehaviour
{
    public int size = 0;
    public int height;
    public List<GameObject> prefabList;
    public GameObject prefab;

    public void GeneratePrefabs(string name, int value = 0)
    {
        if (value != 0) size = value;
        for (int i = 0; i < size; i++)
        {
            GameObject temp = Instantiate(prefab, this.transform);
            temp.transform.localPosition = new Vector2(0, height * -i);
            prefabList.Add(temp);
            temp.name = name + " " + i;
        }
    }

    public void DestroyPrefab()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        prefabList.Clear();
    }
}
