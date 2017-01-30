using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragable {

    void OnDrag(Vector2 draggedDistanceScreen, Vector2 draggedDistanceWorld);

}
