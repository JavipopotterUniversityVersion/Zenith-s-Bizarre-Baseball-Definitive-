using UnityEngine;

public class Pointer : MonoBehaviour
{
    public void SetPointerRotation(Vector2 direction)
    {
        transform.rotation = Quaternion.Euler(0, 0, VectorRotEqualizer.VectorToRotation(direction));
    }

    public Vector2 GetPointerDirection()
    {
        return VectorRotEqualizer.RotationToVector(transform.rotation.eulerAngles.z);
    }
}
