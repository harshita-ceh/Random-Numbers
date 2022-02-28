using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasWeaponController : MonoBehaviour
{
	public GameObject[] weapons;
	public GameObject[] weapons2;
	public int selectedWeaponSlot;
	public int lastSelectedWeaponSlot;
	public Vector3 offsetWeaponSpawnPosition;
	public Transform forceParent;
	private ArrayList weaponSlots;
	private ArrayList weaponScripts;
	private BaseWeaponScript TEMPWeapon;
	private Vector3 TEMPvector3;
	private Quaternion TEMProtation;
	private GameObject TEMPgameObject;
	private Transform myTransform;
	private int ownerNum;
	public bool useForceVectorDirection;
	public Vector3 forceVector;
	private Vector3 theDir;
	public void Start()
	{

		selectedWeaponSlot = 0;
		lastSelectedWeaponSlot = -1;

		weaponSlots = new ArrayList();
		// initialize weapon scripts ArrayList
		weaponScripts = new ArrayList();
		// cache a reference to the transform (looking up a // transform each step can be expensive, so this is important!)
		myTransform = transform;
		if (forceParent == null)
		{
			forceParent = myTransform;
		}

		TEMPvector3 = forceParent.position;
		TEMProtation = forceParent.rotation;

		for (int i = 0; i < weapons.Length; i++)
		{
			// Instantiate the item from the weapons list
			TEMPgameObject = (GameObject)Instantiate(weapons[i], TEMPvector3 + offsetWeaponSpawnPosition, TEMProtation);

			TEMPgameObject.transform.parent = forceParent;
			TEMPgameObject.layer = forceParent.gameObject.layer;
			TEMPgameObject.transform.position = forceParent.position;
			TEMPgameObject.transform.rotation = forceParent.rotation;
			// store a reference to the gameObject in an ArrayList
			weaponSlots.Add(TEMPgameObject);

			TEMPWeapon = TEMPgameObject.GetComponent<BaseWeaponScript>();
			weaponScripts.Add(TEMPWeapon);

			TEMPgameObject.SetActive(false);
		}

		SetWeaponSlot(0);
	}
	public void SetOwner(int aNum)
	{

		ownerNum = aNum;
	}
	public virtual void SetWeaponSlot(int slotNum)
	{

		if (slotNum == lastSelectedWeaponSlot)
			return;
		// disable the current weapon
		DisableCurrentWeapon();
		// set our current weapon to the one passed in
		selectedWeaponSlot = slotNum;
		// make sure sensible values are getting passed in
		if (selectedWeaponSlot < 0)
			selectedWeaponSlot = weaponSlots.Count - 1;
		// make sure that the weapon slot isn’t higher than the // total number of weapons in our list
		if (selectedWeaponSlot > weaponSlots.Count - 1)
			selectedWeaponSlot = weaponSlots.Count - 1;
		// we store this selected slot to use to prevent duplicate // weapon slot setting
		lastSelectedWeaponSlot = selectedWeaponSlot;
		// enable the newly selected weapon
		EnableCurrentWeapon();
	}
	public virtual void NextWeaponSlot(bool shouldLoop)
	{
		// disable the current weapon
		DisableCurrentWeapon();
		// next slot
		selectedWeaponSlot++;
		// make sure that the slot isn’t higher than the total // number of weapons in our list
		if (selectedWeaponSlot == weaponScripts.Count)
		{
			if (shouldLoop)
			{
				selectedWeaponSlot = 0;
			}
			else
			{
				selectedWeaponSlot = weaponScripts.Count - 1;
			}
		}
		// we store this selected slot to use to prevent duplicate // weapon slot setting
		lastSelectedWeaponSlot = selectedWeaponSlot;
		// enable the newly selected weapon
		EnableCurrentWeapon();
	}
	public virtual void PrevWeaponSlot(bool shouldLoop)
	{
		// disable the current weapon
		DisableCurrentWeapon();
		// prev slot
		selectedWeaponSlot--;

		if (selectedWeaponSlot < 0)
		{
			if (shouldLoop)
			{
				selectedWeaponSlot = weaponScripts.Count - 1;
			}
			else
			{
				selectedWeaponSlot = 0;
			}
		}
		// we store this selected slot to use to prevent duplicate // weapon slot setting
		lastSelectedWeaponSlot = selectedWeaponSlot;

		EnableCurrentWeapon();
	}
	public virtual void DisableCurrentWeapon()
	{
		if (weaponScripts.Count == 0)
			return;

		TEMPWeapon = (BaseWeaponScript)weaponScripts[selectedWeaponSlot];

		TEMPWeapon.Disable();

		TEMPgameObject = (GameObject)weaponSlots[selectedWeaponSlot];
		TEMPgameObject.SetActive(false);
	}
	public virtual void EnableCurrentWeapon()
	{
		if (weaponScripts.Count == 0)
			return;

		TEMPWeapon = (BaseWeaponScript)weaponScripts[selectedWeaponSlot];

		TEMPWeapon.Enable();
		TEMPgameObject = (GameObject)weaponSlots[selectedWeaponSlot];
		TEMPgameObject.SetActive(true);
	}
	public virtual void Fire()
	{
		if (weaponScripts == null)
			return;
		if (weaponScripts.Count == 0)
			return;

		TEMPWeapon = (BaseWeaponScript)weaponScripts[selectedWeaponSlot];
		theDir = myTransform.forward;
		if (useForceVectorDirection)
			theDir = forceVector;

		TEMPWeapon.Fire(theDir, ownerNum);
	}
}
