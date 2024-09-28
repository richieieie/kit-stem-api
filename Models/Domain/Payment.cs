﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        public int MethodId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public bool Status { get; set; }
        public int Amount { get; set; }

        [ForeignKey("MethodId")]
        [InverseProperty("Payments")]
        public virtual Method Method { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Payment")]
        public virtual ICollection<UserOrders>? UserOrders { get; set; }
    }
}