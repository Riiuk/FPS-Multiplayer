using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personajes : MonoBehaviour
{
    public GameObject descripciones;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject player5;
    public GameObject player6;
    public GameObject player7;
    public GameObject player8;

    void Start()
    {
        descripciones.SetActive(true);
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);
        player6.SetActive(false);
        player7.SetActive(false);
        player8.SetActive(false);
    }

    public void Player01()
    {
        descripciones.SetActive(false);
        player1.SetActive(true);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);
        player6.SetActive(false);
        player7.SetActive(false);
        player8.SetActive(false);
    }

    public void Player2()
    {
        descripciones.SetActive(false);
        player1.SetActive(false);
        player2.SetActive(true);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);
        player6.SetActive(false);
        player7.SetActive(false);
        player8.SetActive(false);
    }

    public void Player3()
    {
        descripciones.SetActive(false);
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(true);
        player4.SetActive(false);
        player5.SetActive(false);
        player6.SetActive(false);
        player7.SetActive(false);
        player8.SetActive(false);
    }

    public void Player4()
    {
        descripciones.SetActive(false);
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(true);
        player5.SetActive(false);
        player6.SetActive(false);
        player7.SetActive(false);
        player8.SetActive(false);
    }

    public void Player5()
    {
        descripciones.SetActive(false);
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(true);
        player6.SetActive(false);
        player7.SetActive(false);
        player8.SetActive(false);
    }

    public void Player6()
    {
        descripciones.SetActive(false);
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);
        player6.SetActive(true);
        player7.SetActive(false);
        player8.SetActive(false);
    }

    public void Player7()
    {
        descripciones.SetActive(false);
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);
        player6.SetActive(false);
        player7.SetActive(true);
        player8.SetActive(false);
    }

    public void Player8()
    {
        descripciones.SetActive(false);
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);
        player6.SetActive(false);
        player7.SetActive(false);
        player8.SetActive(true);
    }
}
