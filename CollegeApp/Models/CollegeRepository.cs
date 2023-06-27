namespace CollegeApp.Models
{
    public class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>()
        {
                new Student
                {
                    Id = 1,
                    Name = "Test",
                    Email = "test@gmail.com",
                    Address = "Hyd, India"
                },
                new Student
                {
                    Id = 2,
                    Name = "Faris",
                    Email = "faris@gmail.com",
                    Address = "Nablus, Palestine"
                }
        };
    }
}
