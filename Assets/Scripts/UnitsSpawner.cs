using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSpawner : MonoBehaviour
{
    public static UnitsSpawner instance { get; private set; }
    private void Awake() => instance = this;
    
    [SerializeField] private Vector3 _spawnPoint;
    [SerializeField] private Vector3 _minSpawnSpread;
    [SerializeField] private Vector3 _maxSpawnSpread;
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int _spawnAmount;
    [SerializeField] private float _spawnDelay;

    public List<Unit> spawnedUnits;

    private void Start()
    {
        spawnedUnits = new List<Unit>(512);
        StartCoroutine(Spawn(_spawnAmount));
    }
    
    private void Spawn()
    {
        var unit = Instantiate(_unitPrefab);
        var spreadX = Random.Range(_minSpawnSpread.x, _maxSpawnSpread.x);
        var spreadY = Random.Range(_minSpawnSpread.y, _maxSpawnSpread.y);
        var spreadZ = Random.Range(_minSpawnSpread.z, _maxSpawnSpread.z);
        unit.transform.position = _spawnPoint + new Vector3(spreadX, spreadY, spreadZ);
        spawnedUnits.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        spawnedUnits.Remove(unit);
    }

    private IEnumerator Spawn(int count)
    {
        while (true)
        {
            for (int i = 0; i < count; i++)
            {
                Spawn();
            }
            
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}