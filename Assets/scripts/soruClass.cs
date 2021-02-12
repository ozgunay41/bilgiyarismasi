using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class soruClass
{
    public string soru, cevapA, cevapB, cevapC, cevapD;
    public int cevap;

    public soruClass(string soru,string cevapA,string cevapB,string cevapC,string cevapD,int cevap)
    {
        soru = this.soru;
        cevapA = this.cevapA;
        cevapB = this.cevapB;
        cevapC = this.cevapC;
        cevapD = this.cevapD;
        cevap = this.cevap; 
    }

}
