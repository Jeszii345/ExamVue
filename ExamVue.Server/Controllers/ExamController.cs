using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ExamApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamVue.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        public ExamController(IConfiguration configuration)
        {
            // ดึง Connection String จาก appsettings.json
            _connectionString = configuration.GetConnectionString("MySqlConnection");
        }
        // GET: api/<ExamController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ExamController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ExamController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<ExamController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<ExamController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private readonly string _connectionString;

      
        [HttpPost("submit")]
        public IActionResult SubmitExam([FromBody] ExamResult exam)
        {
            if (string.IsNullOrWhiteSpace(exam.Name))
                return BadRequest("Name is required");

            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();

                string sqlInsert = "INSERT INTO ExamResults (Name, Score) VALUES (@name, @score)";
                using (var cmdInsert = new MySqlCommand(sqlInsert, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@name", exam.Name);
                    cmdInsert.Parameters.AddWithValue("@score", exam.Score);
                    cmdInsert.ExecuteNonQuery();
                }

                return Ok(new { message = "บันทึกข้อมูลเรียบร้อย", exam.Name, exam.Score });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("results")]
        public IActionResult GetResults()
        {
            var results = new List<ExamResult>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            string sql = "SELECT * FROM ExamResults ORDER BY ExamDate DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ExamResult
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Score = reader.GetInt32("Score"),
                    ExamDate = reader.GetDateTime("ExamDate")
                });
            }

            return Ok(results);
        }

    }
}
