using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject PrefabToBeSpawned;
    [SerializeField] private ClockManager clock; 
    
    private bool isNight = false;
    private bool canSpawn = true;

    [SerializeField] private int spawnAmount;
    public float minXAxis;
    public float maxXAxis;
    public float minYAxis;
    public float maxYAxis;
    [SerializeField] private LayerMask layersNotToSpawnOn;

    void Awake()
    {
        EventManager.OnNightStart.AddListener(NightStart);
        EventManager.OnDayStart.AddListener(DayStart);
    }

    void Update()
    {
        if (isNight)
        {
            if (clock.minutes >= 30 && canSpawn == true)
            {
                SpawnWolf();
                StartCoroutine(WolfSpawnCooldown());
            }
        }
    }

    private void NightStart()
    {
        isNight = true;
    }

    private void DayStart()
    {
        isNight = false;
    }

    private void SpawnWolf()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Debug.Log("Spawning Wolf");
            float randomPosX = Random.Range(minXAxis, maxXAxis);
            float randomPosY = Random.Range(minYAxis, maxYAxis);
            Vector3 spawnPos = new Vector3(randomPosX, randomPosY, 7);
            if (CanSpawnOn(spawnPos))
            {
                Instantiate(PrefabToBeSpawned, spawnPos, Quaternion.identity);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, Radius);
        Vector2 pointA = new Vector2(minXAxis, minYAxis);
        Vector2 pointB = new Vector2(maxXAxis, maxYAxis);
        Vector2 pointC = new Vector2(minXAxis, maxYAxis);
        Vector2 pointD = new Vector2(maxXAxis, minYAxis);
        Gizmos.DrawLine(pointA, pointC);
        Gizmos.DrawLine(pointB,pointD);
        Gizmos.DrawLine(pointA, pointD);
        Gizmos.DrawLine(pointB, pointC);
    }
    
    private bool CanSpawnOn(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.zero, Mathf.Infinity, layersNotToSpawnOn);
        return hit.collider == null;
    }

    IEnumerator WolfSpawnCooldown()
    {
        canSpawn = false;
        while (clock.minutes >= 30)
        {
            yield return null;
        }
        canSpawn = true;
    }
}
