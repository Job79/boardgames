using Core.Domain;

namespace BoardGamesWebsite.Models;

public class RegistrationModel
{
    public int Id { get; set; }
    public bool? DidAttend { get; set; }
    
    public required User User { get; set; }
    public required int AttendanceCount { get; set; }
    public required int NonAttendanceCount { get; set; }
}