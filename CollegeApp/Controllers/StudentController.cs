using CollegeApp.Models;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        //private readonly IMyLogger _myLogger;
        //// 1. Tightly-coupled
        //public StudentController()
        //{
        //    _myLogger = new LogToServerMemory();
        //}

        //private readonly IMyLogger _myLogger;
        //// 2. Loosely-coupled - Dependency Injection
        //public StudentController(IMyLogger myLogger)
        //{
        //    _myLogger = myLogger;
        //}

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            //var students = new List<StudentDTO>();

            //foreach (var student in CollegeRepository.Students)
            //{
            //    StudentDTO obj = new StudentDTO()
            //    {
            //        Id = student.Id,
            //        Name = student.Name,
            //        Email = student.Email,
            //        Address = student.Address,
            //    };

            //    students.Add(obj);
            //}

            //_myLogger.Log("Get All Students");
            _logger.LogInformation("GetStudents method started");

            var students = CollegeRepository.Students.Select(std => new StudentDTO
            {
                Id = std.Id,
                Name = std.Name,
                Email = std.Email,
                Address = std.Address
            });
            //Ok - 200 Success
            return Ok(students);
        }

        [HttpGet]
        //[HttpGet("{id:int}", Name = "GetStudentById")]
        [Route("{id:int:max(100)}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Student> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request in GetStudentById");
                return BadRequest(); // 400 Client Error
            }

            var student = CollegeRepository.Students.Where(std => std.Id == id).FirstOrDefault();

            if (student == null)
            {
                _logger.LogError($"Student with id {id} not found");
                return NotFound($"Student with id {id} not found"); // 404
            }

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address
            };

            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("{name:regex(^[[a-zA-Z]]+$)}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Student> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest(); // 400 Client Error

            var student = CollegeRepository.Students.Where(std => std.Name == name).FirstOrDefault();

            if (student == null)
                return NotFound($"Student with name {name} not found");

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address
            };

            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            /**
             * If [ApiController] annotation isn't there, then we need to add this:
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            **/

            if (model == null)
            {
                return BadRequest();
            }

            ///// ---- Custom Validations ----

            ////// 1. Directly adding error message to ModelState     
            //if (model.AdmissionDate < DateTime.Now)
            //{
            //    ModelState.AddModelError("Admission Date Error", "Admission date must be after or equal to today's date");
            //    return BadRequest(ModelState);
            //}

            //// 2. Using custom attribute
            ////  Created a Validators folder and added DateCheckAttribute class, 
            //// then used it as annotation above the StudentDTO AdmissionDate field.

            if (CollegeRepository.Students.Any(std => std.Email == model.Email))
            {
                return BadRequest($"Email {model.Email} is already in use.");
            }

            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;

            Student student = new Student
            {
                Id = newId,
                Name = model.Name,
                Email = model.Email,
                Address = model.Address,
            };

            CollegeRepository.Students.Add(student);

            model.Id = newId;


            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
            /**
             * This CreatedAtRoute will prepare the link for the newly created record.
               Status - 201
               http://localhost:7185/api/Student/{model.id}
             **/
        }


        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = CollegeRepository.Students.Where(std => std.Id == model.Id).FirstOrDefault();

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.Name = model.Name;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;

            //return Ok(existingStudent);
            return NoContent();
        }

        [HttpPatch]
        [Route("UpdatePartial/{id:int}")]
        // api/student/{id}/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();

            var existingStudent = CollegeRepository.Students.Where(std => std.Id == id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound();

            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                Name = existingStudent.Name,
                Email = existingStudent.Email,
                Address = existingStudent.Address,
            };

            patchDocument.ApplyTo(studentDTO, ModelState);
            // send ModelState as 2nd argument so that
            // if anything goes wrong while applying, the errors will be 
            // added to ModelState

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            // Update the existingStudent record to have the new values
            existingStudent.Name = studentDTO.Name;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;

            return NoContent(); // 204
        }

        [HttpDelete("Delete/{id:int}", Name = "DeleteStudentById")]
        // api/student/delete/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudent(int id)
        {
            if (id <= 0) return BadRequest(); // 400 Client Error

            var student = CollegeRepository.Students.Where(std => std.Id == id).FirstOrDefault();

            if (student == null)
                return NotFound($"Student with id {id} not found"); // 404

            CollegeRepository.Students.Remove(student);

            return Ok(true);
        }
    }
}
