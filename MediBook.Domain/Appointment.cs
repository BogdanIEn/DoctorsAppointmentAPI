namespace MediBook.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "confirmed";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Doctor? Doctor { get; set; }
}
