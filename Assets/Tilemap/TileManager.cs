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

        private Dictionary<Vector3Int, int> connectionIDs = new Dictionary<Vector3Int, int>();

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
        	
            DisplayConnectivityDebug();

            GetSelectedTileDebug(ref selectedTile);
            

            if (selectedTile)
            {
                Vector3Int cellPosition = MouseToCellPosition();

                // Display no preview and prevent placing if we're on a StaticTile space
                if (mainTilemap.GetTile(cellPosition) is StaticTile)
                {
                    previewTilemap.ClearAllTiles();
                    return;
                }

                // Place and delete stuff on the main tilemap
                if (Input.GetMouseButtonDown(0))
                {
                    mainTilemap.SetTile(cellPosition, selectedTile);
                    connectionIDs[cellPosition] = selectedTile.connectionId;
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

        private Vector3Int MouseToCellPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Clamp it to the XY place because that's where the 2D screen is
            worldPosition.z = 0;

            return mainTilemap.WorldToCell(worldPosition);
        }

        private Vector3Int getConnectionPos(Vector3Int position, int connectionCode) 
        {
            switch (connectionCode)
            {
                case 1: 
                    // connection BL
                    position.x -= 1;
                break;

                case 2: 
                    // connection BR
                    position.y -= 1;
                break;

                case 3: 
                    // connection TR
                    position.x += 1;
                break;

                case 4: 
                    // connection TL
                    position.y +=1 ;
                break;
            }
            return position;
        }


        private PlaceableTile goToNextTile(Vector3Int position, int connectionOrigin){

            List<Vector2Int> matrix = mainTilemap.GetTile<PlaceableTile>(position).getConnectMatrix();
            Vector2Int connectionPair = matrix.Find( connectionPair => connectionPair.x == connectionOrigin || connectionPair.y == connectionOrigin);
            int otherConnection = connectionPair.x == connectionOrigin ? connectionPair.y : connectionPair.x; 

            Vector3Int connectedPos = getConnectionPos(position, otherConnection);

            if(isConnectable(connectedPos, otherConnection)){
                return mainTilemap.GetTile<PlaceableTile>(position);
            } else {
                return null;
            }
        }


        private void handleConnection(Vector3Int originPos, List<Vector3Int> connectionsPos){
            PlaceableTile originTile = mainTilemap.GetTile<PlaceableTile>(originPos);
            Debug.Log("Origin tile instance ID: " + originTile.GetInstanceID());

            int newConnectionId = originTile.debugTree ? 1 : 0;
            Vector3Int neutralTilePos = new Vector3Int(0,0,0);
            PlaceableTile neutralTile = null;
            foreach (Vector3Int pos in connectionsPos)
            {
                PlaceableTile connectedTile = mainTilemap.GetTile<PlaceableTile>(pos);
                if(connectedTile && connectionIDs[pos] != 0){
                    if(newConnectionId != 0 ){
                        return;
                    } else {
                        newConnectionId = connectionIDs[pos];
                    }
                } else {
                    neutralTilePos = pos;
                    neutralTile = connectedTile;
                }
            }

            connectionIDs[originPos] = newConnectionId;

            if(newConnectionId != 0 && neutralTile && !originTile.debugTree){
                checkForPath(neutralTilePos, neutralTile);
            }
        }

        private bool isConnectable(Vector3Int position, int connection){
            PlaceableTile connectedTile = mainTilemap.GetTile<PlaceableTile>(position);
            if(connectedTile) {
                foreach(Vector2Int otherConnectionPair in connectedTile.getConnectMatrix()){
                    for(int j = 0; j < 2; j++){
                        int otherConnection = otherConnectionPair[j];
                        if(Mathf.Abs(otherConnection - connection) == 2){
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private void checkForPath(Vector3Int position, PlaceableTile tile ){
            List<Vector2Int> matrix = tile.getConnectMatrix();
            List<Vector3Int> connectedTilesPos = new List<Vector3Int>();
            foreach(Vector2Int connectionPair in matrix){
                for(int i = 0; i < 2; i++){
                    int connection = connectionPair[i];
                    Vector3Int connectedPos = getConnectionPos(position, connection);
                    if(isConnectable(connectedPos, connection)) {
                        connectedTilesPos.Add(connectedPos);
                    }
                }
            }

            handleConnection(position, connectedTilesPos);
        }
        
        public bool GameIsPaused()
        {
            return helpCanvas && helpCanvas.activeSelf;
        }

        /// <summary>
        /// Display debug lines for connectivity in the editor view
        /// </summary>
        private void DisplayConnectivityDebug()
        {
            BoundsInt tilemapBounds = mainTilemap.cellBounds;
           
            // Iterate through all the possible tiles in the tilemap
            foreach (var point in tilemapBounds.allPositionsWithin)
            {
                PlaceableTile tile = mainTilemap.GetTile(point) as PlaceableTile;

                // Not a placeable tile: we don't
                if (tile is null)
                {
                    continue;
                }

                Vector3 worldPosition = mainTilemap.CellToWorld(point);
                Color debugColor;

                switch (connectionIDs[point])
                {
                    case 0:
                        debugColor = Color.black;
                        break; 
                    case 1:
                        debugColor = Color.red;
                        break;
                    case 2:
                        debugColor = Color.blue;
                        break;
                    case 3:
                        debugColor = Color.green;
                        break;
                    default://case 4:
                        debugColor = Color.yellow;
                        break;
                }
                
                Debug.DrawLine(worldPosition, worldPosition + 0.8f*Vector3.up, debugColor, 1);
                Debug.DrawLine(worldPosition - 0.5f*Vector3.right + 0.5f*Vector3.up, worldPosition + 0.5f*Vector3.right + 0.5f*Vector3.up,debugColor, 1);
            }
        }

        /// <summary>
        /// Change the selected tile from the numeric keys of the keyboard.
        /// </summary>
        private void GetSelectedTileDebug(ref PlaceableTile selectedTile)
        {
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
        }
    }
}
