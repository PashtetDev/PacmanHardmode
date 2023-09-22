using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private GameObject flor;
    [SerializeField]
    private GameObject floor;
    [SerializeField]
    private float size;
    private List<Vector2> map;

    private void Start()
    {
        FloorGenerate();
        MapDrawer();
        gameObject.name = "Map";
    }

    private void FloorGenerate()
    {
        map = new List<Vector2>
        {
            Vector2.zero
        };
        while (map.Count < size)
        {
            Vector2 newCell = RandomFlor();
            if (!map.Contains(newCell))
                map.Add(newCell);
            if (Random.Range(0, 10) == 0)
                CreateRoom(Random.Range(3, 9), map[Random.Range(0, map.Count - 1)]);
        }
        for (int i = 0; i < 5; i++)
        {
            Vector2 currentCell = map[Random.Range(0, map.Count - 1)];
            if (Vector2.Distance(currentCell, Vector2.zero) > 10)
                CreateIsland(Random.Range(3, 9), map[Random.Range(0, map.Count - 1)]);
            else
                i--;
        }
    }

    private void MapDrawer()
    {
        flor = new GameObject("Flor");
        flor.transform.parent = transform;
        for (int i = 0; i < map.Count; i++)
            Instantiate(floor, new Vector2(map[i].x, map[i].y), Quaternion.identity).transform.parent = flor.transform;
    }

    private Vector2 RandomFlor()
    {
        Vector2 newCell = Vector2.zero;
        Vector2 currentCell = map[Random.Range(0, map.Count - 1)];
        int flag = Random.Range(0, 4);
        switch (flag)
        {
            case 0:
                newCell = currentCell + Vector2.up;
                break;
            case 1:
                newCell = currentCell + Vector2.down;
                break;
            case 2:
                newCell = currentCell + Vector2.left;
                break;
            case 3:
                newCell = currentCell + Vector2.right;
                break;
            default: break;
        }
        return newCell;
    }

    private void CreateRoom(int radius, Vector2 currentCell)
    {
        for (int i = -radius; i < radius; i++)
            for (int j = -radius; j < radius; j++)
            {
                if (!map.Contains(currentCell + new Vector2(i, j)) && (int)Vector2.Distance(currentCell, currentCell + new Vector2(i, j)) < radius)
                    map.Add(currentCell + new Vector2(i, j));
            }    
    }

    private void CreateIsland(int radius, Vector2 currentCell)
    {
        for (int i = -radius; i < radius; i++)
            for (int j = -radius; j < radius; j++)
            {
                if (map.Contains(currentCell + new Vector2(i, j)) 
                    && (int)Vector2.Distance(currentCell, currentCell + new Vector2(i, j)) < radius)
                    map.Remove(currentCell + new Vector2(i, j));
            }
    }
}
