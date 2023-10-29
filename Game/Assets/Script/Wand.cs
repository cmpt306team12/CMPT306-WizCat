using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wand : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    
    private static readonly Vector3 AimingOffset = new Vector3(0, 0.475f, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireProjectile(Vector3 targetPosition, Vector3 startingPosition)
    {
        Vector3 offsetTargetPosition = targetPosition + AimingOffset;
        Vector3 firingDirection = (offsetTargetPosition - startingPosition).normalized;
        float firingAngle = Mathf.Atan2(firingDirection.y, firingDirection.x) * Mathf.Rad2Deg;
        float randomOffsetAngle = Random.Range(-5f, 5f);
        
        Projectile projectile = Instantiate(
            projectilePrefab,
            firingDirection + startingPosition,
            Quaternion.Euler(0,0,firingAngle + randomOffsetAngle));
        projectile.Fire();
    }
}
