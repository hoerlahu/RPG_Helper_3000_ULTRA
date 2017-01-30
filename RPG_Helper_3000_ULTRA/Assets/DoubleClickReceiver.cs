using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityScript;

public class DoubleClickReceiver : MonoBehaviour
{
    private const float co_max_time_double_click = 0.5f;
    private const float co_max_dist_double_click = 0.05f;

    private Vector3 lastClick;
    private float timeOfClick;

    private List<IDoubleClickReceiver> listeners = new List<IDoubleClickReceiver>();

    public void AddListener(IDoubleClickReceiver listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(IDoubleClickReceiver listener)
    {
        listeners.Remove(listener);
    }

    void OnMouseDown()
    {
        if (WasDoubleClick(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
            foreach (IDoubleClickReceiver listener in listeners) {
                listener.OnDoubleClick();
            }
        }
    }

    protected bool WasDoubleClick(Vector3 click)
    {

        bool wasDoubleClick = false;

        if (lastClick != Vector3.zero)
        {
            wasDoubleClick = (
                    Math.Pow((click.x - lastClick.x), 2) 
                    + 
                    Math.Pow((click.x - lastClick.x), 2)) 
                    < 
                    Math.Pow(co_max_dist_double_click, 2) 
                    && 
                    (timeOfClick - Time.time <= co_max_time_double_click);
        }

        lastClick = click;
        timeOfClick = Time.time;

        return wasDoubleClick;
    }

}

