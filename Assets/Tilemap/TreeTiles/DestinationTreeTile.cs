using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AspectGgj2023.Gameboard 
{
    /// <summary>
    /// Tile that is a source of souls, propagating its ids to the path of tiles connected.
    /// </summary>
    public class DestinationTreeTile : Tile
    {
        private const int winningCondition = 4;

        /// <summary>
        /// How many souls have reached a destination tile.
        /// </summary>
        static private int salvagedSoul = 0;

        /// <summary>
        /// Get filled with the ID of origin trees when a tile having the ID is connected to a destination tile => there is a path between them.
        /// </summary>
        static private List<int> originTreesConnected = new List<int>();

        /// <summary>
        /// Connect an origin tree to the destination tree through a tile and return if the game is won or not.
        /// </summary>
        public bool ConnectOriginToDestination(int originTreeId) 
        {
            Debug.Assert(originTreeId > 0 && originTreeId <= OriginTreeTile.maxTreeId);

            // If the tree is already connected: useless (should not happen though)
            if (!originTreesConnected.Contains(originTreeId))
            {
                Debug.Log("Add");
                originTreesConnected.Add(originTreeId);
            }
            else 
            {
                Debug.Log("Not add");
            }

            Debug.Log("ConnectOriginToDestination: " + originTreeId);
            Debug.Log("new count: " + originTreesConnected.Count);

            // The list can only contain one ID of each tree with no double: win condition is simply this
            return (originTreesConnected.Count == OriginTreeTile.maxTreeId);
        }

        /// <summary>
        /// Trigger by a soul reaching the tile.
        /// </summary>
        public bool AddSalvagedSoul()
        {
            salvagedSoul += 1;

            return (salvagedSoul >= winningCondition);
        }

        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        {
            salvagedSoul = 0;

            return true;
        }

        /// <summary>
        /// Reset the winning animation and screen conditions
        /// </summary>
        static public void ResetWinningCondition()
        {
            Debug.Log("Reset");
            originTreesConnected.Clear();
            salvagedSoul = 0;
        }

        #if UNITY_EDITOR
      // The following is a helper that adds a menu item to create a MyTile Asset
          [MenuItem("Assets/Create/CustomTiles/DestinationTreeTile")]
          public static void CreateMyTile()
          {
              string path = EditorUtility.SaveFilePanelInProject("Save My Tile", "New My Tile", "Asset", "Save My Tile", "Assets");
              if (path == "")
                  return;
              AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DestinationTreeTile>(), path);    
          }
        #endif
    }
}