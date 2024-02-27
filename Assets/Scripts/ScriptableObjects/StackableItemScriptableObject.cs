using System.Collections.Generic;
using UnityEngine;

/**
 * Responsible for mapping a pool ID to display properties shown on player death canvas
 */
[CreateAssetMenu(fileName = "StackableItems", menuName = "StackableItemsScriptableObject", order = 2)]
public class StackableItemScriptableObject : ScriptableObject
{
    // Represents display properties for each poolID of a stackable item
    [System.Serializable]
    class StackableItemGroup
    {
        public PoolId stackableItemPoolId;
        public string stackableItemName;
        public Sprite stackableItemImage;
    }

    [SerializeField] private List<StackableItemGroup> _stackableItemGroup = new List<StackableItemGroup>();

    public Sprite getImageFromId(PoolId poolId) {
        StackableItemGroup stackableItemGroup = _stackableItemGroup.Find((StackableItemGroup stackableItemGroup) => {
            return stackableItemGroup.stackableItemPoolId == poolId;
        });
        if (stackableItemGroup == null) {
            Debug.LogWarning("Stackable item of name: " + poolId + " is not present in stackable item groups");
            return null;
        }
        return stackableItemGroup.stackableItemImage;
    }

    public string getNameFromId(PoolId poolId) {
        StackableItemGroup stackableItemGroup = _stackableItemGroup.Find((StackableItemGroup stackableItemGroup) => {
            return stackableItemGroup.stackableItemPoolId == poolId;
        });
        if (stackableItemGroup == null) {
            Debug.LogWarning("Stackable item of name: " + poolId + " is not present in stackable item groups");
            return "";
        }
        return stackableItemGroup.stackableItemName;
    }
}
