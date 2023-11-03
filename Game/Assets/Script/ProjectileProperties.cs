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
    private static int baseBurstNumber = 3;
    private static Color baseSpriteColor = Color.white;
    [SerializeField] float speedFactor = 1.5f;
    [SerializeField] float lifetimeMod = 0.5f;
    [SerializeField] float damageMod = 3.0f;
    [SerializeField] float scaleMod = 1.25f;
    [SerializeField] float explosionScaleFactor = 1.2f;


    // Projectile stats 
    private int bounces;
    private float speed;
    private float lifetime;
    private float damage;
    private bool explosive;
    private float scale;
    private float explosionScale;
    private Color myColor = baseSpriteColor;
    private bool bursting;
    private int burstNumber;

    private void Start()
    {
        // Set all stats to base values
        bounces = 0;
        speed = baseSpeed;
        lifetime = baseLifetime;
        damage = baseDamage;
        explosive = false;
        scale = baseScale;
        bursting = false;
        burstNumber = baseBurstNumber;
    }

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
                    Debug.Log(this.speed);
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
                    Debug.Log("Explosive: " + explosive + " " + perks[i]);
                    this.explosionScale = baseExplosionScale * (Mathf.Pow(explosionScaleFactor, perks[i]));
                    break;

                case 5: // Size up/down
                    this.scale = baseScale * (Mathf.Pow(scaleMod, perks[i]));
                    break;

                case 6: // Burst shot
                    this.bursting = perks[i] > 0;
                    this.burstNumber = baseBurstNumber + (perks[i] - 1);
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
}
