namespace TicketSystemWebApi.Models
{
    public class LoginResponseDto
    {
        public bool Success { get; set; } = default!;

        public Guid UserID { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string Error { get; set; } = default!;
    }
}
