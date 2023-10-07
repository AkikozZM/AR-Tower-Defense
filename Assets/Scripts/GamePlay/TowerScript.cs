using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MyFirstARGame
{
    public class TowerScript : MonoBehaviour
    {
        private NetworkCommunication netComm;

        public int controller = 0;
        public int playerIdx;
        private int attackDelay = 500;

        private int currentDelay;

        public int towerHealth = 10;
        public int towerDamage = 1;
        public int towerIncome = 5;
        public GameObject[] bulletPrefab;

        private bool firstMoment = true;
        public bool createDelay = false;

        public PlaceableGrid_Script placeableGrid = null;

        void Start()
        {
            netComm = FindObjectOfType<NetworkCommunication>();
        }

        void Update()
        {
            if (firstMoment && createDelay)
            {
                if (this.tag == "Income")
                {
                    if (controller == 1)
                    {
                        netComm.IncrementIncome(towerIncome, 0);
                    }
                    else if (controller == 2)
                    {
                        netComm.IncrementIncome(0, towerIncome);
                    }
                }

                firstMoment = false;
            }

            if (this.tag == "Attack" && currentDelay > attackDelay && createDelay)
            {
                if (controller == 1)
                {
                    bulletPrefab[0].GetComponent<BulletScript>().damage = towerDamage;
                    GameObject bull = PhotonNetwork.Instantiate(bulletPrefab[0].name, this.transform.position + new Vector3(0.0f, 0.12f, 0.0f), this.transform.rotation) as GameObject;
                    //bull.GetComponent<BulletScript>().damage = towerDamage;
                    //Debug.Log("Tower Damage: " + towerDamage);
                    currentDelay = 0;
                }
                else if (controller == 2)
                {
                    bulletPrefab[1].GetComponent<BulletScript>().damage = towerDamage;
                    GameObject bull = PhotonNetwork.Instantiate(bulletPrefab[1].name, this.transform.position + new Vector3(0.0f, 0.12f, 0.0f), this.transform.rotation) as GameObject;
                    //bull.GetComponent<BulletScript>().damage = towerDamage;
                    //Debug.Log("Tower Damage: " + towerDamage);
                    currentDelay = 0;
                }
                
            }

            currentDelay++;
        }

        public void DamageTower(int damage)
        {
            towerHealth -= damage;
            if (towerHealth <= 0 && placeableGrid != null)
            {
                placeableGrid.removePiece();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(towerHealth);
                stream.SendNext(towerDamage);
                stream.SendNext(towerIncome);
            }
            else
            {
                towerHealth = (int)stream.ReceiveNext();
                towerDamage = (int)stream.ReceiveNext();
                towerIncome = (int)stream.ReceiveNext();
            }
        }
    }
}
