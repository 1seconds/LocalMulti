using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class LobbySceneService : Singleton<LobbySceneService>, IService {
    public ServiceType type => ServiceType.Lobby;
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        return true;
    }
}