﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public IList<string>? Roles { get; set; }
        public string? UserImage { get; set; }
        public bool IsDeactivateRequest { get; set; }

        // navigate -> ICollection is interface / List is concreate class
        public ICollection<Log> logs { get; set; } = new List<Log>();
        public ICollection<Message> messages { get; set; } = new List<Message>();
        public ICollection<Budget> budgets { get; set; } = new List<Budget>();
        public ICollection<Reminder> reminders { get; set; } = new List<Reminder>();
        public ICollection<UserIncome> userIncomes { get; set; } = new List<UserIncome>();
        public ICollection<UserExpense> userExpenses { get; set; } = new List<UserExpense>();
        public ICollection<UserOrganization> userOrganizations { get; set; } = new List<UserOrganization>();
        public ICollection<DeactivateUserAccount> deactivateUserAccounts { get; set; } = new List<DeactivateUserAccount>();
        public ICollection<Report> reports { get; set; } = new List<Report>();
    }
}
