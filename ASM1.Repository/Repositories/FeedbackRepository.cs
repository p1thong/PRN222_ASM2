using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(CarSalesDbContext context) : base(context)
        {
        }

        public IEnumerable<Feedback> GetAllFeedbacks()
        {
            return _context.Set<Feedback>()
                .Include(f => f.Customer)
                .ToList();
        }

        public Feedback? GetFeedbackById(int feedbackId)
        {
            return _context.Set<Feedback>()
                .Include(f => f.Customer)
                .FirstOrDefault(f => f.FeedbackId == feedbackId);
        }

        public void AddFeedback(Feedback feedback)
        {
            _context.Set<Feedback>().Add(feedback);
            _context.SaveChanges();
        }

        public void UpdateFeedback(Feedback feedback)
        {
            _context.Set<Feedback>().Update(feedback);
            _context.SaveChanges();
        }

        public void DeleteFeedback(int feedbackId)
        {
            var feedback = GetFeedbackById(feedbackId);
            if (feedback != null)
            {
                _context.Set<Feedback>().Remove(feedback);
                _context.SaveChanges();
            }
        }
    }
}