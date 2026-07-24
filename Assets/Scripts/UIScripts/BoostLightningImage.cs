using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostLightningImage : MonoBehaviour
{
    public static BoostLightningImage Instance { get; private set; }

    [SerializeField] private Image lightningBoostImage;

    private void Awake()
    {
        Instance = this;
    }

    public Image LightningBoostImage => lightningBoostImage;
}
