using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponScript : MonoBehaviour
{
	[System.NonSerialized]
	public bool canFire;
	public int ammo = 100;
	public int maxAmmo = 100;
	public bool isInfiniteAmmo;
	public GameObject projectileGO;
	public Collider parentCollider;
	private Vector3 fireVector;
	[System.NonSerialized]
	public Transform myTransform;
	private int myLayer;
	public Vector3 spawnPosOffset;
	public float forwardOffset = 1.5f;
	public float reloadTime = 0.2f;
	public float projectileSpeed = 10f;
	public bool inheritVelocity;
	[System.NonSerialized]
	public Transform theProjectile;
	private GameObject theProjectileGO;
	private bool isLoaded;
	private ProjectileController theProjectileController;

    public object SpawnController { get; private set; }

    public virtual void Start()
	{
		Init();
	}
	public virtual void Init()
	{
		
		myTransform = transform;
		
		myLayer = gameObject.layer;
		
		Reloaded();
	}
	public virtual void Enable()
	{
		// drop out if firing is disabled
		if (canFire == true)
			return;
		// enable weapon (do things like show the weapons mesh etc.)
		canFire = true;
	}
	public virtual void Disable()
	{
		// drop out if firing is disabled
		if (canFire == false)
			return;
		// hide weapon (do things like hide the weapons mesh etc.)
		canFire = false;
	}
	public virtual void Reloaded()
	{
		
		isLoaded = true;
	}
	public virtual void SetCollider(Collider aCollider)
	{
		parentCollider = aCollider;
	}
	public virtual void Fire(Vector3 aDirection, int ownerID)
	{
		// be sure to check canFire so that the weapon can be // enabled or disabled as required!
		if (!canFire)
			return;
		// if the weapon is not loaded, drop out
		if (!isLoaded)
			return;
		// if we’re out of ammo and we do not have infinite ammo, // drop out...
		if (ammo <= 0 && !isInfiniteAmmo)
			return;
		
		ammo--;
		
		FireProjectile(aDirection, ownerID);
	
		isLoaded = false;
		
		CancelInvoke("Reloaded");
		Invoke("Reloaded", reloadTime);
	}
	public virtual void FireProjectile(Vector3 fireDirection, int ownerID)
	{
		
		theProjectile = MakeProjectile(ownerID);
	
		theProjectile.LookAt(theProjectile.position + fireDirection);

     	}
	public virtual Transform MakeProjectile(int ownerID)
	{

        theProjectile = SpawnController.Instance.Spawn(projectileGO, myTransform.position + spawnPosOffset + (myTransform.forward * forwardOffset), myTransform.rotation);
		theProjectileGO = theProjectile.gameObject;
		theProjectileGO.layer = myLayer;
		
		theProjectileController = theProjectileGO.GetComponent<ProjectileController>();
	
		Physics.IgnoreLayerCollision(myTransform.gameObject.layer, myLayer);
		
		return theProjectile;
	}

    private class ProjectileController
    {
    }
}
