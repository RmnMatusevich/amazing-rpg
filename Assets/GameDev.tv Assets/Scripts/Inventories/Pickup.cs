using System;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// To be placed at the root of a Pickup prefab. Contains the data about the
    /// pickup such as the type of item and the number.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        // STATE
        InventoryItem item;
        int number;

        // CACHED REFERENCE
        Inventory inventory;

        // LIFECYCLE METHODS

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.GetComponent<Inventory>();
        }

        // PUBLIC

        /// <summary>
        /// Set the vital data after creating the prefab.
        /// </summary>
        /// <param name="item">The type of item this prefab represents.</param>
        public void Setup(InventoryItem item, int number)
        {
            this.item = item;
            this.number = number;
        }

        public InventoryItem GetItem()
        {
            return item;
        }

        public int GetNumber()
        {
            return number;
        }

        public void PickupItem()
        {
            bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            if (item == null) return false;

            return inventory.HasSpaceFor(item);
        }
    }
}