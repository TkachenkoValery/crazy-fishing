using UnityEngine;

public static class AxisExtension
{
    public static Vector3 ConvertToVector3(this Axis axis)
    {
        Vector3 result = Vector3.zero;
        result[(int)axis] = 1;
        return result;
    }
}