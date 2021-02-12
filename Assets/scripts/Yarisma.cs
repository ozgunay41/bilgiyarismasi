using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Yarisma : MonoBehaviour
{
    public PhotonView PV;
    public Text soru, cevapA, cevapB, cevapC, cevapD,zamanText;
    public Button[] buttons=new Button[4];
    public new GameObject gameObject;
    Sorular sr;
    public int soruSayisi=0,cevap,skor1=0,skor2=0;
    public float zaman;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.MasterClient.SetScore(0);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.PlayerList[1].SetScore(0);
        }

        PV = GetComponent<PhotonView>();
        sr = GetComponent<Sorular>();
        soruEkle();
    }
    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount==1)
        {
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
            SceneManager.LoadScene("LobbyScene");

        }
        if (soruSayisi -1 <= 2)
        {

            if (zaman >= 0)
            {
                zaman -= 1 * Time.deltaTime;
                zamanText.text = (zaman - 3f).ToString("0");
                if (zaman < 3)
                {
                    TurnGreen(buttons[sr.sorular[soruSayisi - 1].cevap - 1]);
                    zamanText.text = "0";
                    bekle();
                }
            }
            else
            {
                soruEkle();
            }
        }
        else
        {
            
            SceneManager.LoadScene("LobbyScene");
        }
            

    }


    public void soruEkle()
    {
        buttonVisible();
        TurnWhite(buttons);


        if (soruSayisi < sr.sorular.Count)
        {
            zaman = 13.0f;
            soru.text = sr.sorular[soruSayisi].soru;
            cevapA.text = sr.sorular[soruSayisi].cevapA;
            cevapB.text = sr.sorular[soruSayisi].cevapB;
            cevapC.text = sr.sorular[soruSayisi].cevapC;
            cevapD.text = sr.sorular[soruSayisi].cevapD;
            cevap = sr.sorular[soruSayisi].cevap;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.PlayerList[0].GetScore() > PhotonNetwork.PlayerList[1].GetScore())
                {
                    PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
                }
            }
            else if(!PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.PlayerList[0].GetScore() < PhotonNetwork.PlayerList[1].GetScore())
                {
                    PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
                }
            }

            Debug.Log("Oyun bitti");
            SceneManager.LoadScene("LobbyScene");
        }
            

        soruSayisi++;
    }

    public void cevapVer(int deger)
    {
        if (deger == cevap)
        {
            buttonInvisible();
            TurnGreen(buttons[deger - 1]);
            PlayerPrefs.SetInt("puan", PlayerPrefs.GetInt("puan") + 1);
            Debug.Log("doğru cevap");
            Debug.Log(PlayerPrefs.GetInt("puan"));
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("ben master clientim");
                PhotonNetwork.MasterClient.SetScore(PlayerPrefs.GetInt("puan"));
            }
            else if(!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("misafir oyuncuyum");
                PhotonNetwork.PlayerList[1].SetScore(PlayerPrefs.GetInt("puan"));
            }
              

        }
        else
        {
            buttonInvisible();
            TurnRed(buttons[deger - 1]);
            TurnGreen(buttons[cevap - 1]);
            Debug.Log("Yanlış Cevap");
        }
        bekle();
        
    }

    public void TurnRed(Button button)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.red;
            button.colors = colors;
        }

    public void TurnGreen(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.green;
        button.colors = colors;
    }

    public void TurnWhite(Button[] buttons)
    {
        for(int i=0;i<4;i++)
        {
            ColorBlock colors = buttons[i].colors;
            colors.normalColor = Color.white;
            colors.selectedColor = Color.blue;
            buttons[i].colors = colors;
        }
        
    }

    public void buttonVisible()
    {
        buttons[0].enabled = true;
        buttons[1].enabled = true;
        buttons[2].enabled = true;
        buttons[3].enabled = true;

    }
    public void buttonInvisible()
    {
        buttons[0].enabled = false;
        buttons[1].enabled = false;
        buttons[2].enabled = false;
        buttons[3].enabled = false;
    }

    IEnumerator bekle()
    {
        yield return new WaitForSeconds(3);
    }

    /*public void puanArttir()
    {
        PV.RPC("RPC_puanArttir", RpcTarget.All,null);
    }
    [PunRPC]
    public void RPC_puanArttir()
    {
        skor1++;
    }*/


}
