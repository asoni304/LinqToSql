using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace LinqToSql
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSQLDataClassesDataContext dataContext; 
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSql.Properties.Settings.AsoniDBConnectionString"].ConnectionString;

            dataContext = new LinqToSQLDataClassesDataContext(connectionString);

            //InsertStudentLectureAssociations();
            // GetCarlaLectures();
            //  GetAllStudentsFromYale();
            // GetAllUniWithFemales();
            //GetAllLecturesAtYale();
            //
            //UpdateCarla();
            DeleteLeni();
        }

        public void InsertUniversity()
        {
           

            University yale = new University();
            yale.Name = "Yale";
            dataContext.Universities.InsertOnSubmit(yale);

            University beijingTech = new University();
            beijingTech.Name = "Beijing Tech";
            dataContext.Universities.InsertOnSubmit(beijingTech);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudent() {

            University yale = dataContext.Universities.First(un => un.Name.Equals("Yale"));
            University beijingTech = dataContext.Universities.First(un => un.Name.Equals("Beijing Tech"));

            List<Student> students = new List<Student>();

            students.Add(new Student { Gender="Female", Name="Carla",UniversityId= yale.Id, });
            students.Add(new Student { Gender = "Male", Name = "Bazuu", UniversityId = beijingTech.Id, });
            students.Add(new Student { Gender = "Female", Name = "Antonia", University= beijingTech, });
            students.Add(new Student { Gender = "Female", Name = "Leni", University = yale, });


            dataContext.Students.InsertAllOnSubmit(students);

            dataContext.SubmitChanges();

            MainDataGrid2.ItemsSource = dataContext.Students;
        }

        public void InsertLecture()
        {
            List<Lecture> lectures = new List<Lecture>();

            lectures.Add(new Lecture { Name = "Math" });
            lectures.Add(new Lecture { Name = "IT" });
            lectures.Add(new Lecture { Name = "Languages" });
            lectures.Add(new Lecture { Name = "Technicals" });
            lectures.Add(new Lecture { Name = "Art" });
            lectures.Add(new Lecture { Name = "Music" });

            dataContext.Lectures.InsertAllOnSubmit(lectures);
            dataContext.SubmitChanges();
            MainDataGrid2.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations()
        {
            Student carla = dataContext.Students.First(st => st.Name.Equals("Carla"));
            Student leni = dataContext.Students.First(un => un.Name.Equals("Leni"));
            Student antonia = dataContext.Students.First(un => un.Name.Equals("Antonia"));
            Student bazuu = dataContext.Students.First(un => un.Name.Equals("Bazuu"));

            Lecture maths = dataContext.Lectures.First(le => le.Name.Equals("Math"));
            Lecture it = dataContext.Lectures.First(le => le.Name.Equals("IT"));
            Lecture languages = dataContext.Lectures.First(le => le.Name.Equals("Languages"));
            Lecture technicals = dataContext.Lectures.First(le => le.Name.Equals("Technicals"));
            Lecture art = dataContext.Lectures.First(le => le.Name.Equals("Art"));




            List<StudentLecture> studentLectures = new List<StudentLecture>();

            studentLectures.Add(new StudentLecture {StudentId=carla.Id ,LectureId= languages.Id});
            studentLectures.Add(new StudentLecture { StudentId = carla.Id, LectureId = it.Id });
            studentLectures.Add(new StudentLecture { StudentId = leni.Id, LectureId = languages.Id });
            studentLectures.Add(new StudentLecture { StudentId = leni.Id, LectureId = art.Id });
            studentLectures.Add(new StudentLecture { StudentId = antonia.Id, LectureId = maths.Id });
            studentLectures.Add(new StudentLecture { StudentId = antonia.Id, LectureId = technicals.Id });
            studentLectures.Add(new StudentLecture { StudentId = bazuu.Id, LectureId = languages.Id });

            dataContext.StudentLectures.InsertAllOnSubmit(studentLectures);

            dataContext.SubmitChanges();

            MainDataGrid2.ItemsSource = dataContext.StudentLectures;


        }

        public void GetCarlaUniversity()
        {
            Student carla = dataContext.Students.First(st => st.Name.Equals("Carla"));

            University carlaUni = carla.University;

            List<University> carlaUniversity = new List<University>();

            carlaUniversity.Add(carlaUni);

            dataContext.Universities.InsertAllOnSubmit(carlaUniversity);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource= dataContext.Universities; 
        }

        public void GetCarlaLectures()
        {
            Student carla = dataContext.Students.First(st => st.Name.Equals("Carla"));

            var carlaLectures = from le in carla.StudentLectures select le.Lecture;

            MainDataGrid2.ItemsSource = carlaLectures;
        }

        public void GetAllStudentsFromYale()
        {
            var studentsFromYale = from student in dataContext.Students
                                   where student.University.Name == "Yale"
                                   select student;

            MainDataGrid2.ItemsSource = studentsFromYale;
        }
        public void GetAllUniWithFemales()
        {
            var uniWithFemales = from student in dataContext.Students
                                 join university in dataContext.Universities
                                 on student.University equals university
                                 where student.Gender == "Female" 
                                 select university;

            MainDataGrid2.ItemsSource= uniWithFemales;
        }

        public void GetAllLecturesAtYale()
        {
            var allLecturesAtYale = from studentLecture in dataContext.StudentLectures
                                    join student in dataContext.Students
                                    on studentLecture.StudentId equals student.Id
                                    where student.University.Name == "Yale"
                                    select studentLecture.Lecture;

            MainDataGrid2.ItemsSource = allLecturesAtYale;
        }

        public void UpdateCarla()
        {
            Student carla = dataContext.Students.FirstOrDefault(st => st.Name.Equals("Carla"));

            carla.Name = "Carla Clotet";

            dataContext.SubmitChanges();

            MainDataGrid2.ItemsSource = dataContext.Students;

        }

        public void DeleteLeni()
        {
            Student leni = dataContext.Students.FirstOrDefault(st => st.Name =="Leni");

            dataContext.Students.DeleteOnSubmit(leni);
            dataContext.SubmitChanges();

            MainDataGrid2.ItemsSource = dataContext.Students;

        }
    }
}
