using UnityEngine;
using UnityEngine.Pool;

public class AlertManager : MonoBehaviour
{
    [SerializeField] GameObject sliderPrefab;
    [SerializeField] GameObject canvas;
    public static AlertManager instance;
    ObjectPool<AlertSlider> pooledObject;
    private void Awake()
    {
        instance = this;
        pooledObject = new(createSlider, getSlider, returnSlider, destroySlider);
    }
    private AlertSlider createSlider()
    {
        GameObject instance = Instantiate(sliderPrefab, Vector3.zero, Quaternion.identity);
        instance.transform.SetParent(canvas.transform);
        AlertSlider alertSlider = instance.GetComponent<AlertSlider>();

        alertSlider.OnRelease = pooledObject.Release;
        return alertSlider;
    }
    private void getSlider(AlertSlider instance)
    {
        instance.gameObject.SetActive(true);
    }
    public void returnSlider(AlertSlider instance)
    {
        instance.gameObject.SetActive(false);
    }
    private void destroySlider(AlertSlider instance)
    {
        Destroy(instance);
    }

    public AlertSlider get() => pooledObject.Get();
}
