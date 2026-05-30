using System.Collections;
using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    public GameObject ItemPrefab;

    public int spawnAmount;
    public float minXAxis;
    public float maxXAxis;
    public float minYAxis;
    public float maxYAxis;
    public Color guiColor;

    [SerializeField] private bool spawnOnDay;
    [SerializeField] private bool spawnOnNight;

    [SerializeField] private LayerMask riverLayer;

    void Awake()
    {
        GlobalEventManager.OnDayStart.AddListener(SpawnDayMushrooms);
        GlobalEventManager.OnNightStart.AddListener(SpawnNightMushrooms);
    }
    
    void Start()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            StartCoroutine(RespawnMushrooms());
        }
    }

    void SpawnDayMushrooms()
    {
        if (spawnOnDay)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                StartCoroutine(RespawnMushrooms());
            }
        }
    }

    void SpawnNightMushrooms()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            StartCoroutine(RespawnMushrooms());
        }
    }

    IEnumerator RespawnMushrooms()
    {
        yield return new WaitForSeconds(1);
        float randomPosX = Random.Range(minXAxis, maxXAxis);
        float randomPosY = Random.Range(minYAxis, maxYAxis);
        Vector3 spawnPos = new Vector3(randomPosX, randomPosY, 7);
        if (CanGrowOn(spawnPos))
        {
            Instantiate(ItemPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = guiColor;
        Vector2 pointA = new Vector2(minXAxis, minYAxis);
        Vector2 pointB = new Vector2(maxXAxis, maxYAxis);
        Vector2 pointC = new Vector2(minXAxis, maxYAxis);
        Vector2 pointD = new Vector2(maxXAxis, minYAxis);
        Gizmos.DrawLine(pointA, pointC);
        Gizmos.DrawLine(pointB,pointD);
        Gizmos.DrawLine(pointA, pointD);
        Gizmos.DrawLine(pointB, pointC);
    }

    private bool CanGrowOn(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.zero, Mathf.Infinity, riverLayer);
        return hit.collider == null;
    }
}