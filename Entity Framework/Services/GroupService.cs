using Entity_Framework.Contexts;
using Entity_Framework.Entity;
using Entity_Framework.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Services
{
    public static class GroupService
    {
        private static readonly MyAppDbContext _context;
        static GroupService()
        {
            _context = new MyAppDbContext();
        }
        public static void AddGroup()
        {
        GroupNameInput: BasicMessages.InputMessage("group name");
            string groupName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(groupName))
            {
                ErrorMessages.InvalidInputMessage(groupName);
                goto GroupNameInput;
            }
        GroupLimitInput: BasicMessages.InputMessage("group limit");
            string groupLimitInput = Console.ReadLine();
            int groupLimit;
            bool isSucceded = int.TryParse(groupLimitInput, out groupLimit);
            if (!isSucceded || string.IsNullOrWhiteSpace(groupLimitInput))
            {
                ErrorMessages.InvalidInputMessage(groupLimitInput);
                goto GroupLimitInput;
            }
            if (groupLimit <= 0 || groupLimit > 20)
            {
                ErrorMessages.LimitInputMessage(groupLimitInput);
                goto GroupLimitInput;
            }
            var CountTeacher = _context.Teachers.Count();
            if (CountTeacher == 0)
            {
                ErrorMessages.CountIsZeroMessage("teacher");
                return;
            }
        TeacherInput: TeacherService.GetAllTeachers();
            BasicMessages.InputMessage("id");
            string inputId = Console.ReadLine();
            isSucceded = int.TryParse((inputId), out int teacherId);
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
        BeginDateInput: BasicMessages.InputMessage("Begin date");
            string BegindateInput = Console.ReadLine();
            DateTime BeginDate;
            isSucceded = DateTime.TryParseExact(BegindateInput, format: "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out BeginDate);
            if (!isSucceded)
            {
                ErrorMessages.InvalidInputMessage(BegindateInput);
                goto BeginDateInput;
            }
        EndDateInput: BasicMessages.InputMessage("End date");
            string EnddateInput = Console.ReadLine();
            DateTime EndDate;
            isSucceded = DateTime.TryParseExact(EnddateInput, format: "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out EndDate);
            if (!isSucceded || BeginDate.AddMonths(6) > EndDate)
            {
                ErrorMessages.InvalidInputMessage(BegindateInput);
                goto EndDateInput;
            }
            Group group = new Group();
            group.Name = groupName;
            group.Limit = groupLimit;
            group.TeacherId = teacherId;
            group.BeginDate = BeginDate;
            group.EndDate = EndDate;
            _context.Groups.Add(group);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(groupName, "added");
        }
        public static void GetAllGroups()
        {
            foreach (var group in _context.Groups)
            {
                Console.WriteLine($"Id: {group.Id} | Name: {group.Name} | Limit: {group.Limit} | Begin date: {group.BeginDate} | End date: {group.EndDate}");
            }
        }
        public static void RemoveGroup()
        {
            var groupCount = _context.Groups.Count();
            if (groupCount <= 0)
            {
                ErrorMessages.CountIsZeroMessage("group");
                return;
            }
        GroupIdInput: GetAllGroups();
            Console.WriteLine("All students will be deleted");
            BasicMessages.InputMessage("group id");
            string inputId = Console.ReadLine();
            int groupId;
            bool isSucceded = int.TryParse(inputId, out groupId);
            if (!isSucceded || string.IsNullOrWhiteSpace(inputId))
            {
                ErrorMessages.InvalidInputMessage(inputId);
                goto GroupIdInput;
            }
            var existGroup = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == groupId);
            if (existGroup == null)
            {
                ErrorMessages.NotFoundMessage(inputId);
                goto GroupIdInput;
            }
            foreach (var student in existGroup.Students)
            {
                student.IsDeleted = true;
            }
            existGroup.IsDeleted = true;
            _context.Groups.Update(existGroup);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(existGroup.Name, "deleted");
        }
        public static void GetDetailOfGroup()
        {
        GroupInput: GetAllGroups();
            string inputId = Console.ReadLine();
            int groupId;
            bool isSucceded = int.TryParse(inputId, out groupId);
            if (!isSucceded || string.IsNullOrWhiteSpace(inputId))
            {
                ErrorMessages.InvalidInputMessage(inputId);
                goto GroupInput;
            }
            var existGroup = _context.Groups.Include(x => x.Teacher).Include(x => x.Students).FirstOrDefault(x => x.Id == groupId);
            if (existGroup == null)
            {
                ErrorMessages.NotFoundMessage(inputId);
                goto GroupInput;
            }
            Console.WriteLine($"Id: {groupId} | Name: {existGroup.Name} | Limit: {existGroup.Limit} | Teacher: {existGroup.Teacher.Surname} {existGroup.Teacher.Name}");
            Console.WriteLine("Students:");
            foreach (var student in existGroup.Students)
            {
                Console.WriteLine($"{student.Surname} {student.Name}");
            }
        }
        public static void UpdateGroup()
        {
            var groupCount = _context.Groups.Count();
            if (groupCount <= 0)
            {
                ErrorMessages.CountIsZeroMessage("group");
                return;
            }
        UpdateInput: GetAllGroups();
            BasicMessages.InputMessage("group id");
            string inputId = Console.ReadLine();
            int groupId;
            bool isSucceded = int.TryParse(inputId, out groupId);
            if (!isSucceded || string.IsNullOrWhiteSpace(inputId))
            {
                ErrorMessages.InvalidInputMessage(inputId);
                goto UpdateInput;
            }
            var existGroup = _context.Groups.FirstOrDefault(x => x.Id == groupId);
            if (existGroup == null)
            {
                ErrorMessages.NotFoundMessage(inputId);
                goto UpdateInput;
            }
            string groupName = existGroup.Name;
            int groupLimit = existGroup.Limit;
            int groupTeacherId = existGroup.TeacherId;
            DateTime groupBegindate = existGroup.BeginDate;
            DateTime groupEnddate = existGroup.EndDate;
        NameInput: BasicMessages.WantToChangeMessage("group name");
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
            GroupNameInput: BasicMessages.InputMessage("new group name");
                groupName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    ErrorMessages.InvalidInputMessage(groupName);
                    goto GroupNameInput;
                }
                var existName = _context.Groups.FirstOrDefault(x => x.Name == groupName);
                if (existName != null)
                {
                    ErrorMessages.ExistMessage("group name");
                    goto NameInput;
                }
                existGroup.Name = groupName;
            }
        LimitInput: BasicMessages.WantToChangeMessage("group limit");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto LimitInput;
            }
            if (result == 'y')
            {
            GroupLimitInput: BasicMessages.InputMessage("new limit");
                input = Console.ReadLine();
                isSucceded = int.TryParse(input, out groupLimit);
                if (!isSucceded || string.IsNullOrWhiteSpace(groupName))
                {
                    ErrorMessages.InvalidInputMessage(groupName);
                    goto GroupLimitInput;
                }
                var studentCount = existGroup.Students.Count();
                if (groupLimit < studentCount)
                {
                    ErrorMessages.StudentLimitMessage();
                    goto GroupLimitInput;
                }
                existGroup.Limit = groupLimit;
            }
        BegindateInput: BasicMessages.WantToChangeMessage("group begin date");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto BegindateInput;
            }
            if (result == 'y')
            {
                BasicMessages.InoutBeginDateMessage("begin date");
                input = Console.ReadLine();
                isSucceded = DateTime.TryParseExact(input, format: "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out groupBegindate);
                if (!isSucceded || string.IsNullOrWhiteSpace(input))
                {
                    ErrorMessages.InvalidInputMessage(input);
                    goto BegindateInput;
                }
                existGroup.BeginDate = groupBegindate;

            }
        EnddateInput: BasicMessages.WantToChangeMessage("group end date");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto EnddateInput;
            }
            if (result == 'y')
            {
                BasicMessages.InoutEndDateMessage("end date", 6);
                input = Console.ReadLine();
                isSucceded = DateTime.TryParseExact(input, format: "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out groupEnddate);
                if (!isSucceded || string.IsNullOrWhiteSpace(input))
                {
                    ErrorMessages.InvalidInputMessage(input);
                    goto EnddateInput;
                }
            }
            if (existGroup.BeginDate.AddMonths(6).Date >= groupEnddate.Date)
            {
                ErrorMessages.InvalidInputMessage("group date");
                goto BegindateInput;
            }
            existGroup.EndDate = groupEnddate;
        TeacherInput: BasicMessages.WantToChangeMessage("teacher");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out result);
            if (!isSucceded || string.IsNullOrWhiteSpace(input) || result != 'y' && result != 'n')
            {
                ErrorMessages.InvalidInputMessage(input);
                goto TeacherInput;
            }
            if (result == 'y')
            {
                var CountTeacher = _context.Teachers.Count();
                if (CountTeacher <= 1)
                {
                    ErrorMessages.CountIsZeroMessage("teacher");
                }
                TeacherService.GetAllTeachers();
                BasicMessages.InputMessage("teacher id");
                input = Console.ReadLine();
                isSucceded = int.TryParse(input, out groupTeacherId);
                if (!isSucceded || string.IsNullOrWhiteSpace(input))
                {
                    ErrorMessages.InvalidInputMessage(input);
                    goto TeacherInput;
                }
                var existTeacher = _context.Teachers.Find(groupTeacherId);
                if (existTeacher == null)
                {
                    ErrorMessages.NotFoundMessage(inputId);
                    goto TeacherInput;
                }
                existGroup.TeacherId = groupTeacherId;
            }
            _context.Groups.Update(existGroup);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ErrorMessages.ErrorMessage();
            }
            BasicMessages.SuccessMessage(existGroup.Name, "updated");
        }
    }
}
