using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject bird;
    [SerializeField]
    private GameObject enemyBullet;

    [SerializeField]
    private GameObject bonfire;
    [SerializeField]
    private GameObject pipe;
    [SerializeField]
    private GameObject portal;
    [SerializeField]
    private GameObject player;
    private GameObject objecManager;
    [SerializeField]
    private GameObject point, bigPoint, pickUp;
    public static MapGenerator instance;
    private GameObject wallManager;
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private List<GameObject> treesPrefabs;
    [SerializeField]
    private float size;
    [SerializeField]
    private GameObject[] enemy;
    private List<Vector2Int> map;
    private List<Vector2Int> walls;
    private List<GameObject> enemies;
    private List<Vector2Int> subjects;
    [SerializeField]
    private bool isBoss;

    public void Awake()
    {
        subjects = new List<Vector2Int>();
        instance = this;
        if (isBoss)
        {
            RoomCreator();
        }
        else
        {
            FloorGenerate();
            EnemyFiller();
        }
        WallFiller();
        objecManager = new GameObject("ObjectManager");
        objecManager.transform.parent = transform;


        if (!isBoss)
        {
            if (PlayerController.instance.myBoosts.isFire > 0)
                BonfireFiller();
            BigPointFiller();
            PickUp();
            PointFiller();
        }
        CreateRoom(10, Vector2Int.zero);
        MapDrawer();
        gameObject.name = "Map";
        StartCoroutine(Wait());
    }

    private void RoomCreator()
    {
        map = new List<Vector2Int>
        {
            Vector2Int.zero
        };
        Vector2Int offset = new Vector2Int(-20, 0);
        for (int i = 0; i < 25; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                map.Add(offset + new Vector2Int(i, j));
            }
        }
        PlayerCreate();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        if (PlayerController.instance.myBoosts.bird != 0)
            StartCoroutine(SpawnerBird());
        if (PlayerController.instance.myBoosts.enemyBullet != 0)
            StartCoroutine(SpawnerBullet());
    }

    private void PointFiller()
    {
        for (int i = 0; i < map.Count; i++)
        {
            if (!subjects.Contains(map[i]))
            {
                Instantiate(point, (Vector2)map[i], Quaternion.identity).transform.parent = objecManager.transform;
                subjects.Add(map[i]);
            }
        }
    }

    private void BonfireFiller()
    {
        int count = 0;
        int maxCount = PlayerController.instance.myBoosts.isFire * 10;
        Debug.Log(maxCount);
        while (count < maxCount)
        {
            count++;
            Vector2Int position = RandomCell();
            if (!subjects.Contains(position))
            {
                subjects.Add(position);
                Instantiate(bonfire, (Vector2)position, Quaternion.identity);
            }
        }
    }

    private GameObject PlayerCreate()
    {
        if (isBoss)
        {
            GameObject myPlayer = null;
            Vector2Int currentPosition = new Vector2Int(-15, 2);
            if (PlayerController.instance == null)
            {
                myPlayer = Instantiate(player, (Vector2)currentPosition, Quaternion.identity);
                myPlayer.GetComponent<PlayerController>().Initialize();
            }
            else
            {
                myPlayer = PlayerController.instance.gameObject;
            }
            return myPlayer;
        }
        else
        {
            GameObject myPlayer = null;
            Vector2Int currentPosition = RandomCell();
            if (PlayerController.instance == null)
            {
                myPlayer = Instantiate(player, (Vector2)currentPosition, Quaternion.identity);
                myPlayer.GetComponent<PlayerController>().Initialize();
            }
            else
            {
                myPlayer = PlayerController.instance.gameObject;
            }
            return myPlayer;
        }
    }

    private void PickUp()
    {
        int count = 0;
        while (count != 1)
        {
            Vector2Int currentCell = RandomCell();
            if (!subjects.Contains(currentCell))
            {
                Instantiate(pickUp, (Vector2)currentCell, Quaternion.identity).transform.parent = objecManager.transform;
                subjects.Add(currentCell);
                count++;
            }
        }
    }

    private void BigPointFiller()
    {
        int count = 0;
        while (count != 2)
        {
            Vector2Int currentCell = RandomCell();
            if (!subjects.Contains(currentCell))
            {
                Instantiate(bigPoint, (Vector2)currentCell, Quaternion.identity).transform.parent = objecManager.transform;
                subjects.Add(currentCell);
                count++;
            }
        }
    }

    private void EnemyFiller()
    {
        enemies = new List<GameObject>();
        while (enemies.Count != 4)
        {
            Vector2Int place = RandomCell();
            if (Vector2Int.Distance(place, Vector2Int.zero) > 15)
            {
                enemies.Add(Instantiate(enemy[enemies.Count], (Vector2)place, Quaternion.identity));
                subjects.Add(place);
            }
        }

        for (int i = 0; i < subjects.Count; i++)
            CreateRoom(Random.Range(10, 15), subjects[i]);

        InitBandit();
    }

    private void WallFiller()
    {
        walls = new List<Vector2Int>();
        for (int i = 0; i < map.Count; i++)
        {
            Vector2Int currentCell = map[i];
            if (!map.Contains(currentCell + Vector2Int.up) && !walls.Contains(currentCell + Vector2Int.up))
                walls.Add(currentCell + Vector2Int.up);
            if (!map.Contains(currentCell + Vector2Int.down) && !walls.Contains(currentCell + Vector2Int.down))
                walls.Add(currentCell + Vector2Int.down);
            if (!map.Contains(currentCell + Vector2Int.left) && !walls.Contains(currentCell + Vector2Int.left))
                walls.Add(currentCell + Vector2Int.left);
            if (!map.Contains(currentCell + Vector2Int.right) && !walls.Contains(currentCell + Vector2Int.right))
                walls.Add(currentCell + Vector2Int.right);
        }
    }

    private void FloorGenerate()
    {
        map = new List<Vector2Int>
        {
            Vector2Int.zero
        };
        while (map.Count < size)
        {
            Vector2Int newCell = RandomFlor();
            if (!map.Contains(newCell))
                map.Add(newCell);
            if (Random.Range(0, 10) == 0)
                CreateRoom(Random.Range(3, 9), map[Random.Range(0, map.Count - 1)]);
        }

        GameObject myPlayer = PlayerCreate();

        //int count = 0;
        //while (count != 5)
        //{
        //    int radius = Random.Range(3, 9);
        //    Vector2Int currentCell = RandomCell();
        //    if (Vector2.Distance(currentCell, myPlayer.transform.position) > 10 + radius)
        //    {
        //        CreateIsland(radius, currentCell);
        //        count++;
        //    }

        //}
    }

    private void MapDrawer()
    {
        wallManager = new GameObject("Wall");
        wallManager.transform.parent = transform;
        for (int i = 0; i < walls.Count; i++)
            Instantiate(wallPrefab, new Vector2(walls[i].x, walls[i].y), Quaternion.identity).transform.parent = wallManager.transform;
    }

    public void InitBandit()
    {
        for (int i = 0; i < enemies.Count; i++)
            enemies[i].GetComponent<Bandit>().Initialize();
    }

    private Vector2Int RandomFlor()
    {
        Vector2Int newCell = Vector2Int.zero;
        Vector2Int currentCell = map[Random.Range(0, map.Count - 1)];
        int flag = Random.Range(0, 4);
        switch (flag)
        {
            case 0:
                newCell = currentCell + Vector2Int.up;
                break;
            case 1:
                newCell = currentCell + Vector2Int.down;
                break;
            case 2:
                newCell = currentCell + Vector2Int.left;
                break;
            case 3:
                newCell = currentCell + Vector2Int.right;
                break;
            default: break;
        }
        return newCell;
    }

    private void CreateRoom(int radius, Vector2Int currentCell)
    {
        for (int i = -radius; i < radius; i++)
            for (int j = -radius; j < radius; j++)
            {
                if (!map.Contains(currentCell + new Vector2Int(i, j)) && (int)Vector2Int.Distance(currentCell, currentCell + new Vector2Int(i, j)) < radius)
                    map.Add(currentCell + new Vector2Int(i, j));
            }
    }

    private void CreateIsland(int radius, Vector2Int currentCell)
    {
        for (int i = -radius; i < radius; i++)
            for (int j = -radius; j < radius; j++)
            {
                if (map.Contains(currentCell + new Vector2Int(i, j))
                    && (int)Vector2Int.Distance(currentCell, currentCell + new Vector2Int(i, j)) < radius)
                    map.Remove(currentCell + new Vector2Int(i, j));
            }
    }

    public Vector2Int RandomCell()
    {
        return map[Random.Range(0, map.Count)];
    }


    public void DeleteEnemy(GameObject enemy)
    {
        Vector2 portalPosition = enemy.transform.position;
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            StartCoroutine(Wait(portalPosition));
        }

    }

    private IEnumerator Wait(Vector2 position)
    {
        yield return new WaitForSeconds(1);
        if (!PlayerController.instance.myBoosts.boss && PlayerController.instance.myBoosts.points < 1000)
            Instantiate(portal, position, Quaternion.identity);
        else
            Instantiate(pipe, position, Quaternion.identity);

    }

    private IEnumerator SpawnerBullet()
    {
        while (!PlayerController.instance.isLose)
        {
            if (Random.Range(0, 2) == 0)
                Instantiate(enemyBullet, PlayerController.instance.transform.position + new Vector3(Random.Range(13, 15), Random.Range(-8, 8), 0), Quaternion.identity);
            else
                Instantiate(enemyBullet, PlayerController.instance.transform.position + new Vector3(Random.Range(-13, -15), Random.Range(-8, 8), 0), Quaternion.identity);

            yield return new WaitForSeconds(2 / Mathf.Log(PlayerController.instance.myBoosts.enemyBullet + 1));
        }
    }
    private IEnumerator SpawnerBird()
    {
        while (!PlayerController.instance.isLose)
        {
            if (Random.Range(0, 2) == 0)
                Instantiate(bird, PlayerController.instance.transform.position + new Vector3(Random.Range(13, 15), Random.Range(-8, 8), 0), Quaternion.identity);
            else
                Instantiate(bird, PlayerController.instance.transform.position + new Vector3(Random.Range(-13, -15), Random.Range(-8, 8), 0), Quaternion.identity);
            yield return new WaitForSeconds(2 / Mathf.Log(PlayerController.instance.myBoosts.bird + 1));
        }
    }

}
