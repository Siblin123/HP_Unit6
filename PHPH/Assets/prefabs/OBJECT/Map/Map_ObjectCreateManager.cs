using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;
public class Map_ObjectCreateManager : NetworkBehaviour
{
    public MapObject[] mapObjects;
    public Vector3 offset_Prefab;

    PlatformEffector2D[] all_MapGround;

    [Header("생성될 오브젝트수 x ~ y사이")]
    public Vector2Int spawn_Countinterval;

    private List<Vector3Int> tilePositions = new List<Vector3Int>(); // 타일 위치 리스트
    private List<GameObject> spawnedMapObject = new List<GameObject>(); // 생성된 오브젝트 리스트

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
                    tilePositions.Add(position); // 유효한 타일만 리스트에 추가
                }
            }

            int randocountmNum = Random.Range(spawn_Countinterval.x, spawn_Countinterval.y); // 랜덤 생성 수
            HashSet<int> usedIndices = new HashSet<int>(); // 사용된 인덱스를 추적하기 위한 HashSet

            for (int i = 0; i < randocountmNum; i++)
            {
                if (usedIndices.Count >= tilePositions.Count)
                {
                    break; // 모든 타일 위치가 사용된 경우 루프 종료
                }

                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, tilePositions.Count); // 랜덤 인덱스 결정
                } while (usedIndices.Contains(randomIndex)); // 이미 사용된 인덱스는 다시 선택하지 않음

                usedIndices.Add(randomIndex); // 사용된 인덱스 추가

                Vector3 worldPosition = all_MapGround[k].GetComponent<Tilemap>().CellToWorld(tilePositions[randomIndex]); // 셀 위치를 월드 위치로 변환
                int randomMapObject = Random.Range(0, mapObjects.Length); // 랜덤 프리팹 인덱스 결정

                // 프리팹 생성
                GameObject spawnedObject = Instantiate(mapObjects[randomMapObject].gameObject, worldPosition + offset_Prefab, mapObjects[randomMapObject].transform.rotation);
                spawnedObject.GetComponent<NetworkObject>().Spawn(); // 네트워크 오브젝트로 스폰
                spawnedMapObject.Add(spawnedObject); // 생성된 오브젝트 리스트에 추가


            }
        }
    }
}
