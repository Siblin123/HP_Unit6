using UnityEngine;
using System.IO;
using System.Collections.Generic;
using NPOI.XSSF.UserModel; // .xlsx 지원
using NPOI.SS.UserModel;

public class Item_Crafting_Data : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public int itemType;         // 아이템 타입 (1: 장비, 2: 도구, 3: 공격, 4: 소비, 5: 설치)
        public string itemName;      // 완성 아이템 이름
        public List<string> materials; // 조합법 (재료 리스트)
        public bool hasMemory;       // 지식의 기억 (0이면 false, 0이 아니면 true)
        public int price;            // 가격
    }

    [SerializeField] private List<ItemData> itemList = new List<ItemData>(); // 🔹 인스펙터에서 보기

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "item_data.xlsx");
        itemList = ReadExcel(path);
    }

    List<ItemData> ReadExcel(string path)
    {
        List<ItemData> itemList = new List<ItemData>();

        if (!File.Exists(path))
        {
            Debug.LogError("엑셀 파일이 존재하지 않습니다: " + path);
            return itemList;
        }

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheetAt(0); // 첫 번째 시트

            for (int i = 4; i <= sheet.LastRowNum; i++) // 5행부터 데이터 읽기 (0-based index)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;



                // 🔹 A열 (완성 아이템)
                ICell completeItemCell = row.GetCell(1);

                double cellValue = -1; // 기본값
                if (completeItemCell.CellType == CellType.Formula)
                {
                    if (completeItemCell.CachedFormulaResultType == CellType.Numeric)
                    {
                        cellValue = completeItemCell.NumericCellValue;
                    }
                }
                // 수식 결과값이 0이면 반복문 종료
                if (cellValue == 0)
                {
                    Debug.Log("완성 아이템 셀 값이 0이므로 반복문 종료");
                    break;
                }



                string rawItemName = GetCellValue(completeItemCell);
                if (string.IsNullOrEmpty(rawItemName) || !char.IsDigit(rawItemName[0])) continue;

                int itemType = int.Parse(rawItemName[0].ToString()); // 숫자로 타입 구분
                string itemName = rawItemName.Substring(2).Trim(); // '1. ' 같은 숫자 부분 제거

                // 🔹 B열 (조합법) → `+` 기호 기준으로 분리
                ICell recipeCell = row.GetCell(2);
                List<string> materials = new List<string>();
                string rawRecipe = GetCellValue(recipeCell);
                if (!string.IsNullOrEmpty(rawRecipe))
                {
                    materials.AddRange(rawRecipe.Split(new[] { '+' }, System.StringSplitOptions.RemoveEmptyEntries));
                    for (int j = 0; j < materials.Count; j++)
                    {
                        materials[j] = materials[j].Trim();
                    }
                }

                // 🔹 C열 (지식의 기억) → 0이면 false, 0이 아니면 true
                ICell memoryCell = row.GetCell(3);
               
                bool hasMemory = false;
                if ((GetCellNumericValue(memoryCell) != 0))
                {
                    hasMemory = true;
                }                  
               

                // 🔹 D열 (가격) → int 변환
                ICell priceCell = row.GetCell(4);
                int price = GetCellNumericValue(priceCell);

                // 🔹 리스트에 추가
                ItemData itemData = new ItemData
                {
                    itemType = itemType,
                    itemName = itemName,
                    materials = materials,
                    hasMemory = hasMemory,
                    price = price
                };

                itemList.Add(itemData);
            }
        }

        return itemList;
    }

    // 🔹 문자열 값을 안전하게 가져오기 (수식 포함)
    string GetCellValue(ICell cell)
    {
        if (cell == null) return "";

        switch (cell.CellType)
        {
            case CellType.String:
                return cell.StringCellValue.Trim();

            case CellType.Numeric:
                return cell.NumericCellValue.ToString();

            case CellType.Boolean:
                return cell.BooleanCellValue ? "true" : "false";

            case CellType.Formula:
                return GetFormulaResult(cell); // 수식 결과 가져오기

            default:
                return "";
        }
    }

    // 🔹 수식이 있는 경우 계산된 값 가져오기
    string GetFormulaResult(ICell cell)
    {
        if (cell == null || cell.CellType != CellType.Formula) return "";

        switch (cell.CachedFormulaResultType)
        {
            case CellType.String:
                return cell.StringCellValue.Trim();

            case CellType.Numeric:
                return cell.NumericCellValue.ToString();

            case CellType.Boolean:
                return cell.BooleanCellValue ? "true" : "false";

            default:
                return "";
        }
    }

    // 🔹 숫자 값을 안전하게 가져오기 (수식 포함)
    int GetCellNumericValue(ICell cell)
    {
        if (cell == null) return 0;

        switch (cell.CellType)
        {
            case CellType.Numeric:
                return (int)cell.NumericCellValue;

            case CellType.Formula:
                if (cell.CachedFormulaResultType == CellType.Numeric)
                {
                    return (int)cell.NumericCellValue;
                }
                else if (cell.CachedFormulaResultType == CellType.String)
                {
                    return -99999;
                }
                break;
        }

        return 0;
    }
}
