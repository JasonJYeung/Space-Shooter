using UnityEngine;

public class PointInTime {

    public Quaternion rotation;
    public Vector3 position;

    public PointInTime(Vector3 _position, Quaternion _rotation) {
        rotation = _rotation;
        position = _position;
    }


}
