namespace MyFirstARGame
{
    using Photon.Pun;
    using UnityEngine;
    
    /// <summary>
    /// You can use this class to make RPC calls between the clients. It is already spawned on each client with networking capabilities.
    /// </summary>
    public class NetworkCommunication : MonoBehaviourPun
    {
        [SerializeField] private Scoreboard scoreboard;

        public void IncrementScore()
        {
            var playerName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(playerName);
            this.photonView.RPC("Network_SetPlayerScore", RpcTarget.All, playerName, currentScore + 1);
        }

        [PunRPC]
        public void Network_SetPlayerScore(string playerName, int newScore)
        {
            Debug.Log($"Player {playerName} scored!");
            this.scoreboard.SetScore(playerName, newScore);
        }

        public void IncrementHealth(int damage1, int damage2)
        {
            var health = this.scoreboard.GetHealth();

            int currentHealth1 = health[0];
            int currentHealth2 = health[1];

            currentHealth1 -= damage1;
            currentHealth2 -= damage2;

            this.photonView.RPC("Network_IncrementHealth", RpcTarget.All, currentHealth1, currentHealth2);
        }

        [PunRPC]
        public void Network_IncrementHealth(int health1, int health2)
        {
            this.scoreboard.SetHealth(health1, health2);
        }

        public void IncrementIncome(int increase1, int increase2)
        {
            var income = this.scoreboard.GetIncome();

            int currentIncome1 = income[0];
            int currentIncome2 = income[1];

            currentIncome1 += increase1;
            currentIncome2 += increase2;

            this.photonView.RPC("Network_IncrementIncome", RpcTarget.All, currentIncome1, currentIncome2);
        }

        [PunRPC]
        public void Network_IncrementIncome(int income1, int income2)
        {
            this.scoreboard.SetIncome(income1, income2);
        }

        public bool SpendMoney(int cost1, int cost2)
        {
            var money = this.scoreboard.GetMoney();

            int currentMoney1 = money[0];
            int currentMoney2 = money[1];

            currentMoney1 -= cost1;
            currentMoney2 -= cost2;

            if (currentMoney1 >= 0 && currentMoney2 >= 0)
            {
                this.photonView.RPC("Network_IncrementMoney", RpcTarget.All, currentMoney1, currentMoney2);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void IncrementMoney()
        {
            var money = this.scoreboard.GetMoney();

            int currentMoney1 = money[0];
            int currentMoney2 = money[1];

            var income = this.scoreboard.GetIncome();

            int currentIncome1 = income[0];
            int currentIncome2 = income[1];

            currentMoney1 += currentIncome1;
            currentMoney2 += currentIncome2;

            this.photonView.RPC("Network_IncrementMoney", RpcTarget.All, currentMoney1, currentMoney2);
        }

        [PunRPC]
        public void Network_IncrementMoney(int money1, int money2)
        {
            this.scoreboard.SetMoney(money1, money2);
        }

        public void ReadyMethod()
        {
            int act_num = PhotonNetwork.LocalPlayer.ActorNumber;

            this.photonView.RPC("Network_ReadyMethod", RpcTarget.All, act_num);
        }

        [PunRPC]
        public void Network_ReadyMethod(int player_num)
        {
            this.scoreboard.ReadyMethod(player_num);
        }
    }

}