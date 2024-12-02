using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Recipe recipe;
    [SerializeField] float maxCount;

    [NonSerialized] public Player player;
    //[SerializeField] List> ini list buat displaynya
    private List<Recipe.ColorCount> colors = new();
    private List<Recipe.ColorCount> dominantColor = new(); 
    float count = 0;
    public bool isFull => count >= maxCount;
    private void OnEnable()
    {
        //assign button combine ke ini
    }
    public bool Add(Recipe.ColorItems item)
    {
        if (isFull) return false;
        var recipe = colors.Find(x => x.color == item);
        if (recipe == null)
        {
            recipe = new()
            {
                color = item,
                count = 0
            };
            colors.Add(recipe);
        }
        recipe.count += 1;
        count += 1;
        return true;
    }
    public void Combine()
    {
        if (isFull)
        {
            var availableRecipe = recipe.recipes.Find(x => IsSame(x.colors, colors));
            if (availableRecipe != null)
            {
                player.SetColor(availableRecipe.result);
            }
            else if (colors.Count > 0)
            {
                foreach (var item in colors)
                {
                    if (dominantColor.Count == 0) dominantColor.Add(item);
                    else if (item.count > dominantColor[0].count)
                    {
                        dominantColor.Clear();
                        dominantColor.Add(item);
                    }
                    else if (item.count == dominantColor[0].count)
                    {
                        dominantColor.Add(item);
                    }
                }
                int rand = UnityEngine.Random.Range(0, dominantColor.Count);
                player.SetColor(dominantColor[rand].color);
            }
            dominantColor.Clear();
            colors.Clear();
            count = 0;
        }
    }
    public bool IsSame<T>(List<T> item1, List<T> item2) where T : Recipe.ColorCount
    {
        if (item1.Count != item2.Count) return false;
        int count = item1.Count;
        for (int i = 0; i < count; i++)
        {
            var ToCompare = item2.Find(x => x.color == item1[i].color);
            if (ToCompare == null || item1[i].count != ToCompare.count) return false;
        }
        return true;
    }
}
