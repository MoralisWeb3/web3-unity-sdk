using Moralis.Platform.Objects;
using MoralisWeb3ApiSdk;
using System;
using UnityEngine;
#if UNITY_WEBGL
using Cysharp.Threading.Tasks;
#else    
using System.Threading.Tasks;
#endif

public class UpdateUserTest
{
#if UNITY_WEBGL
    public async static UniTask UpdateUserName (MoralisUser user)
#else    
    public async static Task UpdateUserName (MoralisUser user)
#endif
    {
        string newName = GetTestName(user.username);

        Debug.Log($"Updating user with session: {user.sessionToken} from {user.username} to {newName}");

        try
        {
            user.username = newName;

            await user.SaveAsync();

            Debug.Log("Usder saved. Re-fetching ...");

            MoralisUser updatedUser = await MoralisInterface.GetClient().UserFromSession(user.sessionToken);

            Debug.Log($"User record now has name: {updatedUser.username}");
        }
        catch (Exception exp)
        {
            Debug.LogError($"User update failed: {exp.Message}");
        }
    }

    private static string GetTestName(string currentName)
    {
        string[] names = { "Clem the Great", "Sion the Bold", "Bob", "D@ve", "Oogmar the Deft", "Alesdair the Blessed", "Seviel the Mighty", "Master Adept Xactant", "Semaphore the Beautiful", "Gamemaster Nexnang" };

        System.Random rand = new System.Random((int)DateTime.Now.Ticks);

        int x = rand.Next(names.Length);

        while (names[x].Equals(currentName))
        {
            x = rand.Next(names.Length);
        }

        return names[x];
    }
}
