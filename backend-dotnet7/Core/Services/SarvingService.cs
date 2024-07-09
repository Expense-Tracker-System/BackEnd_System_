using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace backend_dotnet7.Core.Services
{
    public class SarvingService : ISavingService

    {

        private readonly ApplicationDbContext context;

        public SarvingService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<savingViewDTO>> GetSavingDetails(string userName, savingViewrequestDTO request)
        {

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            //var transactions = context.SavingViewEntitiess.Where(t => t.userName == userName).Select(t => new SavingViewEntities
            //{
            //    Id = t.Id,
            //    Amount= t.Amount,
            //    BankName = t.BankName,
            //    FromDate = t.FromDate,
            //    userName = t.userName
            //});

            //return (Task<SavingViewEntities>)transactions;
            // var query = context.SavingViewEntitiess.Where(t => t. == userName);

            //if (model.FromDate.HasValue && model.ToDate.HasValue)
            //{
            //    query = query.Where(t => t.FromDate >= model.FromDate.Value && t.FromDate <= model.ToDate.Value);
            //}

            //return await query.Select(t => new SavingViewEntities
            //{
            //    Id = t.Id,
            //    Amount = t.Amount,
            //    BankName = t.BankName,
            //    FromDate = t.FromDate,
            //    userName = userName

            //}).ToListAsync();

            var savingDetails = await context.SavingViewEntitiess.Where(sa => sa.userName == userName && sa.BankName == request.BankName && sa.Date >= request.StartDate && sa.Date <= request.EndDate).Select(sa=> new savingViewDTO(sa.Id,sa.Amount,sa.BankName,sa.Date,sa.userName)).ToListAsync();
            return savingDetails;

        }

        public async Task<SavingViewEntities> PostSarvingDetails(SavingViewEntities model, string userName)
        {
            var entity = new SavingViewEntities
            {
                Id = model.Id,
                Amount = model.Amount,
                Description = model.Description,
                BankName = model.BankName,
                Date = model.Date,
                userName = userName, // Assign the UserName from DTO
            };

            context.SavingViewEntitiess.Add(model);
            await context.SaveChangesAsync();

            return model;
        }
    }
    }

     
