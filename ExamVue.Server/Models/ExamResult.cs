using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamApi.Models
{
    public class ExamResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public DateTime ExamDate { get; set; } = DateTime.Now;
    }
}
