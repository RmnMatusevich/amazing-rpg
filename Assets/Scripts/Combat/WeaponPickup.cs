using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using System;
using RPG.Control;
using RPG.Attributes;

public class WeaponPickup : MonoBehaviour, IRaycastable
{
    [SerializeField] WeaponConfig weapon;
    [SerializeField] float healthToRestore = 0;
    [SerializeField] float respawnTime = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Pickup(other.gameObject);
        }
    }

    private void Pickup(GameObject subject)
    {
        if (weapon != null)
        {
            subject.GetComponent<Fighter>().EquipWeapon(weapon);
        }

        if (healthToRestore > 0)
        {
            subject.GetComponent<Health>().Heal(healthToRestore);
        }

        StartCoroutine(HideForSeconds(respawnTime));
    }

    private IEnumerator HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);
    }

    private void ShowPickup(bool shouldShow)
    {
        GetComponent<Collider>().enabled = shouldShow;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldShow);
        }
    }

    CursorType IRaycastable.GetCursorType()
    {
        return CursorType.Pickup;
    }

    bool IRaycastable.HandleRaycast(PlayerController callingController)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Pickup(callingController.gameObject);
        }

        return true;
    }
}
