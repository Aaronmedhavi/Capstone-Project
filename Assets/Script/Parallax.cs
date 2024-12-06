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

        // Horizontal Looping
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= spriteSize.x)
        {
            float offsetX = (cameraTransform.position.x > transform.position.x) ? spriteSize.x : -spriteSize.x;
            transform.position = new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z);
        }

        // Vertical Looping
        if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= spriteSize.y)
        {
            float offsetY = (cameraTransform.position.y > transform.position.y) ? spriteSize.y : -spriteSize.y;
            transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
        }

        previousCameraPosition = cameraTransform.position;
    }

    private void CreateClones()
    {
        Transform leftClone = Instantiate(
            this.transform,
            new Vector3(transform.position.x - spriteSize.x, transform.position.y, transform.position.z),
            transform.rotation,
            transform.parent
        );
        leftClone.name = this.name + "_Left";
        Parallax leftParallax = leftClone.GetComponent<Parallax>();
        if (leftParallax != null)
        {
            leftParallax.enabled = false;
        }

        Transform rightClone = Instantiate(
            this.transform,
            new Vector3(transform.position.x + spriteSize.x, transform.position.y, transform.position.z),
            transform.rotation,
            transform.parent
        );
        rightClone.name = this.name + "_Right";
        Parallax rightParallax = rightClone.GetComponent<Parallax>();
        if (rightParallax != null)
        {
            rightParallax.enabled = false;
        }

        Transform topClone = Instantiate(
            this.transform,
            new Vector3(transform.position.x, transform.position.y + spriteSize.y, transform.position.z),
            transform.rotation,
            transform.parent
        );
        topClone.name = this.name + "_Top";
        Parallax topParallax = topClone.GetComponent<Parallax>();
        if (topParallax != null)
        {
            topParallax.enabled = false;
        }

        Transform bottomClone = Instantiate(
            this.transform,
            new Vector3(transform.position.x, transform.position.y - spriteSize.y, transform.position.z),
            transform.rotation,
            transform.parent
        );
        bottomClone.name = this.name + "_Bottom";
        Parallax bottomParallax = bottomClone.GetComponent<Parallax>();
        if (bottomParallax != null)
        {
            bottomParallax.enabled = false;
        }
    }
}
