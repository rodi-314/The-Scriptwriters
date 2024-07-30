using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int currentScene;
    public SerializableVector3 playerPosition;
    public Dictionary<string, int> inventory;
    public List<string> storyItems;
    public Dictionary<string, List<string>> conversations;
}

[Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static implicit operator SerializableVector3(Vector3 v)
    {
        return new SerializableVector3(v.x, v.y, v.z);
    }

    public static implicit operator Vector3(SerializableVector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
}
