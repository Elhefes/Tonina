using UnityEngine;

public interface IsTalkable
{
    TextSubject Subject { get; }
    bool CanTalk { get; }
    int LineLimit { get; } // how many lines this talkable will actually speak/advance through

    void OnStartTalking(GameObject player);
    void ProcessNextLines();
    void StopTalking();
}