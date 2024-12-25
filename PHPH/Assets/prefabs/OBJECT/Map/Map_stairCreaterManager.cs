using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class Map_stairCreaterManager : MonoBehaviour
{
    Tilemap tilemap; // Ÿ�ϸ� ������Ʈ ����

    public List<stair> stairPrefab;
    public List<GameObject> spawned_Stair;
    public Vector3 offset_Prefab;

    [Header("��� ���� ���� x ~ y����")]
    public Vector2Int spawn_interval;
    private List<Vector3Int> tilePositions = new List<Vector3Int>(); // Ÿ�� ��ġ ����Ʈ
    private List<GameObject> spawnedStairs = new List<GameObject>(); // ������ ������Ʈ ����Ʈ

    private void Start()
    {
        tilemap=GetComponent<Tilemap>();
        Random_StairSpawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            foreach (GameObject s in spawned_Stair)
            {
                s.transform.position = s.transform.position + offset_Prefab;
            }
        }
    }


    public void Random_StairSpawn()
    {

        // Ÿ�ϸ��� ��� Ÿ�� ��ġ�� �����ɴϴ�.

        tilePositions.Clear();
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            if (tilemap.GetTile(position) != null)
            {
                tilePositions.Add(position); // ��ȿ�� Ÿ�ϸ� ����Ʈ�� �߰�
            }
        }





        // ����Ʈ�� Ÿ�� ��ġ�� ����� ���� �������� ������ ����
        for (int j = spawn_interval.x; j < tilePositions.Count;)
        {

            int randomRotate = Random.Range(0, 3); // ���� ȸ�� ����
            if (randomRotate == 2)
            {
                j += randomRotate; // ���� �������� �ε��� �̵�
                continue;
            }


            int randomNum = Random.Range(spawn_interval.x, spawn_interval.y); // ���� ���� ����
            Vector3 worldPosition = tilemap.CellToWorld(tilePositions[j]); // �� ��ġ�� ���� ��ġ�� ��ȯ

            // ������ ���� (ù ��° ������ ���)
            GameObject spawnedObject = Instantiate(stairPrefab[0].gameObject, worldPosition + offset_Prefab, stairPrefab[0].transform.rotation);
            spawnedObject.transform.GetChild(0).GetComponent<stair_Box>().up_Ground = tilemap.transform.GetComponent<PlatformEffector2D>();
            randomRotate = Random.Range(0, 2); // ���� ȸ�� ����
            if (randomRotate == 0)
            {
                Quaternion rotation = spawnedObject.transform.rotation; // ���� ȸ���� ��������
                rotation = Quaternion.Euler(rotation.eulerAngles.x, 180, rotation.eulerAngles.z); // Y�� ȸ������ 180���� ����
                spawnedObject.transform.rotation = rotation; // ȸ���� ����
            }
            spawnedStairs.Add(spawnedObject); // ������ ������Ʈ ����Ʈ�� �߰�
            j += randomNum; // ���� �������� �ε��� �̵�
        }



    }






}





