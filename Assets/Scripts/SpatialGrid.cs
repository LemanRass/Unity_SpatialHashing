using UnityEngine;

public class SpatialGrid : MonoBehaviour
{
    public static SpatialGrid instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public Vector2Int gridSize;
    public Vector2 cellSize;

    public CustomArray<Unit>[,] units;
    public int unitsCellCapacity;
    public bool useSpatial;

    public void Init()
    {
        units = new CustomArray<Unit>[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                units[x, y] = new CustomArray<Unit>(unitsCellCapacity);
            }
        }
    }

    private void Update()
    {
        if (useSpatial)
        {
            ClearAllBuckets();
            FillAllBuckets();
        }
    }

    public void AddUnitToBuckets(Unit unit)
    {
        int minX = Mathf.FloorToInt(unit.transform.position.x - unit.radius);
        int maxX = Mathf.FloorToInt(unit.transform.position.x + unit.radius - 0.000001f);

        int minZ = Mathf.FloorToInt(unit.transform.position.z - unit.radius);
        int maxZ = Mathf.FloorToInt(unit.transform.position.z + unit.radius - 0.000001f);

        for (int z = maxZ; z >= minZ; z--)
        {
            for (int x = minX; x <= maxX; x++)
            {
                units[x, z].Add(unit);
            }
        }
    }

    private void FillAllBuckets()
    {
        for (int i = 0; i < UnitsSpawner.instance.spawnedUnits.Count; i++)
        {
            var unit = UnitsSpawner.instance.spawnedUnits[i];
            AddUnitToBuckets(unit);
        }
    }

    private void ClearAllBuckets()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                units[x, y].Clear();
            }
        }
    }

    public void GetUnitsFromBucketsInRadius(Vector3 position, float radius, ref CustomArray<Unit> result)
    {
        result.Clear();

        int minX = Mathf.FloorToInt(position.x - radius);
        int maxX = Mathf.FloorToInt(position.x + radius - 0.000001f);

        int minZ = Mathf.FloorToInt(position.z - radius);
        int maxZ = Mathf.FloorToInt(position.z + radius - 0.000001f);

        for (int z = maxZ; z >= minZ; z--)
        {
            for (int x = minX; x <= maxX; x++)
            {
                for (int i = 0; i < units[x, z].Count; i++)
                {
                    result.Add(units[x, z].Get(i));
                }
            }
        }
    }

    public void GetCellsInRadius(Vector3 position, float radius, ref CustomArray<Vector2Int> result)
    {
        result.Clear();

        int minX = Mathf.FloorToInt(position.x - radius);
        int maxX = Mathf.FloorToInt(position.x + radius - 0.000001f);

        int minZ = Mathf.FloorToInt(position.z - radius);
        int maxZ = Mathf.FloorToInt(position.z + radius - 0.000001f);

        for (int z = maxZ; z >= minZ; z--)
        {
            for (int x = minX; x <= maxX; x++)
            {
                for (int i = 0; i < units[x, z].Count; i++)
                {
                    result.Add(new Vector2Int(x, z));
                }
            }
        }
    }

    public void GetCellFromPosition(Vector3 position, out int x, out int y)
    {
        x = Mathf.FloorToInt(position.x / cellSize.x);
        y = Mathf.FloorToInt(position.z / cellSize.y);
    }

    public Vector3 GetPositionFromCell(int x, int y)
    {
        return new Vector3(x + cellSize.x / 2.0f, 0.0f, y + cellSize.y / 2.0f);
    }








    #region GIZMOS

    private void OnDrawGizmos()
    {
        DrawGrid();
        
        if (Application.isPlaying)
        {
            DrawBuckets();
        }
    }

    private void DrawBuckets()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (units[x, y].Count > 0)
                {
                    var position = GetPositionFromCell(x, y);
                    var size = new Vector3(cellSize.x, 0.01f, cellSize.y);
                    Gizmos.DrawCube(position, size);
                }
            }
        }
    }

    private void DrawGrid()
    {
        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int z = 0; z <= gridSize.y; z++)
            {
                var xStart = new Vector3(x * cellSize.x, 0.0f, z * cellSize.y);
                var xFinish = new Vector3(gridSize.x * cellSize.x, 0.0f, z * cellSize.y);
                Debug.DrawLine(xStart, xFinish);

                var zFinish = new Vector3(x * cellSize.x, 0.0f, gridSize.y * cellSize.y);
                Debug.DrawLine(xStart, zFinish);
            }
        }
    }

    #endregion
}