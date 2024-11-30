using System.Collections;
using System.Collections.Generic;
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

    public bool isFull;
    public IPickable.type Type { get => IPickable.type.Backpack; }
    public void OnAdd()
    {
        input.performed += x => ToggleBackpack();
        input.Enable();
    }
    public void ToggleBackpack()
    {
        if(Invoker.UndoCommand() != (ICommand) this) Invoker.ExecuteCommand(this);
    }
    public void Execute() => backpackPanel.SetActive(false);
    public void Undo() => backpackPanel.SetActive(true);
}
public class Inventory : MonoBehaviour
{
    public Recipe recipe;
    private Dictionary<ColorItems, int> inventory;

}