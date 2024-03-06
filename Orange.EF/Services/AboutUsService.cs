namespace Orange.EF.Services;

public class AboutUsService
{
    private static string _aboutUsText = "";

    public static void Update(string text)
    {
        _aboutUsText = text;
    }

    public static string GetAboutUs() => _aboutUsText;
}