using System;
using UnityEngine;
using UnityEngine.UIElements;
public class EnemyAlertHandler : MonoBehaviour {

    AlertSlider alertSlider;

    [SerializeField] GameObject alertPrefab;
    [SerializeField] float offset;
    [NonSerialized] public Transform Target;
    [NonSerialized] public Enemy enemy;
    [NonSerialized] public Enemy.ViewSettings viewSettings;

    private bool IsAlert;
    public bool isAlert
    {
        get => IsAlert;
        set
        {
            IsAlert = value;
            if (IsAlert && !alertSlider)
            {
                //alertSlider = AlertManager.instance.get();
                alertSlider = ObjectPoolManager.GetObject(alertPrefab, true, ObjectPoolManager.PooledInfo.Canvas).GetComponent<AlertSlider>();
                alertSlider.OnRelease += OnAlert;
                alertSlider.AlertValue = 0;
            }
        }
    }
    void OnAlert(AlertSlider slider)
    {
        alertSlider = null;
        slider.OnRelease -= OnAlert;
        if (slider.AlertValue >= 0)
        {
            var Walking = enemy.StateMachine.GetState(Enemy.State.Walk) as EnemyWalk;
            Walking.isChasing = true;
            Walking.target_location = Target;
            enemy.StateMachine.ChangeState(Enemy.State.Walk,0.2f);
        }
       ObjectPoolManager.ReleaseObject(slider.gameObject);
    }
    private void Update()
    {
        if (alertSlider)
        {
            alertSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset * Vector3.up);
            alertSlider.Alert(isAlert, viewSettings);
        }
    }
}
