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
    public class OriginTreeTile : Tile
    {
        /// <summary>
        /// Id of the origin tree the tile represent
        /// </summary>
        [SerializeField]
        private int originTreeId = 1;

        public int GetOriginTreeId()
        {
            return originTreeId;
        }

        #if UNITY_EDITOR
      // The following is a helper that adds a menu item to create a MyTile Asset
          [MenuItem("Assets/Create/CustomTiles/OriginTreeTile")]
          public static void CreateMyTile()
          {
              string path = EditorUtility.SaveFilePanelInProject("Save My Tile", "New My Tile", "Asset", "Save My Tile", "Assets");
              if (path == "")
                  return;
              AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<OriginTreeTile>(), path);    
          }
        #endif
    }
}