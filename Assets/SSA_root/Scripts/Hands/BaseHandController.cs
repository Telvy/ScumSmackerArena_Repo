using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponShooter;

public class BaseHandController : MonoBehaviour
{
    [SerializeField] protected WeaponInventoryBase weaponInventoryBase = null;
    

    public void Awake()
    {
        weaponInventoryBase = GetComponentInParent<WeaponInventoryBase>();
    }

    public virtual void OnCurrentItemEquipped()
    {

    }

    public virtual void OnCurrentItemDropped()
    {

    }

    public virtual void UseCurrentItem()
    {

    }

    public virtual void DropCurrentItem()
    {

    }
}
