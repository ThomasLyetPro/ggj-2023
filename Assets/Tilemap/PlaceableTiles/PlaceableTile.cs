using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AspectGgj2023.Gameboard 
{
    public class PlaceableTile : Tile
    {
      [SerializeField]
      [Header("Connections pair are parsed that way :")]
      [Header("1 means it connects to the bottom left tile")]
      [Header("2 means it connects to the bottom right tile")]
      [Header("3 means it connects to the top right tile")]
      [Header("4 means it connects to the top left tile")]
      [Header("x:1 and y:2 mean the tile bridges the bottom left to the bottom right.")]
      [Header("1,2 and 2,1 are equivalents ")]
      private List<Vector2Int> connectionMatrix;

      /// <summary>
      /// ID of the tree connected to the tile.
      /// </summary>
      public int originTreeId = 0;

      /// <summary>
      /// Return the matrix Nx2 of the possible connections available for the tiles.
      /// </summary>
      public List<Vector2Int> GetConnectionMatrix()
      {
        return this.connectionMatrix;
      }

        #if UNITY_EDITOR
      // The following is a helper that adds a menu item to create a MyTile Asset
          [MenuItem("Assets/Create/CustomTiles/PlaceableTile")]
          public static void CreateMyTile()
          {
              string path = EditorUtility.SaveFilePanelInProject("Save My Tile", "New My Tile", "Asset", "Save My Tile", "Assets");
              if (path == "")
                  return;
              AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PlaceableTile>(), path);    
          }
        #endif
    }
}