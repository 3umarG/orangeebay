namespace Orange_Bay.Models.ContactUs;

public class ContactUsMessage
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime CreatedOn { get; set; }
}