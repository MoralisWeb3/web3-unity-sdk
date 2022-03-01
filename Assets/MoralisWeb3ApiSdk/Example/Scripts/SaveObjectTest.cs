using MoralisWeb3ApiSdk;
using System;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_WEBGL
using Cysharp.Threading.Tasks;
using Moralis.WebGL.Platform.Objects;

#else
using System.Threading.Tasks;
using Moralis.Platform.Objects;
#endif


public class SaveObjectTest
{
#if UNITY_WEBGL
    public async static UniTask UpdateObject ()
#else
    public async static Task UpdateObject ()
#endif
    {
        Hero h1 = MoralisInterface.GetClient().Create<Hero>();
        h1.Name = GetTestName();

        System.Random rand = new System.Random((int)DateTime.Now.Ticks);
        h1.Strength = rand.Next(20) + 3;
        h1.Level = rand.Next(10) + 1;
        h1.Warcry = "Decaf is a lie!";

        await h1.SaveAsync();
    }

    private static string GetTestName()
    {
        string[] names = { "Clem the Great", "Sion the Bold", "Bob", "D@ve", "Oogmar the Deft", "Alesdair the Blessed", "Seviel the Mighty", "Master Adept Xactant", "Semaphore the Beautiful", "Gamemaster Nexnang" };

        System.Random rand = new System.Random((int)DateTime.Now.Ticks);

        int x = rand.Next(names.Length);

        return names[x];
    }
}

public class Hero : MoralisObject
{
    public Hero() : base("Hero")
    {
        Bag = new List<string>();
    }

    public int Strength { get; set; }
    public int Level { get; set; }
    public string Name { get; set; }
    public string Warcry { get; set; }
    public DateTime BirthDay { get; set; }
    public List<string> Bag { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}

