using Entity_Framework.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using Entity_Framework.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Framework.Messages;

namespace Entity_Framework.Services
{

    public static class TeacherService
    {
        private static readonly MyAppDbContext _context;
        static TeacherService()
        {
            _context = new MyAppDbContext();
        }
        public static void AddTeacher()
        {
        TeacherName: BasicMessages.InputMessage("name");
            string teacherName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(teacherName))
            {
                ErrorMessages.InvalidInputMessage(teacherName);
                goto TeacherName;
            }
        TeacherSurname: BasicMessages.InputMessage("surname");
            string teacherSurname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(teacherSurname))
            {
                ErrorMessages.InvalidInputMessage($"{teacherSurname}");
                goto TeacherSurname;
            }
            Teacher teacher = new Teacher();
            teacher.Name = teacherName;
            teacher.Surname = teacherSurname;
            _context.Teachers.Add(teacher);
            try
            {
               _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(teacherName, "added");
        }
        public static void RemoveTeacher()
        {
            TeacherInput: GetAllTeachers();
            BasicMessages.InputMessage("id");
            string inputId = Console.ReadLine();
            bool isSucceded = int.TryParse(inputId, out int teacherId);
            if (!isSucceded)
            {
                ErrorMessages.InvalidInputMessage(inputId);
                goto TeacherInput;
            }
            var existTeacher = _context.Teachers.Include(x => x.Groups).FirstOrDefault(x => x.Id == teacherId);
            if (existTeacher == null) 
            {
                ErrorMessages.InvalidInputMessage(inputId);
                goto TeacherInput;
            }
            foreach ( var group in existTeacher.Groups)
            {
                var group1 = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == group.Id);
                foreach(var student in group1.Students)
                {
                    student.IsDeleted = true;
                }
                group.IsDeleted = true;
            }
            existTeacher.IsDeleted = true;
            _context.Teachers.Update(existTeacher);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(existTeacher.Name, "deleted");
        }
        public static void GetAllTeachers()
        {
            foreach (var teacher in _context.Teachers)
            {
                Console.WriteLine($"Id: {teacher.Id} | Name: {teacher.Name} | Surname: {teacher.Surname}");
            }
        }
        public static void UpdateTeacher()
        {
        TeacherInput: GetAllTeachers();
            BasicMessages.InputMessage("id");
            string inputId = Console.ReadLine();
            bool isSucceded = int.TryParse((inputId), out int teacherId);
            if (!isSucceded || string.IsNullOrWhiteSpace(inputId))
            {
                ErrorMessages.InvalidInputMessage(inputId);
                goto TeacherInput;
            }
            var existTeacher = _context.Teachers.Find(teacherId);
            if (existTeacher == null)
            {
                ErrorMessages.NotFoundMessage(inputId);
                goto TeacherInput;
            }
            string Name = existTeacher.Name;
            string Surname = existTeacher.Surname;
            existTeacher.Name = Name;
            existTeacher.Surname = Surname;
            ChangeInput: BasicMessages.WhatToChangeTeacher();
            string answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer))
            {
                ErrorMessages.InvalidInputMessage(answer);
                goto ChangeInput;
            }
            switch (answer.ToLower())
            {
                case "name":
                NameInput: BasicMessages.InputMessage("name");
                    Name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        ErrorMessages.InvalidInputMessage(Name);
                        goto NameInput;
                    }
                    existTeacher.Name = Name;
                    break;
                case "surname":
                SurnameInput: BasicMessages.InputMessage("Surname");
                    Surname = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(Surname))
                    {
                        ErrorMessages.InvalidInputMessage(Surname);
                        goto SurnameInput;
                    }
                    existTeacher.Surname = Surname;
                    break;
                case "both":
                BothInputName: BasicMessages.InputMessage("name");
                    Name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        ErrorMessages.InvalidInputMessage(Name);
                        goto BothInputName;
                    }
                BothInputSurname: BasicMessages.InputMessage("Surname");
                    Surname = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(Surname))
                    {
                        ErrorMessages.InvalidInputMessage(Surname);
                        goto BothInputSurname;
                    }
                    existTeacher.Name = Name;
                    existTeacher.Surname = Surname;
                    break;
                default:
                    ErrorMessages.InvalidInputMessage(answer);
                    goto ChangeInput;
            }
            _context.Teachers.Update(existTeacher);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(Name, "updated");
        }
        public static void GetTeacherDetails()
        {
        InputId: GetAllTeachers();
            BasicMessages.InputMessage("teacher id");
            string inputId = Console.ReadLine();
            int teacherId;
            bool isSucceded = int.TryParse(inputId, out teacherId);
            if (!isSucceded || string.IsNullOrWhiteSpace(inputId))
            {
                ErrorMessages.InvalidInputMessage(inputId);
                goto InputId;
            }
            var existTeacher = _context.Teachers.Include(x => x.Groups).FirstOrDefault(x => x.Id == teacherId);
            if (existTeacher == null)
            {
                ErrorMessages.NotFoundMessage("teacher");
                goto InputId;
            }
            Console.WriteLine($"Id: {existTeacher.Id} | Name: {existTeacher.Name} | Surname: {existTeacher.Surname}");
            Console.WriteLine("Groups: ");
            foreach (var group in existTeacher.Groups)
            {
                Console.WriteLine(group.Name);
            }
        }
    }
}
