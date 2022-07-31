using UnityEngine;

public class RangeUnit : Unit
{
    private CustomArray<Unit> _tempUnits;
    
    protected override void Start()
    {
        if (SpatialGrid.instance.useSpatial)
        {
            _tempUnits = new CustomArray<Unit>(100);
        }
    }

    protected override void Update()
    {
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        transform.position += direction * (5.0f * Time.deltaTime);
        
        if (SpatialGrid.instance.useSpatial)
        {
            KillUnitsInRadiusWithSpatial();
        }
        else
        {
            KillUnitsInRadiusWithoutSpatial();
        }
    }

    private void KillUnitsInRadiusWithoutSpatial()
    {
        for (int i = 0; i < UnitsSpawner.instance.spawnedUnits.Count; i++)
        {
            var unit = UnitsSpawner.instance.spawnedUnits[i];
            
            var distance = Vector3.Distance(transform.position, unit.transform.position);

            if (distance <= radius)
            {
                unit.Destroy();
                i--;
            }
        }
    }
    
    private void KillUnitsInRadiusWithSpatial()
    {
        SpatialGrid.instance.GetUnitsFromBucketsInRadius(transform.position, radius, ref _tempUnits);
        for (int i = 0; i < _tempUnits.Count; i++)
        {
            if (_tempUnits.Get(i) == this)
                continue;
            
            _tempUnits.Get(i).Destroy();
        }
    }
}