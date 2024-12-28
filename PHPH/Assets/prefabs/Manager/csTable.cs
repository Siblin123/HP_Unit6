using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class csTable : MonoBehaviour
{
    public static csTable Instance;
    public GameManager gameManager;
    public Inventory_Manager inventory_Manager;
    private void Awake()
    {
        Instance = this;
    }
}
