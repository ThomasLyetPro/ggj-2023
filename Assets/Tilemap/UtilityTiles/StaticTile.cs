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
    /// Tile supposed to be unmovable for the user in the game.
    /// </summary>
    public class StaticTile : Tile
    {
        #if UNITY_EDITOR
      // The following is a helper that adds a menu item to create a MyTile Asset
          [MenuItem("Assets/Create/CustomTiles/StaticTile")]
          public static void CreateMyTile()
          {
              string path = EditorUtility.SaveFilePanelInProject("Save My Tile", "New My Tile", "Asset", "Save My Tile", "Assets");
              if (path == "")
                  return;
              AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<StaticTile>(), path);    
          }
        #endif
    }
}