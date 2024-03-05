namespace Orange_Bay.DTOs.Dashboard;

public class DashboardRegisterRequestDto : DashboardLoginRequestDto
{
    public DashboardRegisterOption UserType { get; set; }
}

public class DashboardLoginRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public enum DashboardRegisterOption
{
    Employee = 3,
    Admin = 4
}