using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        IEnumerable<Feedback> GetAllFeedbacks();
        Feedback? GetFeedbackById(int feedbackId);
        void AddFeedback(Feedback feedback);
        void UpdateFeedback(Feedback feedback);
        void DeleteFeedback(int feedbackId);

    }
}