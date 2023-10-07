namespace fourthAPI.DTOs
{
    public class UserLoginSuccessDTO
    {
        public string message { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string token { get; set; }
        public int teamId {  get; set; }
    }
}
