using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;

    [Header("Parallax Settings")]
    [SerializeField] private Vector2 parallaxFactor = new Vector2(0.1f, 0.1f);

    [Header("Loop Settings")]
    [SerializeField] private Vector2 spriteSize;

    [Header("Rotation Adjustments")]
    [SerializeField] private bool flipX = false;

    [SerializeField] private bool flipY = false;

    private Vector3 previousCameraPosition;
    private Transform[] clones; // Store references to the clones

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        previousCameraPosition = cameraTransform.position;

        CreateClones();
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        Vector3 move = new Vector3(
            deltaMovement.x * parallaxFactor.x * (flipX ? -1 : 1),
            deltaMovement.y * parallaxFactor.y * (flipY ? -1 : 1),
            0
        );
        transform.position += move;

        // Move clones along with the original
        foreach (Transform clone in clones)
        {
            clone.position += move;
        }

        // Horizontal Looping
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= spriteSize.x)
        {
            float offsetX = (cameraTransform.position.x > transform.position.x) ? spriteSize.x : -spriteSize.x;
            transform.position = new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z);

            // Update clone positions after looping
            UpdateClonePositions();
        }

        // Vertical Looping
        if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= spriteSize.y)
        {
            float offsetY = (cameraTransform.position.y > transform.position.y) ? spriteSize.y : -spriteSize.y;
            transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);

            // Update clone positions after looping
            UpdateClonePositions();
        }

        previousCameraPosition = cameraTransform.position;
    }

    private void CreateClones()
    {
        clones = new Transform[4]; // We have 4 clones: left, right, top, bottom

        clones[0] = Instantiate(
            this.transform,
            new Vector3(transform.position.x - spriteSize.x, transform.position.y, transform.position.z),
            transform.rotation,
            transform.parent
        );
        clones[0].name = this.name + "_Left";
        DisableParallaxComponent(clones[0]);

        clones[1] = Instantiate(
            this.transform,
            new Vector3(transform.position.x + spriteSize.x, transform.position.y, transform.position.z),
            transform.rotation,
            transform.parent
        );
        clones[1].name = this.name + "_Right";
        DisableParallaxComponent(clones[1]);

        clones[2] = Instantiate(
            this.transform,
            new Vector3(transform.position.x, transform.position.y + spriteSize.y, transform.position.z),
            transform.rotation,
            transform.parent
        );
        clones[2].name = this.name + "_Top";
        DisableParallaxComponent(clones[2]);

        clones[3] = Instantiate(
            this.transform,
            new Vector3(transform.position.x, transform.position.y - spriteSize.y, transform.position.z),
            transform.rotation,
            transform.parent
        );
        clones[3].name = this.name + "_Bottom";
        DisableParallaxComponent(clones[3]);
    }

    // Helper function to disable the Parallax component on a clone
    private void DisableParallaxComponent(Transform clone)
    {
        Parallax parallax = clone.GetComponent<Parallax>();
        if (parallax != null)
        {
            parallax.enabled = false;
        }
    }

    // Helper function to update the positions of the clones relative to the original
    private void UpdateClonePositions()
    {
        clones[0].position = new Vector3(transform.position.x - spriteSize.x, transform.position.y, transform.position.z); // Left
        clones[1].position = new Vector3(transform.position.x + spriteSize.x, transform.position.y, transform.position.z); // Right
        clones[2].position = new Vector3(transform.position.x, transform.position.y + spriteSize.y, transform.position.z); // Top
        clones[3].position = new Vector3(transform.position.x, transform.position.y - spriteSize.y, transform.position.z); // Bottom
    }
}
