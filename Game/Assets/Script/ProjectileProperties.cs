using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    // Base projectile stats used to determine perk modified stats
    private static float baseSpeed = 8.0f;
    private static float baseLifetime = 3.0f;
    private static int baseBounces = 0;
    private static float baseDamage = 5.0f;
    private static float baseScale = 0.4f;
    private static float baseExplosionScale = 2.0f;
    private static float baseExplosionDamage = 5.0f;
    private static int baseBurstNumber = 3;
    private static int baseSplits = 0;
    private static float baseExplosionKnockback = 2.0f;
    private static Color baseSpriteColor = Color.white;
    [SerializeField] float speedFactor = 1.5f;
    [SerializeField] float lifetimeMod = 1.0f;
    [SerializeField] float damageMod = 5.0f;
    [SerializeField] float explosionDamageMod = 5.0f;
    [SerializeField] float scaleMod = 1.25f;
    [SerializeField] float explosionScaleFactor = 1.2f;
    private static float baseHomingForce = 1.0f;
    private static float baseBoomerangForce = 2.0f;




    // Projectile stats 
    private int bounces = baseBounces;
    private float speed = baseSpeed;
    private float lifetime = baseLifetime;
    private float damage = baseDamage;
    private bool explosive = false;
    private float scale = baseScale;
    private float explosionScale = baseExplosionScale;
    private float explosionDamage = baseExplosionDamage;
    private float explosionKnockback = baseExplosionKnockback;
    private Color myColor = baseSpriteColor;
    private bool bursting = false;
    private int burstNumber = baseBurstNumber;
    private int splits = baseSplits;
    private bool homing = false;
    private float homingForce = baseHomingForce;
    private string homingTag = "";
    private float boomerangForce = baseBoomerangForce;
    private bool boomerang = false;

    // bonus attributes
    private float bonusScaleMod = 0.05f;
    private float bonusSpeedMod = 1.0f;
    private float bonusDamageMod = 2.0f;
    private float bonusDamage = 0.0f;
    private float bonusSpeed = 0.0f;
    private float bonusSize = 0.0f;
    private float bonusLifetime = 0.0f;
    private float bonusLifetimeMod = 0.8f;
    
    public void ApplyPerks(int[] perks)
    {
        // Reset bonus values first
        bonusDamage = 0.0f;
        bonusSize = 0.0f;
        bonusSpeed = 0.0f;

        for (int i = 0; i < perks.Length; i++)
        {
            switch (i)
            {
                case 0: // Bounce Perk: +1 bounce per bounce perk
                    this.bounces = baseBounces + perks[i];
                    break;

                case 1: // Speed up/down perks: +50% per speed perk
                    this.speed = baseSpeed * (Mathf.Pow(speedFactor, perks[i]));
                    if (perks[i] < 0) // Have speed down perks; apply damage bonus
                    {
                        bonusDamage = bonusDamage + Mathf.Abs(perks[i] * bonusDamageMod);
                    }
                    break;

                case 2: // Lifetime up/down perks
                    this.lifetime = baseLifetime + (perks[i] * lifetimeMod);
                    if (perks[i] < 0) // Have lifetime down perks; apply size bonus
                    {
                        bonusSize = bonusSize + (Mathf.Abs(perks[i] * bonusScaleMod));
                    }
                    break;

                case 3: // Damage up/down
                    this.damage = baseDamage + (perks[i] * damageMod);
                    this.myColor = new Color(1, Mathf.Max(0.0f, 1.0f - (perks[i] * 0.25f)), Mathf.Max(0.0f, 1.0f - (perks[i] * 0.25f)));
                    break;

                case 4: // Explosive
                    this.explosive = perks[i] > 0;
                    this.explosionScale = baseExplosionScale * (Mathf.Pow(explosionScaleFactor, perks[i]));
                    this.explosionDamage = baseExplosionDamage + (perks[i] * explosionDamageMod);
                    this.explosionKnockback = baseExplosionKnockback * perks[i];
                    break;

                case 5: // Size up/down
                    this.scale = baseScale * (Mathf.Pow(scaleMod, perks[i]));
                    if (perks[i] < 0) //Have size down perks, apply bonus speed
                    {
                        bonusSpeed = bonusSpeed + (Mathf.Abs(perks[i] * bonusSpeedMod));
                    }
                    break;

                case 6: // Burst shot
                    this.bursting = perks[i] > 0;
                    this.burstNumber = baseBurstNumber + (perks[i] - 1);
                    break;

                case 7: // Non-modify
                    break;

                case 8: // Splits Perk: +1 shot per split perk
                    this.splits = baseSplits + perks[i];
                    break;

                case 9: // Homing projectiles: Additional perks means stronger homing force
                    this.homingForce = baseHomingForce * perks[i];
                    this.homing = perks[i] > 0;
                    break;

                case 10: // Boomerang projectiles: Additional perks add boomerang force
                    this.boomerangForce = baseBoomerangForce * perks[i];
                    this.boomerang = perks[i] > 0;
                    this.bonusSpeed = bonusSpeed + (Mathf.Abs(perks[i] * bonusSpeedMod));
                    this.bonusLifetime = bonusLifetime + (Mathf.Abs(perks[i] * bonusLifetimeMod));
                    Debug.Log("Boomerang: " + boomerang);
                    break;

                default:
                    Debug.LogError("Applying undefined PerkID: " + i);

                    break;
            }
        }
        if (gameObject.CompareTag("Player"))
        {
            this.homingTag = "Enemy";
        } else if (gameObject.CompareTag("Enemy"))
        {
            this.homingTag = "Player";
        }
    }

    public float getLifetime() { return lifetime + bonusLifetime; }
    public float getDamage() { return damage + bonusDamage; }
    public float getSpeed() { return speed + bonusSpeed; }
    public int getBounces() { return bounces; }
    public bool isExplosive() {  return explosive; }
    public float getScale() {  return scale + bonusSize; }
    public float getExplosionScale() { return explosionScale; }
    public Color getSpriteColor() { return myColor; }
    public bool isBursting() {  return bursting; }
    public int getBurstNumber() {  return burstNumber; }
    public float getExplosionDamage() {  return explosionDamage; }
    public float getExplosionKnockback() { return explosionKnockback; }
    public int getSplits() { return splits; }
    public bool IsHoming() { return homing; }
    public float getHomingForce() {  return homingForce; }
    public string getHomingTag() { return homingTag;  }
    public void setHomingTag(string tag) { homingTag = tag; }
    public bool IsBoomerang() { return boomerang; }
    public float getBoomerangForce() { return boomerangForce; }
}
