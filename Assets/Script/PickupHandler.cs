using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPickable
{
    public enum type
    {
        Backpack,
        Color
    }
    public type Type { get; }
    public void OnAdd();
}
public class PickupHandler : MonoBehaviour
{
    public List<Backpack> backpacks = new();
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IPickable>(out var pickable))
        {
            switch (pickable.Type)
            {
                case IPickable.type.Backpack:
                    backpacks.Add((Backpack)pickable);
                    break;
                case IPickable.type.Color:
                    if(backpacks.Count > 0)
                    {
                        var BackPack = backpacks.Find(x => !x.isFull);
                    }
                    break;
            }
            pickable.OnAdd();
        }
    }

#if UNITY_EDITOR
    public LayerMask pickableLayer;
    private Collider2D col;
    private void OnValidate()
    {
        col ??= GetComponent<Collider2D>();
        if (!col) return;
        col.isTrigger = true;
        col.includeLayers = pickableLayer;
        col.excludeLayers = ~pickableLayer;
        col.contactCaptureLayers = pickableLayer;
        col.callbackLayers = pickableLayer;
    }
    #endif
}
