using System.IO;
using UnityEngine;

public class GenerateMatrix : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Starting GenerateMatrix...");

        // Tạo ma trận 120x120
        int[,] map = new int[120, 120];

        // Khởi tạo toàn bộ ma trận là tường (1)
        for (int x = 0; x < 120; x++)
        {
            for (int y = 0; y < 120; y++)
            {
                map[x, y] = 1;
            }
        }

        // Tạo đường đi đơn giản
        for (int x = 1; x < 119; x++)
        {
            map[x, 1] = 0; // Đường đi ngang từ điểm bắt đầu
        }
        for (int y = 1; y < 119; y++)
        {
            map[118, y] = 0; // Đường đi dọc đến điểm kết thúc
        }
        for (int x = 2; x < 118; x += 10)
        {
            for (int y = 2; y < 118; y += 5)
            {
                map[x, y] = 0; // Nhánh ngẫu nhiên
            }
        }

        Debug.Log("Map initialized. Sample value at [1,1]: " + map[1, 1]);

        // Lưu vào JSON
        SaveToJson(map);
    }

    void SaveToJson(int[,] map)
    {
        if (map == null)
        {
            Debug.LogError("Map is null!");
            return;
        }

        // Khởi tạo MapData
        MapData mapData = new MapData();
        mapData.map = new int[map.GetLength(0)][];

        // Chuyển từ int[,] sang int[][]
        for (int x = 0; x < map.GetLength(0); x++)
        {
            mapData.map[x] = new int[map.GetLength(1)];
            for (int y = 0; y < map.GetLength(1); y++)
            {
                mapData.map[x][y] = map[x, y];
            }
        }

        // Kiểm tra dữ liệu
        if (mapData.map == null || mapData.map.Length == 0)
        {
            Debug.LogError("mapData.map is empty or null!");
            return;
        }

        Debug.Log("mapData.map size: " + mapData.map.Length + "x" + mapData.map[0].Length);
        Debug.Log("Sample value in mapData.map[1][1]: " + mapData.map[1][1]);

        try
        {
            // Serialize thành JSON
            string json = JsonUtility.ToJson(mapData, true);
            if (string.IsNullOrEmpty(json) || json == "{}")
            {
                Debug.LogError("JSON serialization failed! JSON content: " + json);
                return;
            }

            // Ghi file
            string path = Application.dataPath + "/map.json";
            File.WriteAllText(path, json);
            Debug.Log("Saved map to " + path);

            // Kiểm tra nội dung file
            string savedJson = File.ReadAllText(path);
            Debug.Log("File content length: " + savedJson.Length);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving JSON: " + e.Message);
        }
    }

    [System.Serializable]
    public class MapData
    {
        public int[][] map;
    }
}