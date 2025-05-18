namespace Business.Dtos;

public class SignUpResult
{
    public bool Succeeded { get; set; }
    public List<string> Errors { get; set; } = new();
    public string? Message { get; set; }
}
