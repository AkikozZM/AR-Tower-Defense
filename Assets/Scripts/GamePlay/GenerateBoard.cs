using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MyFirstARGame
{
    public class GenerateBoard : MonoBehaviour
    {
        public GameObject piece;
        public NetworkLauncher networkLauncher;
        public float gridSizeX = 4.0f;
        public float gridSizeY = 8.0f;

        private void Update()
        {
            //MouseClick();
            ScreenTouch();
        }

        /// <summary>
        /// Spawn a piece on the Placeable Grid
        /// </summary>
        private void SpawnPiece(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // check the tag && if the grid has piece already
                if (hit.collider.gameObject.CompareTag("PlaceableGrid"))
                {
                    GameObject curr = hit.collider.gameObject;
                    if (curr.GetComponent<PlaceableGrid_Script>().getHasPiece() == false)
                    {
                        curr.GetComponent<PlaceableGrid_Script>().setPiece();
                        Vector3 hitPos = hit.collider.gameObject.transform.position;
                        PhotonNetwork.Instantiate(piece.name, hitPos, Quaternion.identity);
                    }
                }
            }
        }
        private void ScreenTouch()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    SpawnPiece(ray);
                }
            }
        }
        private void MouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                SpawnPiece(ray);
            }
        }
        /// <summary>
        /// NetworkLauncher class will call this function, when server detects the first player joins the room
        /// </summary>
        public void GenerateGameBoard()
        {
            // when game starts, instantiate a gameboard
            GameObject board = PhotonNetwork.Instantiate("Board", new Vector3(0, 0, 0), Quaternion.Euler(0, 90, 0)) as GameObject;
        }
    }
}
