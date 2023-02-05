using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AspectGgj2023.Gameboard
{
    public class SoulManager : MonoBehaviour
    {


        # region Internal references
        [SerializeField]
        private TileManager tileManager;

        private Vector3Int tilePos;

        private int connectionOrigin = 0;

        # endregion

        // Start is called before the first frame update
        void Start()
        {
            tilePos = new Vector3Int(0,0,0);
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.transform.position = tileManager.mainTilemap.CellToWorld(tilePos) + new Vector3(0,0.5f,0);



            if(Input.GetKeyDown(KeyCode.LeftControl)){
                StartCoroutine(Travel());
            }
        }

        IEnumerator Travel(int count = 0){
            yield return new WaitForSeconds(0.5f);
            count ++;
            if (count > 100) yield break;

            Tile currentTile = tileManager.mainTilemap.GetTile<Tile>(tilePos);

            if (currentTile is OriginTreeTile ){

                List<Vector3Int> possiblePaths = new List<Vector3Int>();
                List<int> possibleConnections = new List<int>();
                // check all neibourg connected
                for (int i = 1; i <= 4; i ++)
                {
                    Vector3Int neighbourgPos = tileManager.GetConnectedPosition(tilePos, i);
                    if(tileManager.IsConnectable(neighbourgPos, connectionOrigin)) {
                        possibleConnections.Add(i);
                        possiblePaths.Add(neighbourgPos);
                    }
                }
                if(possiblePaths.Count == 0) yield break;
                int nextTileindex = Random.Range(0, possiblePaths.Count);

                tilePos = possiblePaths[nextTileindex];
                connectionOrigin = Mathf.Abs(possibleConnections[nextTileindex] - 2);
                StartCoroutine(Travel(count));
            }

            else if (currentTile is PlaceableTile){

                PlaceableTile currentPlaceableTile = currentTile as PlaceableTile;
                //TODO chose origin with path connected ?
                int nextConnections = currentPlaceableTile.getOpposedConnection(connectionOrigin);

                if(tileManager.goToNextTile(tilePos, connectionOrigin).HasValue){
                    Debug.Log("------------------------------------------------");
                    Debug.Log("Previous position " + tilePos);
                    tilePos = tileManager.goToNextTile(tilePos, connectionOrigin).Value;
                    Debug.Log("next pos :" + tilePos);
                    Debug.Log("I go towards connection :" + nextConnections);
                    Debug.Log("------------------------------------------------");

                    if(nextConnections > 2){
                        connectionOrigin = nextConnections - 2;
                    } else {
                        connectionOrigin = nextConnections + 2;
                    }
                    StartCoroutine(Travel(count));
                }
            }

        }
    }
}