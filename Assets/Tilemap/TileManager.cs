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
        public Tilemap mainTilemap;

        [SerializeField]
        private Tilemap previewTilemap;

        [SerializeField]
        private GameManager gameManager;

        [Header("Available tiles")]
        [SerializeField]
        private PlaceableTile tileBLBR;

        [SerializeField]
        private PlaceableTile tileTLBL;   

        [SerializeField]
        private PlaceableTile tileTRBL;   
        [SerializeField]
        private PlaceableTile tileTRBR;   

        [SerializeField]
        private PlaceableTile tileTLBR;     

        [SerializeField]
        private PlaceableTile tileTRTL;   
        # endregion
        
        /// <summary>
        /// Coordinates of the last cell hovered in tilemap space.
        /// </summary>
        private Vector3Int lastCellHovered;

        /// <summary>
        /// Tile class currently selected.
        /// </summary>
        private PlaceableTile selectedTile;

        /// <summary>
        /// Table matching cell coordinates with the ID of its origin tree.
        /// </summary> 
        /// <remarks>
        /// We can't store it in the tiles custom scripts since we never really instance them (they're juste references)
        /// </remarks>
        private Dictionary<Vector3Int, int> originTreeIds = new Dictionary<Vector3Int, int>();

        void Start()
        {
            // Assert the SerializeFields to avoid finding null refs mid-through a test
            Debug.Assert(mainTilemap != null);
            Debug.Assert(previewTilemap != null);
            Debug.Assert(tileBLBR != null);
            Debug.Assert(tileTLBL != null);
            Debug.Assert(tileTLBR != null);
            Debug.Assert(tileTRBR != null);
            Debug.Assert(tileTRTL != null);
            Debug.Assert(tileTRBL != null);

            // Default selected tile
            selectedTile = tileTLBR;

            BoundsInt tilemapBounds = mainTilemap.cellBounds;

            // Iterate through all the possible tiles in the tilemap
            foreach (var point in tilemapBounds.allPositionsWithin)
            {
                
                OriginTreeTile tile = mainTilemap.GetTile(point) as OriginTreeTile;
                if (tile) 
                {
                    originTreeIds[point] = tile.GetOriginTreeId();
                }
            }
        }

        private void Update()
        {
        	if (GameIsPaused()) return;
        	
            // Draw the connectivity in the debug view
            DisplayConnectivityDebug();

            // Change the selected tile using the keyboard
            GetSelectedTileDebug(ref selectedTile);

            // No tile selected: nothing to place and stop there
            if (!selectedTile)
            {
<<<<<<< HEAD
                // Debug.Log("Nothing selected");
=======
>>>>>>> 782f684720a0c0d0a4609c8453883c618987e82e
                previewTilemap.ClearAllTiles();
                return; 
            }

            if(Input.GetKeyDown(KeyCode.Space)) Debug.Log(MouseToCellPosition());

            Vector3Int cellPosition = MouseToCellPosition();

            // Display no preview and prevent placing if we're not on a PlaceableTile space
            TileBase tile = mainTilemap.GetTile(cellPosition);
            if ((tile is StaticTile) || (tile is OriginTreeTile))
            {
                previewTilemap.ClearAllTiles();
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                mainTilemap.SetTile(cellPosition, selectedTile);

                // Set the default tree ID of the tile and try to connect it to its neighbours
                originTreeIds[cellPosition] = selectedTile.originTreeId;
                CheckForPath(cellPosition, selectedTile);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                // Deselect the tile if we don't want to place anything
                selectedTile = null;
            }

            // Manage the preview
            else if (cellPosition != lastCellHovered)
            {
                // Delete the previous preview tile set and create the new one
                previewTilemap.DeleteCells(lastCellHovered, new Vector3Int(1, 1, 1));
                previewTilemap.SetTile(cellPosition, selectedTile);

                // Remember the position
                lastCellHovered = cellPosition;
            }
        }

        /// <summary>
        /// Return the position of the cell currently hovered with the mouse.
        /// </summary>
        private Vector3Int MouseToCellPosition()
        {
            // Get the mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;

            // Convert it to world position on the plane of game
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0;

            // Return its value in tilemap space
            return mainTilemap.WorldToCell(worldPosition);
        }
        
        /// <summary>
        /// Check if the game is paused
        /// </summary>
        /// 
        public bool GameIsPaused()
        {
            return gameManager && gameManager.currentGamePhase != GameManager.GamePhase.Phase1;
        }

        # region Path connections management
        /// <summary>
        /// Return the next position for a given connection value.
        public Vector3Int GetConnectedPosition(Vector3Int position, int connectionCode) 
        {
            switch (connectionCode)
            {
                case 1: 
                    // Connects to the tile at the bottom left (BL)
                    position.x -= 1;
                break;

                case 2: 
                    // Connects to the tile at the bottom right (BR)
                    position.y -= 1;
                break;

                case 3: 
                    // Connects to the tile at the top right (TR)
                    position.x += 1;
                break;

                case 4: 
                    // Connects to the tile at the top left (TL)
                    position.y +=1 ;
                break;
            }
            return position;
        }

        // TODO: Dead code ?
        public Vector3Int? goToNextTile(Vector3Int position, int connectionOrigin){

            List<Vector2Int> matrix = mainTilemap.GetTile<PlaceableTile>(position).GetConnectionMatrix();
            Vector2Int connectionPair = matrix.Find( connectionPair => connectionPair.x == connectionOrigin || connectionPair.y == connectionOrigin);
            int otherConnection = connectionPair.x == connectionOrigin ? connectionPair.y : connectionPair.x; 

            Vector3Int connectedPos = GetConnectedPosition(position, otherConnection);

            if(IsConnectable(connectedPos, otherConnection)){
                return connectedPos;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Change the tree ID of the tile based on its neighbours.
        /// </summary>
        private void HandleConnection(Vector3Int originPos, List<Vector3Int> connectableNeighbours)
        {
            PlaceableTile originTile = mainTilemap.GetTile<PlaceableTile>(originPos);

            int newConnectionId = originTreeIds[originPos];

            Vector3Int neutralTilePos = new Vector3Int(0,0,0);
            PlaceableTile neutralTile = null;
            foreach (Vector3Int neighbourPosition in connectableNeighbours)
            {
                Tile genericTile = mainTilemap.GetTile<Tile>(neighbourPosition);

                // Check first if one of the neighbours is an origin tree tile
                OriginTreeTile originTreeTile = genericTile as OriginTreeTile;
                if (originTreeTile)
                {
                    // We can't connected to an origin tree if we already come from a tree
                    if(newConnectionId != 0 )
                    {
                        return;
                    } 
                    
                    newConnectionId = originTreeTile.GetOriginTreeId();
                    continue;
                }

                PlaceableTile connectedTile = genericTile as PlaceableTile;
                if(connectedTile && originTreeIds[neighbourPosition] != 0)
                {
                    if(newConnectionId != 0 )
                    {
                        return;
                    } 
                    
                    newConnectionId = originTreeIds[neighbourPosition];
                    
                } 
                else 
                {
                    neutralTilePos = neighbourPosition;
                    neutralTile = connectedTile;
                }
            }

            originTreeIds[originPos] = newConnectionId;

            // Check for possible connections to the destination tree now that we handled connections to an existing path to an origin
            
             foreach (Vector3Int neighbourPosition in connectableNeighbours)
            {
                DestinationTreeTile destinationTreeTile = mainTilemap.GetTile<DestinationTreeTile>(neighbourPosition);
                if (destinationTreeTile)
                {
                    // We don't have anything to check if we connect a free tile to the destination
                    if (newConnectionId == 0)
                    {
                        continue;
                    }

                    // If our tile is affiliated to an origin tree however, we signal it and check the winning condition
                    bool isTheGameWon = destinationTreeTile.ConnectOriginToDestination(newConnectionId);
                    if (isTheGameWon)
                    {
                        gameManager.TriggerVictory();
                    }
                }
            }

            // Reattach a potential dangling path
            if(newConnectionId != 0 && neutralTile){
                CheckForPath(neutralTilePos, neutralTile);
            }
        }

        /// <summary>
        /// Checks if the tile at a given position can be connected to a neighbour with the given connection code in its <see cref="PlaceableTile.connectionMatrix"/>.
        /// </summary>
        public bool IsConnectable(Vector3Int position, int connectionCode)
        {
            // If a tile from a tree, we can always connect
            Tile genericTile = mainTilemap.GetTile<Tile>(position);
            if (genericTile is OriginTreeTile || genericTile is DestinationTreeTile)
            {
                return true;
            }

            // Checks if the neighbour is the type that can have connections
            PlaceableTile connectedTile = genericTile as PlaceableTile;
            if (!connectedTile) 
            {
                return false;
            }

            foreach(Vector2Int connectionPair in connectedTile.GetConnectionMatrix())
            {
                for(int j = 0; j < 2; j++)
                {
                    int otherConnection = connectionPair[j];

                    // Reminder: (bottom-left, bottom-right, top-right, top-left) => (1, 2, 3, 4)
                    // Since bottom-left can only connect to top-right, one minus the other makes always 2 or -2
                    // Same for aany other combination
                    if (Mathf.Abs(otherConnection - connectionCode) == 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the tile at a given position can be connected to an existing path.
        /// </summary>
        private void CheckForPath(Vector3Int position, PlaceableTile tile)
        {
            List<Vector3Int> connectedTilesPos = new List<Vector3Int>();
            foreach(Vector2Int connectionPair in tile.GetConnectionMatrix())
            {
                // Test each end of the pair
                for (int i = 0; i < 2; i++)
                {
                    int connectionCode = connectionPair[i];
                    Vector3Int neighbourPosition = GetConnectedPosition(position, connectionCode);

                    // Add it to the list if it can be connected to the tile we're processing
                    if (IsConnectable(neighbourPosition, connectionCode)) 
                    {
                        connectedTilesPos.Add(neighbourPosition);
                    }
                }
            }

            HandleConnection(position, connectedTilesPos);
        }
        # endregion

        # region Debug tools
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

                // Match each connection to a tree with a color
                switch (originTreeIds[point])
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
                    case 4:
                        debugColor = Color.yellow;
                        break;
                    default:
                        Debug.Log("A tile shouldn't have this ID");
                        debugColor = Color.magenta;
                        break;
                }
                
                // Display a little colored cross on top of the tile in the editor
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

        public void OnStraightTileButtonClick()
        {
            if (selectedTile == tileTRBL)
                selectedTile = tileTLBR;
            else
                selectedTile = tileTRBL;
        }

        public void OnCurveTileButtonClick()
        {
            if (selectedTile == tileBLBR)
                selectedTile = tileTRBR;
            else if (selectedTile == tileTRBR)
                selectedTile = tileTRTL;
            else if (selectedTile == tileTRTL)
                selectedTile = tileTLBL;
            else
                selectedTile = tileBLBR;
        }

        # endregion
    }
}
