using UnityEngine;

public class ColorDrop : MonoBehaviour, IPickable
{
    [SerializeField] Recipe.ColorItems Color;
    public Recipe.ColorItems color => Color;
    public IPickable.type Type => IPickable.type.Color;

    public void OnAdd()
    {
        Debug.Log("Collected");
        ObjectPoolManager.ReleaseObject(gameObject);
    }
}
