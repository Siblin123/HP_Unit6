using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;
public class Map_ObjectCreateManager : NetworkBehaviour
{
    public MapObject[] mapObjects;
    public Vector3 offset_Prefab;

    PlatformEffector2D[] all_MapGround;

    [Header("������ ������Ʈ�� x ~ y����")]
    public Vector2Int spawn_Countinterval;

    private List<Vector3Int> tilePositions = new List<Vector3Int>(); // Ÿ�� ��ġ ����Ʈ
    private List<GameObject> spawnedMapObject = new List<GameObject>(); // ������ ������Ʈ ����Ʈ

    private void Start()
    {
        all_MapGround = GetComponentsInChildren<PlatformEffector2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Create_mapObject();
        }
    }

    public void Create_mapObject()
    {
        for (int k = 0; k < all_MapGround.Length; k++)
        {
            tilePositions.Clear();
            BoundsInt bounds = all_MapGround[k].GetComponent<Tilemap>().cellBounds;

            foreach (Vector3Int position in bounds.allPositionsWithin)
            {
                if (all_MapGround[k].GetComponent<Tilemap>().GetTile(position) != null)
                {
                    tilePositions.Add(position); // ��ȿ�� Ÿ�ϸ� ����Ʈ�� �߰�
                }
            }

            int randocountmNum = Random.Range(spawn_Countinterval.x, spawn_Countinterval.y); // ���� ���� ��
            HashSet<int> usedIndices = new HashSet<int>(); // ���� �ε����� �����ϱ� ���� HashSet

            for (int i = 0; i < randocountmNum; i++)
            {
                if (usedIndices.Count >= tilePositions.Count)
                {
                    break; // ��� Ÿ�� ��ġ�� ���� ��� ���� ����
                }

                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, tilePositions.Count); // ���� �ε��� ����
                } while (usedIndices.Contains(randomIndex)); // �̹� ���� �ε����� �ٽ� �������� ����

                usedIndices.Add(randomIndex); // ���� �ε��� �߰�

                Vector3 worldPosition = all_MapGround[k].GetComponent<Tilemap>().CellToWorld(tilePositions[randomIndex]); // �� ��ġ�� ���� ��ġ�� ��ȯ
                int randomMapObject = Random.Range(0, mapObjects.Length); // ���� ������ �ε��� ����

                // ������ ����
                GameObject spawnedObject = Instantiate(mapObjects[randomMapObject].gameObject, worldPosition + offset_Prefab, mapObjects[randomMapObject].transform.rotation);
                spawnedObject.GetComponent<NetworkObject>().Spawn(); // ��Ʈ��ũ ������Ʈ�� ����
                spawnedMapObject.Add(spawnedObject); // ������ ������Ʈ ����Ʈ�� �߰�


            }
        }
    }
}
