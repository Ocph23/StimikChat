namespace StimikChatServer.Models
{
    public interface IConnection
    {
        string ConnectionID { get; set; }
        int UserId { get; set; }
        bool Connected { get; set; }
    }


}
