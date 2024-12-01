using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public interface ICommand
{
    public void Execute();
    public void Undo();
}
public class Backpack : MonoBehaviour, IPickable, ICommand
{
    public string test;
    [SerializeField] private InputAction input;
    [SerializeField] private GameObject backpackPanel;
    [SerializeField] private Inventory colorinventory;

    [NonSerialized] public Player player;
    public bool isFull => colorinventory.isFull;
    public IPickable.type Type { get => IPickable.type.Backpack; }
    public void OnAdd()
    {
        colorinventory.player = player;
        input.performed += x => ToggleBackpack();
        input.Enable();
        gameObject.SetActive(false);
    }
    public void Add(Recipe.ColorItems color) => colorinventory.Add(color);
    public void ToggleBackpack()
    {
        if(Invoker.UndoCommand() != (ICommand) this) Invoker.ExecuteCommand(this);
    }
    public void Execute() => backpackPanel.SetActive(false);
    public void Undo() => backpackPanel.SetActive(true);
}
