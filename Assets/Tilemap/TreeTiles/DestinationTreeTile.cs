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

        static private int salvagedSoul = 0;


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

        static public void ResetWinningCondition()
        {
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