public class Revision {
    public int config { get; set; }
    public int notice { get; set; }

    // local only
    public bool isConfigUpdated { get; private set; }
    public bool isNoticeUpdated { get; private set; }

    public void DiffFrom(Revision localRevision) {
        isConfigUpdated = (config != localRevision.config);
        isNoticeUpdated = (notice != localRevision.notice);
    }
}