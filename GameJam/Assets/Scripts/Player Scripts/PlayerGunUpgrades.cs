using UnityEngine;

// Example (call this from whatever handles boss defeat):
//   FindAnyObjectByType<PlayerGunUpgrades>().hasTripleShot = true;  // after Level 1 boss
//   FindAnyObjectByType<PlayerGunUpgrades>().hasIceShot = true;     // after Level 2 boss

public class PlayerGunUpgrades : MonoBehaviour
{
    [Header("Unlocked Upgrades")]
    public bool hasTripleShot = false;  // unlocked after defeating FireTank (Level 1 boss)
    public bool hasIceShot = false;     // unlocked after defeating FridgeEnemy (Level 2 boss)

    [Header("Triple Shot Settings")]
    [Tooltip("Angle in degrees between the center bullet and the top/bottom bullets")]
    public float spreadAngle = 12f;
}