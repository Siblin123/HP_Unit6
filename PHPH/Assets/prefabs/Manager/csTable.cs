using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class csTable : MonoBehaviour
{
    public static csTable Instance;
    public GameManager gameManager;

    private void Awake()
    {
        Instance = this;
    }
}
