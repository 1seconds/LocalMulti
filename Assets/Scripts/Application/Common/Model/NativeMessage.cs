using Newtonsoft.Json;
using UnityEngine;

public class NativeReqMessage {
    public int seq { get; set; }
    public string key { get; set; } // method
    public string value { get; set; } // json

    [JsonIgnore]
    public NativeResMessage response { get; set; }
}

public class NativeResMessage {
    public int seq { get; set; }
    public string value { get; set; } // json
}

