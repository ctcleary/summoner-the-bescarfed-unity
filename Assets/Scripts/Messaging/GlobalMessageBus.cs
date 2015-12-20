using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GlobalMessageBus {
    
    // SINGLETON
    static MessageBus _instance;
    public static MessageBus Instance
    {
        get
        {
            if (_instance == null) {
                _instance = new MessageBus();
            }
            return _instance;
        }
    }
    private GlobalMessageBus() { }
}
