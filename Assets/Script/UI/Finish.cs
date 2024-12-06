using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTrigger : MonoBehaviour
{
    // Name of the menu scene you want to load
    [SerializeField] private string menuSceneName = "MenuScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Load the menu scene
            SceneManager.LoadScene(menuSceneName);
        }
    }
}
