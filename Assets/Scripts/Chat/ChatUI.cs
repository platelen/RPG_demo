using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatUI : MonoBehaviour
{

    #region Singleton
    public static ChatUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of ChatUI found!");
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] Dropdown channelsDropdown;
    [SerializeField] InputField messageInput;
    [SerializeField] PrivateChatChannel privateChannel;
    [SerializeField] Transform massageContaner;
    [SerializeField] ChatMessageUI massagePrefab;

    PlayerChat playerChat;

    public void SetPlayerChat(PlayerChat chat)
    {
        playerChat = chat;
        RefreshChanels();
        playerChat.onChangeChannels += RefreshChanels;
        playerChat.onReciveMessage += ReciveChatMessage;
    }

    private void Update()
    {
        if (messageInput.isFocused && !messageInput.text.Equals("") && Input.GetKey(KeyCode.Return))
        {
            SendChatMessage(messageInput.text);
            messageInput.text = String.Empty;
        }
    }

    private void RefreshChanels()
    {
        channelsDropdown.ClearOptions();
        channelsDropdown.AddOptions(playerChat.channels.ConvertAll(x => x.name));
    }

    public void SendChatMessage(string text)
    {
        if (playerChat != null) playerChat.channels[channelsDropdown.value].SendFromPlayerChat(text);
    }

    public void ReciveChatMessage(ChatMessage message)
    {
        ChatMessageUI newMessage = Instantiate(massagePrefab, massageContaner);
        newMessage.SetChatMessage(message);
    }

    public void SetPrivateMessage(ChatMessage message)
    {
        privateChannel.SetReciverMessage(message);
        if (playerChat.channels.Contains(privateChannel))
        {
            RefreshChanels();
        }
        else
        {
            playerChat.RegisterChannel(privateChannel);
        }
        channelsDropdown.value = channelsDropdown.options.Count - 1;
    }
}
