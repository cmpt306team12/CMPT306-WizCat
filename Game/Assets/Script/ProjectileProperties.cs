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
    [SerializeField] float lifetimeMod = 0.5f;
    [SerializeField] float damageMod = 3.0f;
    [SerializeField] float explosionDamageMod = 5.0f;
    [SerializeField] float scaleMod = 1.25f;
    [SerializeField] float explosionScaleFactor = 1.2f;



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
    
    public void ApplyPerks(int[] perks)
    {
        for (int i = 0; i < perks.Length; i++)
        {
            switch (i)
            {
                case 0: // Bounce Perk: +1 bounce per bounce perk
                    this.bounces = baseBounces + perks[i];
                    break;

                case 1: // Speed up/down perks: +50% per speed perk
                    this.speed = baseSpeed * (Mathf.Pow(speedFactor, perks[i]));
                    break;

                case 2: // Lifetime up/down perks
                    this.lifetime = baseLifetime + (perks[i] * lifetimeMod);
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

                default:
                    Debug.LogError("Applying undefined PerkID: " + i);

                    break;
            }
        }
    }

    public float getLifetime() { return lifetime; }
    public float getDamage() { return damage; }
    public float getSpeed() { return speed; }
    public int getBounces() { return bounces; }
    public bool isExplosive() {  return explosive; }
    public float getScale() {  return scale; }
    public float getExplosionScale() { return explosionScale; }
    public Color getSpriteColor() { return myColor; }
    public bool isBursting() {  return bursting; }
    public int getBurstNumber() {  return burstNumber; }
    public float getExplosionDamage() {  return explosionDamage; }
    public float getExplosionKnockback() { return explosionKnockback; }
    public int getSplits() { return splits; }
}
