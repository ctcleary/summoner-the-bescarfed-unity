using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioModule : NPCModule, INPCModule
{
    protected AudioSource audioSource;
    public AudioClip attackSound;

    protected override Dictionary<MessageType, Action<Message>> GetSupportedMessageMap()
    {
        return new Dictionary<MessageType, Action<Message>>()
        {
            { MessageType.Attacking, HandleAttacking },
        };
    }

    public override void Awake()
    {
        // TODO Sound Controller Module?
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    private void HandleAttacking(Message message)
    {
        if (message.BoolValue && attackSound != null && audioSource.enabled)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    // Implement Abstract
    public override void Reset()
    {

    }
}
