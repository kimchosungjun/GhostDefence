using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LobbyUI : MonoBehaviour
{
    public LobbyUIController UIController { get; set; } = null;
    public abstract void OnOffUI(bool _isActive);
    public abstract void Init();
    public virtual bool CanCloseWindow() { return true; }
}
