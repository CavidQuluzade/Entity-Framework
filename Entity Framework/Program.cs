using Entity_Framework.Constats;
using Entity_Framework.Services;

namespace Entity_Framework
{
    public static class Program
    {
        static void Main(string[] args)
        {
            bool exit = true;
            while (exit)
            {
                ShowMenu();
                string input = Console.ReadLine();
                bool IsSucceded = int.TryParse(input, out int choice);
                switch ((Choice)choice)
                {
                    case Choice.AddTeacher:
                        TeacherService.AddTeacher();
                        break;
                    case Choice.AllTeacher:
                        TeacherService.GetAllTeachers();
                        break;
                    case Choice.DeleteTeacher:
                        TeacherService.RemoveTeacher();
                        break;
                    case Choice.UpdateTeacher:
                        TeacherService.UpdateTeacher();
                        break;
                    case Choice.GetDetailsOfTeacher:
                        TeacherService.GetTeacherDetails();
                        break;
                    case Choice.AddGroup:
                        GroupService.AddGroup();
                        break;
                    case Choice.GetAllGroups:
                        GroupService.GetAllGroups();
                        break;
                    case Choice.DeleteGroup:
                        GroupService.RemoveGroup();
                        break;
                    case Choice.UpdateGroup:
                        GroupService.UpdateGroup();
                        break;
                    case Choice.GetDetailsOfGroup:
                        GroupService.GetDetailOfGroup();
                        break;
                    case Choice.AddStudent:
                        StudentService.AddStudent();
                        break;
                    case Choice.GetAllStudents:
                        StudentService.GetAllStudents();
                        break;
                    case Choice.DeleteStudent:
                        StudentService.DeleteStudent();
                        break;
                    case Choice.UpdateStudent:
                        StudentService.UpdateStudent();
                        break;
                    case Choice.GetDetailsOfStudent:
                        StudentService.GetDetailsOfStudent();
                        break;
                    case Choice.Exit:
                        exit = false;
                        break;
                }
            }
            static void ShowMenu()
            {
                Console.WriteLine("0   Exit");
                Console.WriteLine("1   Add Teacher");
                Console.WriteLine("2   Delete Teacher");
                Console.WriteLine("3   Update Teacher");
                Console.WriteLine("4   Get Details Of Teacher");
                Console.WriteLine("5   Show all Teachers");
                Console.WriteLine("6   Add Group");
                Console.WriteLine("7   Show Groups");
                Console.WriteLine("8   Delete Group");
                Console.WriteLine("9   Update Group");
                Console.WriteLine("10  Get details of Group");
                Console.WriteLine("11  Add Student");
                Console.WriteLine("12  Delete Student");
                Console.WriteLine("13  Update Student");
                Console.WriteLine("14  Get Detail of Student");
                Console.WriteLine("15  Get All Students");
            }
        }
    }
}
