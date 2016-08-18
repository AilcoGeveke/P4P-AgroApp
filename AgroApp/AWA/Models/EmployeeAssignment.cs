namespace AWA.Models
{
    public class EmployeeAssignment
    {
        public int EmployeeAssignmentId { get; set; }
        public bool IsVerified { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
    }
}
