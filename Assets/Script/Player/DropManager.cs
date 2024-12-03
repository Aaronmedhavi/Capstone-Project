using UnityEngine;

public class DropManager : MonoBehaviour
{
    public static DropManager instance;
    [SerializeField] private GameObject SoulPrefab;

    private void Awake()
    {
        instance = this;
    }
    //KALO MAU UPDATE TAMBAH ENUM DROPTYPENYA JG
    public void DropRandom(float DropChance, Vector3 position)
    {
        int rand = Random.Range(0, 101);
        if (rand <= DropChance)
        {
            Recipe.ColorItems color = Recipe.GetRandomColor();
            GameObject soul = ObjectPoolManager.GetObject(SoulPrefab, false, ObjectPoolManager.PooledInfo.GameObject);
            soul.transform.position = position;
            soul.GetComponent<ColorDrop>().SetColor(color);
            soul.SetActive(true);
        }
    }
    public void DropColor(float DropChance,Recipe.ColorItems type, Vector3 position)
    {
        int rand = Random.Range(0, 101);
        if (rand <= DropChance)
        {
            GameObject soul = ObjectPoolManager.GetObject(SoulPrefab, false, ObjectPoolManager.PooledInfo.GameObject);
            soul.transform.position = position;
            soul.GetComponent<ColorDrop>().SetColor(type);
        }
    }
}
