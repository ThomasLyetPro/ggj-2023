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
        private List<int> originTreesConnected = new List<int>();

        /// <summary>
        /// Connect an origin tree to the destination tree through a tile and return if the game is won or not.
        /// </summary>
        public bool ConnectOriginToDestination(int originTreeId) 
        {
            Debug.Assert(originTreeId > 0 && originTreeId <= OriginTreeTile.maxTreeId);

            // If the tree is already connected: useless (should not happen though)
            if (!originTreesConnected.Contains(originTreeId))
            {
                originTreesConnected.Add(originTreeId);
            }

            Debug.Log("ConnectOriginToDestination: " + originTreeId);
            Debug.Log("new count: " + originTreesConnected.Count);

            // The list can only contain one ID of each tree with no double: win condition is simply this
            return (originTreesConnected.Count == OriginTreeTile.maxTreeId);
        }

        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        {
            originTreesConnected.Clear();

            return true;
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