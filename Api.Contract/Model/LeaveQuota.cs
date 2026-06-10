using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Contract.Model;

public class LeaveQuota
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [StringLength(20)]
    public required string LeaveType { get; set; }

    [Precision(5, 1)]
    public decimal TotalDays { get; set; }

    [Precision(5, 1)]
    public decimal RemainingDays { get; set; }

    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }
}