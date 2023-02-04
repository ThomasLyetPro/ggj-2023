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

        [SerializeField]
        private GameObject helpCanvas;

        [SerializeField]
        private PlaceableTile straightTiles;

        [SerializeField]
        private TileBase cornerTopRight;   

        [SerializeField]
        private TileBase cornerTopLeft;     

        [SerializeField]
        private PlaceableTile cornerBottomRight;   

        [SerializeField]
        private TileBase cornerBottomLeft;        
        # endregion
        
        private Vector3Int lastCellHovered;

        private PlaceableTile selectedTile;

        void Start()
        {
            Debug.Assert(mainTilemap != null);
            Debug.Assert(previewTilemap != null);
            Debug.Assert(previewTile != null);

            Debug.Assert(straightTiles != null);
            Debug.Assert(cornerTopLeft != null);
            Debug.Assert(cornerTopRight != null);
            Debug.Assert(cornerBottomLeft != null);
            Debug.Assert(cornerBottomRight != null);
            selectedTile = straightTiles;

        }

        void Update()
        {
            if (GameIsPaused()) return;

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                SelectStraightTile();
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    selectedTile = straightTiles;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SelectCurveTile();
                    selectedTile = cornerBottomRight;
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    Vector3Int cellPosition = MouseToCellPosition();

                    // Place and delete stuff on the main tilemap
                    if (Input.GetMouseButtonDown(0))
                    {
                        mainTilemap.SetTile(cellPosition, selectedTile);
                        checkForPath(cellPosition, selectedTile);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        mainTilemap.SetTile(cellPosition, null);
                    }

                    // Manage the preview
                    else if (cellPosition != lastCellHovered)
                    {
                        previewTilemap.DeleteCells(lastCellHovered, new Vector3Int(1, 1, 1));
                        previewTilemap.SetTile(cellPosition, previewTile);

                        lastCellHovered = cellPosition;
                    }
                }
                else
                {
                    previewTilemap.ClearAllTiles();
                }
            }
        }

        public void SelectCurveTile()
        {
            selectedTileIndex = 1;
        }

        public void SelectStraightTile()
        {
            selectedTileIndex = 0;
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


        private void checkForPath(Vector3Int position, PlaceableTile tile ){
            List<Vector2Int> matrix = tile.getConnectMatrix();
            foreach(Vector2Int connectionPair in matrix){
                for(int i = 0; i < 2; i++){
                    int connection = connectionPair[i];

                    Vector3Int connectedPos = position;
                    switch (connection)
                    {
                        case 1: 
                            // connection BL
                            connectedPos.x -= 1;
                        break;

                        case 2: 
                            // connection BR
                            connectedPos.y -= 1;
                        break;

                        case 3: 
                            // connection TR
                            connectedPos.x += 1;
                        break;

                        case 4: 
                            // connection TL
                            connectedPos.y +=1 ;
                        break;
                    }
                    PlaceableTile connectedTile = mainTilemap.GetTile<PlaceableTile>(connectedPos);
                    if(connectedTile) {
                        foreach(Vector2Int otherConnectionPair in connectedTile.getConnectMatrix()){
                            for(int j = 0; j < 2; j++){
                                int otherConnection = otherConnectionPair[j];
                                if(Mathf.Abs(otherConnection - connection) == 2){
                                    Debug.Log("CONNECTED !!!");
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool GameIsPaused()
        {
            return helpCanvas && helpCanvas.activeSelf;
        }
    }
}
