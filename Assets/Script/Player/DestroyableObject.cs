using UnityEngine;

public class DestroyableObject : MonoBehaviour, IEntity
{
    public bool isAlive => false;

    public void OnDeath()
    {
    }

    public void OnReceiveDamage(float value, float InvisDuration = -1, Transform origin = null)
    {
    }
}
