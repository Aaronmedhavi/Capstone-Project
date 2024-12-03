using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorChangeTitle : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Color[] colors;
    public float changeInterval = 3f;

    private int currentColorIndex = 0;
    private int nextColorIndex = 1;
    private float timer = 0f;

    void Start()
    {
        if (titleText == null)
        {
            titleText = GetComponent<TextMeshProUGUI>();
        }

        if (colors == null || colors.Length < 2)
        {
            colors = new Color[] { Color.red, Color.green, Color.blue };
        }

        titleText.color = colors[currentColorIndex];
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / changeInterval;

        titleText.color = Color.Lerp(colors[currentColorIndex], colors[nextColorIndex], t);

        if (timer >= changeInterval)
        {
            timer = 0f;

            currentColorIndex = nextColorIndex;
            nextColorIndex = (nextColorIndex + 1) % colors.Length;
        }
    }
}
