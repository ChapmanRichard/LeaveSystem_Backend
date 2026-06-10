using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Contract.Model;

public class LeaveRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int ApplicantId { get; set; }

    [Required]
    [StringLength(20)]
    public required string LeaveType { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [Precision(5, 1)]
    public decimal Duration { get; set; }

    [Required]
    [StringLength(500)]
    public required string Reason { get; set; }

    [Required]
    [StringLength(20)]
    public required string Status { get; set; }

    public int? ApproverId { get; set; }

    [StringLength(500)]
    public string? ApprovalComment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(ApplicantId))]
    public required User Applicant { get; set; }

    [ForeignKey(nameof(ApproverId))]
    public User? Approver { get; set; }
}