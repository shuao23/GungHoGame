using System;
using UnityEngine;

[Serializable]
public struct MoveId {
    [SerializeField]
    private int id;
    [SerializeField]
    private string name;

    public int Id {
        get { return id; }
    }

    public string Name {
        get { return name; }
    }

    public MoveId(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
