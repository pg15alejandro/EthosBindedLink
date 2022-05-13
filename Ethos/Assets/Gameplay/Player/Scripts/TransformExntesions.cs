//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;

public static class TransformExntesions
{
    public static Vector3 GetOffsetVector(this Transform tr, Vector3 offset, Vector3 scale, Vector3 mult)
    {
        var pos = tr.position + tr.TransformVector(offset);
        var v3 = pos + (scale.x * tr.right * mult.x);
        v3 += (scale.y * tr.up * mult.y);
        v3 += (scale.z * tr.forward * mult.z);
        return v3;
    }

    public static Vector3 Left(this Transform tr)
    {
        return -tr.right;
    }

    public static Vector3 Back(this Transform tr)
    {
        return -tr.forward;
    }
}