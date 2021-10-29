using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Interface for grabbable functionality in Scum Smacker Arena. 
//...
//In Scum Smacker Arena any item in the gameworld that "can" be picked up can be used in different ways. 
//These items can be used, dropped, or tossed, depending on the type of item they are. Some items
//may not have inherent uses, but the Use() function may do something miscelleneous like say
//Play a sound or animation. If the item is say, a weapon, then the item's "weapon functionality" 
//will be used be used. If the item does something unique, then that unique function will be triggered, etc.
//...
public interface IGrabbableItem
{ 
    void GrabItem();

    void UseItem();

    void DropItem();

    void ThrowItem();
}
