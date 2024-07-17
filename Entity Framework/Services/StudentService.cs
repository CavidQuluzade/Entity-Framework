using Entity_Framework.Contexts;
using Entity_Framework.Entity;
using Entity_Framework.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Services
{
    public static class StudentService
    {
        private static readonly MyAppDbContext _context;
        static StudentService()
        {
            _context = new MyAppDbContext();
        }
        public static void AddStudent()
        {
            var CountGroups = _context.Groups.Count();
            if (CountGroups <= 0)
            {
                ErrorMessages.CountIsZeroMessage("group");
                return;
            }
        GroupInput: GroupService.GetAllGroups();
            BasicMessages.InputMessage("group id");
            string groupIdInput = Console.ReadLine();
            int groupId;
            bool isSucceded = int.TryParse(groupIdInput, out groupId);
            if (!isSucceded || string.IsNullOrWhiteSpace(groupIdInput))
            {
                ErrorMessages.InvalidInputMessage(groupIdInput);
                goto GroupInput;
            }
            var existGroup = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == groupId);
            if (existGroup == null)
            {
                ErrorMessages.NotFoundMessage(groupIdInput);
                goto GroupInput;
            }
            var CountStudentsinGroup = existGroup.Students.Count();
            if (CountStudentsinGroup + 1 > existGroup.Limit)
            {
                ErrorMessages.StudentLimitMessage();
                if(CountGroups == 1)
                return;
                else 
                goto GroupInput;
            }
        NameInput: BasicMessages.InputMessage("student name");
            string studentName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(studentName))
            {
                ErrorMessages.InvalidInputMessage(studentName);
                goto NameInput;
            }
        SurnameInput: BasicMessages.InputMessage("student surname");
            string studentSurname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(studentSurname))
            {
                ErrorMessages.InvalidInputMessage(studentSurname);
                goto SurnameInput;
            }
        EmailInput: BasicMessages.InputMessage("student Email (must contain '@')");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                ErrorMessages.InvalidInputMessage(email);
                goto EmailInput;
            }
            var existEmail = _context.Students.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (existEmail != null)
            {
                ErrorMessages.ExistMessage(email);
                goto EmailInput;
            }
        BirthDateInout: BasicMessages.InoutBeginDateMessage("student birthdate");
            string BirthdateInput = Console.ReadLine();
            DateTime studentBirthdate;
            isSucceded = DateTime.TryParseExact(BirthdateInput, format: "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out studentBirthdate);
            if (!isSucceded || string.IsNullOrWhiteSpace(BirthdateInput))
            {
                ErrorMessages.InvalidInputMessage(BirthdateInput);
                goto BirthDateInout;
            }
            Student student = new Student();
            student.Surname = studentSurname;
            student.Email = email;
            student.BirthDate = studentBirthdate;
            student.GroupId = groupId;
            student.Name = studentName;
            _context.Students.Add(student);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(studentName, "added");
        }
        public static void GetAllStudents()
        {
            foreach (var student in _context.Students)
            {
                Console.WriteLine($"Id:{student.Id} | Name: {student.Name} | Surname: {student.Surname} | Name: {student.Name} | Email: {student.Email} ");
            }
        }
        public static void DeleteStudent()
        {
        StudentInput: GetAllStudents();
            BasicMessages.InputMessage("student id");
            string InputId = Console.ReadLine();
            int studentId;
            bool isSucceded = int.TryParse(InputId, out studentId);
            if (!isSucceded || string.IsNullOrWhiteSpace(InputId))
            {
                ErrorMessages.InvalidInputMessage(InputId);
                goto StudentInput;
            }
            var existStudent = _context.Students.Find(studentId);
            if (existStudent == null)
            {
                ErrorMessages.NotFoundMessage(InputId);
                goto StudentInput;
            }
            existStudent.IsDeleted = true;
            _context.Students.Update(existStudent);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(existStudent.Name, "deleted");
        }
        public static void UpdateStudent()
        {
            var studentCount = _context.Students.Count();
            if (studentCount <= 0)
            {
                ErrorMessages.CountIsZeroMessage("student");
                return;
            }
        UpdateInput: GetAllStudents();
            BasicMessages.InputMessage("student id");
            string inputId = Console.ReadLine();
            int studentId;
            bool isSucceded = int.TryParse(inputId, out studentId);
            if (!isSucceded || string.IsNullOrWhiteSpace(inputId))
            {
                ErrorMessages.InvalidInputMessage(inputId);
                goto UpdateInput;
            }
            var existStudent = _context.Students.FirstOrDefault(x => x.Id == studentId);
            if (existStudent == null)
            {
                ErrorMessages.NotFoundMessage(inputId);
                goto UpdateInput;
            }
            string studentName = existStudent.Name;
            string studentSurname = existStudent.Surname;
            string studentEmail = existStudent.Email;
            DateTime studentBirthdate = existStudent.BirthDate;
            int studentGroupId = existStudent.GroupId;
        NameInput: BasicMessages.WantToChangeMessage("student name");
            string input = Console.ReadLine();
            char result;
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto NameInput;
            }
            if (result == 'y')
            {
            StudentNameInput: BasicMessages.InputMessage("new student name");
                studentName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(studentName))
                {
                    ErrorMessages.InvalidInputMessage(studentName);
                    goto StudentNameInput;
                }
                existStudent.Name = studentName;
            }
        SurnameInput: BasicMessages.WantToChangeMessage("student surname");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto SurnameInput;
            }
            if (result == 'y')
            {
            StudentSurnameInput: BasicMessages.InputMessage("new student surname");
                studentSurname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(studentSurname))
                {
                    ErrorMessages.InvalidInputMessage(studentSurname);
                    goto StudentSurnameInput;
                }
                existStudent.Surname = studentSurname;
            }
        EmailInput: BasicMessages.WantToChangeMessage("student email");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto EmailInput;
            }
            if (result == 'y')
            {
            StudentEmailInput: BasicMessages.InputMessage("new student email");
                studentEmail = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(studentEmail))
                {
                    ErrorMessages.InvalidInputMessage(studentEmail);
                    goto StudentEmailInput;
                }
                var existEmail = _context.Students.FirstOrDefault(x => x.Email == studentEmail);
                if (existEmail != null)
                {
                    ErrorMessages.ExistMessage("student email");
                    goto EmailInput;
                }
                existStudent.Email = studentEmail;
            }
        BirthdateInput: BasicMessages.WantToChangeMessage("student birthdate");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto BirthdateInput;
            }
            if (result == 'y')
            {
                BasicMessages.InoutBeginDateMessage("birthdate");
                input = Console.ReadLine();
                isSucceded = DateTime.TryParseExact(input, format: "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out studentBirthdate);
                if (!isSucceded || string.IsNullOrWhiteSpace(input))
                {
                    ErrorMessages.InvalidInputMessage(input);
                    goto BirthdateInput;
                }
                existStudent.BirthDate = studentBirthdate;
            }
        GroupInput: BasicMessages.WantToChangeMessage("group");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto GroupInput;
            }
            if (result == 'y')
            {
                var CountGroup = _context.Groups.Count();
                if (CountGroup <= 1)
                {
                    ErrorMessages.CountIsZeroMessage("group");
                    return;
                }
                GroupService.GetAllGroups();
                BasicMessages.InputMessage("group id");
                input = Console.ReadLine();
                isSucceded = int.TryParse(input, out studentGroupId);
                if (!isSucceded || string.IsNullOrWhiteSpace(input))
                {
                    ErrorMessages.InvalidInputMessage(input);
                    goto GroupInput;
                }
                var existGroup = _context.Groups.Find(studentGroupId);
                if (existGroup == null)
                {
                    ErrorMessages.NotFoundMessage(input);
                    goto GroupInput;
                }
                existStudent.GroupId = studentGroupId;
            }
            _context.Students.Update(existStudent);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(existStudent.Name, "updated");
        }
            public static void GetDetailsOfStudent()
        {
        StudentInput: GetAllStudents();
            BasicMessages.InputMessage("student id");
            string InputId = Console.ReadLine();
            int studentId;
            bool isSucceded = int.TryParse(InputId, out studentId);
            if (!isSucceded || string.IsNullOrWhiteSpace(InputId))
            {
                ErrorMessages.InvalidInputMessage(InputId);
                goto StudentInput;
            }
            var existStudent = _context.Students.Include(x => x.Group).Include(x => x.Group.Teacher).FirstOrDefault(x => x.Id == studentId);
            if (existStudent == null)
            {
                ErrorMessages.NotFoundMessage(InputId);
                goto StudentInput;
            }
            Console.WriteLine($"Id: {existStudent.Id} | Surname: {existStudent.Surname} | Name: {existStudent.Name} | Email: {existStudent.Email} | Birthdate: {existStudent.BirthDate} | Group: {existStudent.Group.Name} | Teacher: {existStudent.Group.Teacher.Surname} {existStudent.Group.Teacher.Name}");
        }
    }
}
