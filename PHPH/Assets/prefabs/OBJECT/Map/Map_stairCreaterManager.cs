using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class Map_stairCreaterManager : MonoBehaviour
{
    Tilemap tilemap; // 타일맵 컴포넌트 참조

    public List<stair> stairPrefab;
    public List<GameObject> spawned_Stair;
    public Vector3 offset_Prefab;

    [Header("계단 생성 간격 x ~ y사이")]
    public Vector2Int spawn_interval;
    private List<Vector3Int> tilePositions = new List<Vector3Int>(); // 타일 위치 리스트
    private List<GameObject> spawnedStairs = new List<GameObject>(); // 생성된 오브젝트 리스트

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

        // 타일맵의 모든 타일 위치를 가져옵니다.

        tilePositions.Clear();
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            if (tilemap.GetTile(position) != null)
            {
                tilePositions.Add(position); // 유효한 타일만 리스트에 추가
            }
        }





        // 리스트의 타일 위치를 사용해 랜덤 간격으로 프리팹 생성
        for (int j = spawn_interval.x; j < tilePositions.Count;)
        {

            int randomRotate = Random.Range(0, 3); // 랜덤 회전 결정
            if (randomRotate == 2)
            {
                j += randomRotate; // 랜덤 간격으로 인덱스 이동
                continue;
            }


            int randomNum = Random.Range(spawn_interval.x, spawn_interval.y); // 랜덤 간격 결정
            Vector3 worldPosition = tilemap.CellToWorld(tilePositions[j]); // 셀 위치를 월드 위치로 변환

            // 프리팹 생성 (첫 번째 프리팹 사용)
            GameObject spawnedObject = Instantiate(stairPrefab[0].gameObject, worldPosition + offset_Prefab, stairPrefab[0].transform.rotation);
            spawnedObject.transform.GetChild(0).GetComponent<stair_Box>().up_Ground = tilemap.transform.GetComponent<PlatformEffector2D>();
            randomRotate = Random.Range(0, 2); // 랜덤 회전 결정
            if (randomRotate == 0)
            {
                Quaternion rotation = spawnedObject.transform.rotation; // 기존 회전값 가져오기
                rotation = Quaternion.Euler(rotation.eulerAngles.x, 180, rotation.eulerAngles.z); // Y축 회전값을 180으로 설정
                spawnedObject.transform.rotation = rotation; // 회전값 적용
            }
            spawnedStairs.Add(spawnedObject); // 생성된 오브젝트 리스트에 추가
            j += randomNum; // 랜덤 간격으로 인덱스 이동
        }



    }






}





