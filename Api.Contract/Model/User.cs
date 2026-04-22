using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Contract.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string Username { get; set; }

    [Required]
    [StringLength(20)]
    public required string Role { get; set; }

    public ICollection<LeaveQuota> LeaveQuotas { get; set; } = new List<LeaveQuota>();

    public ICollection<LeaveRequest> SubmittedLeaveRequests { get; set; } = new List<LeaveRequest>();

    public ICollection<LeaveRequest> ApprovedLeaveRequests { get; set; } = new List<LeaveRequest>();
}
