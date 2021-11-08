using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalChatChannel : ChatChannel
{

    #region Singleton
    public static ChatChannel instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of GlobalChat found!");
            return;
        }
        instance = this;
    }
    #endregion
}
