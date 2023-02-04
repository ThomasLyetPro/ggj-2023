using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AspectGgj2023.Gameboard 
{
    public class TileManager : MonoBehaviour
    {
        # region Internal references
        [SerializeField]
        private Tilemap mainTilemap;

        [SerializeField]
        private Tilemap previewTilemap;

        [SerializeField]
        private TileBase previewTile;

        [SerializeField]
        private List<TileBase> placeableTiles;
        # endregion
        
        private Vector3Int lastCellHovered;

        private int selectedTileIndex = 0;

        void Start()
        {
            Debug.Assert(mainTilemap != null);
            Debug.Assert(previewTilemap != null);
            Debug.Assert(previewTile != null);

            Debug.Assert(placeableTiles.Count == 2);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                selectedTileIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                selectedTileIndex = 1;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Vector3Int cellPosition = MouseToCellPosition();
                Debug.Log("Tile hovered: " + cellPosition);

                // Place and delete stuff on the main tilemap
                if (Input.GetMouseButtonDown(0))
                {
                    mainTilemap.SetTile(cellPosition, placeableTiles[selectedTileIndex]);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    mainTilemap.SetTile(cellPosition, null);
                }

                // Manage the preview
                else if (cellPosition != lastCellHovered)
                {
                    previewTilemap.DeleteCells(lastCellHovered, new Vector3Int(1,1,1));
                    previewTilemap.SetTile(cellPosition, previewTile);

                    lastCellHovered = cellPosition;
                }
            }
            else 
            {
                previewTilemap.ClearAllTiles();
            }
        }

        private Vector3Int MouseToCellPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Clamp it to the XY place because that's where the 2D screen is
            worldPosition.z = 0;

            return mainTilemap.WorldToCell(worldPosition);

        }
    }
}
