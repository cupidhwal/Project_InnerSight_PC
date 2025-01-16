using UnityEngine;

namespace Seti
{
    public enum GameMessageType
    {
        Damaged,
        Dead,
        Respawn,
        //...
    }

    /// <summary>
    /// IMessageReceiver를 상속 받은 클래스에게만 메시지 타입 전달
    /// </summary>
    public interface IMessageReceiver
    {
        void OnReceiveMessage(GameMessageType type, object sender, object msg);
    }

    public class MessageSystem
    {

    }
}