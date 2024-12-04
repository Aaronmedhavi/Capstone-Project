using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private Vector3 spawn_area;
    [SerializeField] private LayerMask layerMask;

    [Header("Spawn Settings")]
    [SerializeField] private int min_spawnNumber;
    [SerializeField] private int max_spawnNumber;
    [SerializeField] private float spawn_distance_between;
    //2-4 musuh
    //5-8 musuh

    private List<Vector3> spawned_positions = new();
    private Collider2D[] grounds;
    private void Start()
    {
        grounds = Physics2D.OverlapBoxAll(transform.position, spawn_area, 0, layerMask);
        int count = grounds.Length;
        int spawn_number = Random.Range(min_spawnNumber, max_spawnNumber + 1);
        Vector3 spawn_position;
        for(int i = 0; i < spawn_number; i++)
        {
            bool isSpawnable = false;
            do
            {
                int random_ground = Random.Range(0, count);
                isSpawnable = GetRandomArea((BoxCollider2D)grounds[random_ground], out spawn_position);

            } while (isSpawnable == false);
            var npc = ObjectPoolManager.GetObject(EnemyPrefab, false, ObjectPoolManager.PooledInfo.GameObject);
            npc.transform.position = spawn_position;
            npc.GetComponent<Enemy>().SetColor(Recipe.GetRandomColor());
            npc.SetActive(true);
        }
    }
    bool GetRandomArea(BoxCollider2D ground, out Vector3 position)
    {
        Vector3 size = ground.size;
        size = new Vector3(size.x * ground.transform.lossyScale.x, size.y * ground.transform.lossyScale.y, 0);
        float y = ground.transform.position.y + size.y + 1.1f; //1.1 tinggi stickmannya maaf bruteforce
        float x = 0.5f * Random.Range(-size.x, size.x) + ground.transform.position.x;
        x = Mathf.Clamp(x, transform.position.x - 0.5f * spawn_area.x, transform.position.x + 0.5f * spawn_area.x);

        position = new Vector3(x, y, 0);
        Vector3 post = position;
        return spawned_positions.TrueForAll(x => Vector3.Distance(x, post) > spawn_distance_between);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawn_area);
    }
    #endif
}
