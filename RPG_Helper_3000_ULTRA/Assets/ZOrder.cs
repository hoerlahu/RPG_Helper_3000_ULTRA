using UnityEngine;

public class ZOrder
{
    public enum zOrder {
        GUI = -1
    }

    public static Vector3 SetZOrder(Vector3 location, zOrder zOrder) {
        location.z = (float)zOrder;
        return location;
    }
}