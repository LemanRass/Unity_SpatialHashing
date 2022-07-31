using UnityEngine;

public class Unit : MonoBehaviour
{
    public float radius;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        transform.position -= transform.forward * (3.0f * Time.deltaTime);

        float scale = 0.1f + Mathf.PingPong(Time.time, 2.0f);
        radius = scale / 2.0f;
        transform.localScale = new Vector3(scale, 0.1f, scale);
    }

    public void Destroy()
    {
        UnitsSpawner.instance.RemoveUnit(this);
        Destroy(gameObject);
    }
}