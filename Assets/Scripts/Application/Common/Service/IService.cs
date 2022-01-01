using System.Threading.Tasks;

public enum ServiceType {
    StringBundle,
    Setting,
    Rule,
    Lobby,
    Unit,
    Stage
}

public interface ServiceStatePresenter {
    void ShowServiceState(string key);
}

public interface IService {
    ServiceType type { get; }
    Task<bool> Initialize(ServiceStatePresenter presenter);
}