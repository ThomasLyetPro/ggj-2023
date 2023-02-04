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
      [Header("connections pair are parsed that way :")]
      [Header("1 is bottom left")]
      [Header("2 is bottom right")]
      [Header("3 is top right")]
      [Header("4 is top left")]
      [Header("x:1 and y:2 connect bottom left to bottom right.")]
      [Header("1,2 and 2,1 are equivalents ")]
      private List<Vector2Int> connectMatrix;

      public int connectionId = 0;

      public bool debugTree = false;

      public List<Vector2Int> getConnectMatrix(){
        return this.connectMatrix;
      }

        #if UNITY_EDITOR
      // The following is a helper that adds a menu item to create a MyTile Asset
          [MenuItem("Assets/Create/PlaceableTile")]
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