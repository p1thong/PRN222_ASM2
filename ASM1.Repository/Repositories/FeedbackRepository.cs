using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Repository.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly CarSalesDbContext _context;
        public FeedbackRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        public void AddFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
        }

        public IEnumerable<Feedback> GetAllFeedbacks() => _context.Feedbacks.ToList();

        public Feedback GetFeedBackById(int id) => _context.Feedbacks.Find(id);
    }
}
