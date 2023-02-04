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
        private PlaceableTile tileBLBR;

        [SerializeField]
        private PlaceableTile tileTLBL;   
        
        [SerializeField]
        private List<TileBase> placeableTiles;

        [SerializeField]
        private GameObject helpCanvas;

        [SerializeField]
        private PlaceableTile straightTiles;

        [SerializeField]
        private PlaceableTile tileTRBL;   
        [SerializeField]
        private PlaceableTile tileTRBR;   

        [SerializeField]
        private PlaceableTile tileTLBR;     

        [SerializeField]
        private PlaceableTile tileTRTL;   
        # endregion
        
        private Vector3Int lastCellHovered;

        private PlaceableTile selectedTile;

        void Start()
        {
            Debug.Assert(mainTilemap != null);
            Debug.Assert(previewTilemap != null);

            Debug.Assert(tileBLBR != null);
            Debug.Assert(tileTLBL != null);
            Debug.Assert(tileTLBR != null);
            Debug.Assert(tileTRBR != null);
            Debug.Assert(tileTRTL != null);
            Debug.Assert(tileTRBL != null);
            selectedTile = tileTLBR;

        }

        void Update()
        {
        	if (GameIsPaused()) return;
        
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedTile = tileBLBR;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectedTile = tileTLBL;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectedTile = tileTRBL;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selectedTile = tileTRBR;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                selectedTile = tileTLBR;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                selectedTile = tileTRTL;
            }

            if (selectedTile)
            {
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
                        previewTilemap.SetTile(cellPosition, selectedTile);

                        lastCellHovered = cellPosition;
                    }
                }
                else
                {
                    previewTilemap.ClearAllTiles();
                }
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
