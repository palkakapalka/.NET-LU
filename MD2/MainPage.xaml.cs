using project;
using MD3;


namespace MD2
{
    public partial class MainPage : ContentPage

    {

        public MainPage()
        {
            InitializeComponent();
            LoadDataOnStartup(); 
        }

        /*        
       private DataManager _dataManager = new DataManager();
       private void OnGenerateDataClicked(object sender, EventArgs e)
       {
           _dataManager.Reset();
           _dataManager.CreateTestData(); // testa datu generacija
           OutputLabel.Text = _dataManager.Print(); 
       }

       private void OnViewDataClicked(object sender, EventArgs e)
       {
           OutputLabel.Text = _dataManager.Print(); // atminā datu izvade
       }

       private void OnLoadDataClicked(object sender, EventArgs e) // skatit datus no faila
       {
           string filePath = Path.Combine(@"C:\Temp\data.txt"); 
           _dataManager.Load(filePath); 
           OutputLabel.Text = "Dati ielādēti no faila.\n\n" + _dataManager.Print(); 

       }

       private void SaveDataInFile(object sender, EventArgs e) // saglabat datus faila
       {
           string filePath = Path.Combine(@"C:\Temp\data.txt");
           _dataManager.Load(filePath);
           _dataManager.Save(filePath);
       }
       */

        private void LoadDataOnStartup()
        {
            DatabaseManager dbManager = new DatabaseManager();
            //OutputLabel.Text += dbManager.TestConnection();
            //dbManager.ResetDataBase();
            //dbManager.GenerateTestData();
        }


        private DatabaseManager dbManager = new DatabaseManager();

        string emptyString = "";





        //////////////////////////////Teacher block////////////////////////////////////////////////

        /// Maina skolotāju sadaļas redzamību.
        private void OnToggleTeacherClicked(object sender, EventArgs e)
        {
            TeacherForm.IsVisible = !TeacherForm.IsVisible;
        }

        /// Maina skolotāju pievienošanas formas redzamību.
        private void OnToggleTeacherCreationClicked(object sender, EventArgs e)
        {
            TeacherCreationForm.IsVisible = !TeacherCreationForm.IsVisible;
        }

        /// Maina skolotāju rediģēšanas formas redzamību un ielādē pieejamos skolotājus.
        private void OnToggleTeacherUpdateClicked(object sender, EventArgs e)
        {
            TeacherUpdateForm.IsVisible = !TeacherUpdateForm.IsVisible;
            LoadTeachersIntoPicker();
        }

        /// Maina skolotāju dzēšanas formas redzamību un ielādē pieejamos skolotājus.
        private void OnToggleDeleteClicked(object sender, EventArgs e)
        {
            TeacherDeleteForm.IsVisible = !TeacherDeleteForm.IsVisible;
            LoadTeachersIntoPicker();
        }


        /// Apstrādā skolotāja pievienošanu, pārbaudot ievades datus un pievienojot tos datubāzei.
        private void OnAddTeacherClicked(object sender, EventArgs e)
        {
            // Iegūst datus no lietotāja ievades
            string TeacherName = TeacherNameEntry.Text;
            string TeacherSurname = TeacherSurnameEntry.Text;
            DateTime contractDate = ContractDatePicker.Date;
            string TeacherGender = TeacherGenderPicker?.SelectedItem?.ToString();

            // Pārbauda ievades datus
            if (string.IsNullOrWhiteSpace(TeacherName))
            {
                DisplayAlert("Validation Error", "Please enter the teacher's name.", "OK"); // Ziņojums par kļūdu
                return;
            }
            if (string.IsNullOrWhiteSpace(TeacherSurname))
            {
                DisplayAlert("Validation Error", "Please enter the teacher's surname.", "OK"); // Ziņojums par kļūdu
                return;
            }
            if (string.IsNullOrWhiteSpace(TeacherGender) ||
                (TeacherGender != "M" && TeacherGender != "F"))
            {
                DisplayAlert("Validation Error", "Please select a valid gender (M/F).", "OK"); // Ziņojums par kļūdu
                return;
            }
            if (contractDate > DateTime.Now)
            {
                DisplayAlert("Validation Error", "Contract date cannot be in the future.", "OK"); // Ziņojums par kļūdu
                return;
            }

            // Pievieno skolotāju, ja ievades dati ir korekti
            try
            {
                dbManager.AddTeacher(TeacherName, TeacherSurname, TeacherGender, contractDate);
                DisplayAlert("Success", "Teacher added successfully!", "OK"); // Ziņojums par veiksmīgu pievienošanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to add teacher: {ex.Message}", "OK"); // Ziņojums par kļūdu
            }
        }


        /// Ielādē skolotāju sarakstu no datubāzes un pievieno to izvēlnes komponentiem.
        private void LoadTeachersIntoPicker()
        {
            try
            {
                var teachers = dbManager.GetAllTeachers(); // Iegūst skolotāju sarakstu no datubāzes
                TeacherPicker.ItemsSource = teachers; // Pievieno skolotāju sarakstu kā datu avotu izvēlnei
                TeacherPickerForDelete.ItemsSource = teachers; // Pievieno skolotāju sarakstu dzēšanas izvēlnei
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK"); // Ziņojums par kļūdu
            }
        }


        /// Apstrādā skolotāja izvēli no saraksta un atjaunina kontrakta datumu.
        private void OnTeacherSelected(object sender, EventArgs e)
        {
            // Pārbauda, vai ir izvēlēts skolotājs
            if (TeacherPicker.SelectedIndex == -1)
            {
                return; // Ja nekas nav izvēlēts, nedara neko
            }

            // Iegūst izvēlēto skolotāju
            var selectedTeacher = dbManager.GetAllTeachers()[TeacherPicker.SelectedIndex]; // Vai cita metode, lai iegūtu skolotāju sarakstu

            // Atjaunina kontrakta datumu
            NewContractDatePicker.Date = selectedTeacher.ContractDate;
        }



        /// Apstrādā skolotāja informācijas rediģēšanu, validē ievades datus un atjaunina datus datu bāzē.
        private void OnEditTeacherClicked(object sender, EventArgs e)
        {
            // Pārbauda, vai TeacherPicker ir pieejams datu avots
            if (TeacherPicker.ItemsSource == null)
            {
                try
                {
                    // Iegūst visus skolotājus no datu bāzes un aizpilda TeacherPicker
                    var teachers = dbManager.GetAllTeachers();
                    TeacherPicker.ItemsSource = teachers.Select(t => $"{t.Id}: {t.Name} {t.Surname} {t.ContractDate}").ToList();
                }
                catch (Exception ex)
                {
                    // Parāda kļūdas paziņojumu, ja skolotāju ielāde neizdodas
                    DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK");
                    return;
                }
            }

            // Iegūst atlasīto skolotāju no TeacherPicker
            MD3.Teacher selectedTeacher = TeacherPicker.SelectedItem as MD3.Teacher;
            if (selectedTeacher == null)
            {
                DisplayAlert("Validation Error", "Please select a teacher.", "OK");
                return;
            }

            // Iegūst datus no ievades laukiem vai atstāj esošās vērtības
            string updatedName = string.IsNullOrWhiteSpace(TeacherNewNameEntry.Text)
                ? selectedTeacher.Name
                : TeacherNewNameEntry.Text;

            string updatedSurname = string.IsNullOrWhiteSpace(TeacherNewSurnameEntry.Text)
                ? selectedTeacher.Surname
                : TeacherNewSurnameEntry.Text;

            string updatedGender = TeacherNewGenderPicker.SelectedItem == null
                ? selectedTeacher.Gender
                : TeacherNewGenderPicker.SelectedItem.ToString();

            DateTime updatedContractDate = NewContractDatePicker.Date == default(DateTime)
                ? selectedTeacher.ContractDate
                : NewContractDatePicker.Date;

            // Validē ievades datus
            if (string.IsNullOrWhiteSpace(updatedName))
            {
                DisplayAlert("Validation Error", "Teacher's name cannot be empty.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(updatedSurname))
            {
                DisplayAlert("Validation Error", "Teacher's surname cannot be empty.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(updatedGender) || (updatedGender != "M" && updatedGender != "F"))
            {
                DisplayAlert("Validation Error", "Please select a valid gender (M/F).", "OK");
                return;
            }
            if (updatedContractDate > DateTime.Now)
            {
                DisplayAlert("Validation Error", "Contract date cannot be in the future.", "OK");
                return;
            }

            // Atjaunina skolotāja informāciju datu bāzē
            try
            {
                dbManager.UpdateTeacher(selectedTeacher.Id, updatedName, updatedSurname, updatedGender, updatedContractDate);
                DisplayAlert("Success", "Teacher updated successfully!", "OK");
            }
            catch (Exception ex)
            {
                // Parāda kļūdas paziņojumu, ja atjaunināšana neizdodas
                DisplayAlert("Error", $"Failed to update teacher: {ex.Message}", "OK");
            }
        }


        /// Dzēš atlasīto skolotāju no datu bāzes, validē izvēli un paziņo par rezultātu.
        private void OnDeleteTeacherClicked(object sender, EventArgs e)
        {
            // Pārbauda, vai TeacherPickerForDelete ir pieejams datu avots
            if (TeacherPickerForDelete.ItemsSource == null)
            {
                try
                {
                    // Iegūst visus skolotājus no datu bāzes un aizpilda TeacherPickerForDelete
                    var teachers = dbManager.GetAllTeachers();
                    TeacherPickerForDelete.ItemsSource = teachers.Select(t => $"{t.Id}: {t.Name} {t.Surname} {t.ContractDate}").ToList();
                }
                catch (Exception ex)
                {
                    // Parāda kļūdas paziņojumu, ja skolotāju ielāde neizdodas
                    DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK");
                    return;
                }
            }

            // Iegūst atlasīto skolotāju no TeacherPickerForDelete
            MD3.Teacher selectedTeacher = TeacherPickerForDelete.SelectedItem as MD3.Teacher;
            if (selectedTeacher == null)
            {
                DisplayAlert("Validation Error", "Please select a teacher.", "OK");
                return;
            }

            // Dzēš skolotāju no datu bāzes
            try
            {
                dbManager.DeleteTeacher(selectedTeacher.Id);
                DisplayAlert("Success", "Teacher deleted successfully!", "OK");
            }
            catch (Exception ex)
            {
                // Parāda kļūdas paziņojumu, ja dzēšana neizdodas
                DisplayAlert("Error", $"Failed to delete teacher: {ex.Message}", "OK");
            }
        }



        /// Parāda visu skolotāju sarakstu, iegūstot to no datu bāzes un attēlojot OutputLabel.
        private void TeacherList(object sender, EventArgs e)
        {
            // Attīra OutputLabel tekstu
            OutputLabel.Text = string.Empty;

            // Pievieno skolotāju sarakstu, izmantojot ShowTeacher funkciju no dbManager
            OutputLabel.Text += dbManager.ShowTeacher();
        }

        ///////////////////////////////Student block///////////////////////////////////////////////

        private void OnToggleStudentClicked(object sender, EventArgs e)
        {
            StudentForm.IsVisible = !StudentForm.IsVisible;
        }

        private void OnToggleStudentCreationClicked(object sender, EventArgs e)
        {
            StudentCreationForm.IsVisible = !StudentCreationForm.IsVisible;
        }

        private void OnToggleStudentUpdateClicked(object sender, EventArgs e)
        {
            StudentUpdateForm.IsVisible = !StudentUpdateForm.IsVisible;
            LoadStudentIntoPicker();
        }

        private void OnToggleStudentDeleteClicked(object sender, EventArgs e)
        {
            StudentDeleteForm.IsVisible = !StudentDeleteForm.IsVisible;
            LoadStudentIntoPicker();
        }

        private void StudentList(object sender, EventArgs e)
        {

            OutputLabel.Text = emptyString;
            OutputLabel.Text += dbManager.ShowStudent();
        }

        /// Pievieno jaunu studentu, pārbaudot ievadītos datus un saglabājot tos datu bāzē.
        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            // Iegūst ievades datus no interfeisa laukiem
            string StudentName = StudentNameEntry.Text;
            string StudentSurname = StudentSurnameEntry.Text;
            string StudentIdNumber = StudentIdNumberEntry.Text;
            string StudentGender = StudentGenderPicker?.SelectedItem?.ToString();

            // Pārbauda, vai ievades lauki ir aizpildīti un vai dati ir korekti
            if (string.IsNullOrWhiteSpace(StudentName))
            {
                DisplayAlert("Validation Error", "Please enter the student's name.", "OK"); // Paziņojums par kļūdu
                return;
            }
            if (string.IsNullOrWhiteSpace(StudentSurname))
            {
                DisplayAlert("Validation Error", "Please enter the student's surname.", "OK"); // Paziņojums par kļūdu
                return;
            }
            if (string.IsNullOrWhiteSpace(StudentGender) || (StudentGender != "M" && StudentGender != "F"))
            {
                DisplayAlert("Validation Error", "Please select a valid gender (M/F).", "OK"); // Paziņojums par kļūdu
                return;
            }

            // Ja visi dati ir korekti, pievieno studentu datu bāzei
            try
            {
                dbManager.AddStudent(StudentName, StudentSurname, StudentGender, StudentIdNumber);
                DisplayAlert("Success", "Student added successfully!", "OK"); // Ziņojums par veiksmīgu pievienošanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to add student: {ex.Message}", "OK"); // Paziņojums par kļūdu
            }
        }


        /// Ielādē studentu sarakstu Picker komponentos, lai tie būtu pieejami atlasīšanai.
        private void LoadStudentIntoPicker()
        {
            try
            {
                var students = dbManager.GetAllStudent(); // Iegūst studentu sarakstu no datu bāzes
                StudentPicker.ItemsSource = students; // Iestata StudentPicker datu avotu
                StudentPickerForDelete.ItemsSource = students; // Iestata StudentPickerForDelete datu avotu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load students: {ex.Message}", "OK"); // Paziņojums par kļūdu
            }
        }


        /// Apstrādā notikumu, kad tiek izvēlēts students no StudentPicker saraksta.
        private void OnStudentSelected(object sender, EventArgs e)
        {
            // Pārbauda, vai ir izvēlēts students
            if (StudentPicker.SelectedIndex == -1)
            {
                return; // Ja nekas nav izvēlēts, darbību neturpina
            }

            // Iegūst izvēlēto studentu no datu avota, izmantojot izvēlēto indeksu
            var selectedStudent = dbManager.GetAllStudent()[StudentPicker.SelectedIndex]; // Vai cita metode, lai iegūtu sarakstu
        }



        /// Apstrādā notikumu, kad tiek atjaunināta studenta informācija, un pārbauda ievades datu korektumu.
        private void OnEditStudentClicked(object sender, EventArgs e)
        {
            // Ja StudentPicker saraksts nav ielādēts, to aizpilda
            if (StudentPicker.ItemsSource == null)
            {
                try
                {
                    var students = dbManager.GetAllStudent(); // Iegūst visus studentus no datubāzes
                    StudentPicker.ItemsSource = students.Select(t => $"{t.Id}: {t.Name} {t.Surname} {t.StudentIdNumber}").ToList();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load students: {ex.Message}", "OK");
                    return;
                }
            }

            // Iegūst izvēlēto studentu no StudentPicker
            MD3.Student selectedStudent = StudentPicker.SelectedItem as MD3.Student;
            if (selectedStudent == null)
            {
                DisplayAlert("Validation Error", "Please select a Student.", "OK");
                return;
            }

            // Iegūst atjauninātos datus no ievades laukiem vai saglabā esošos
            string updatedName = string.IsNullOrWhiteSpace(StudentNewNameEntry.Text)
                ? selectedStudent.Name
                : StudentNewNameEntry.Text;

            string updatedSurname = string.IsNullOrWhiteSpace(StudentNewSurnameEntry.Text)
                ? selectedStudent.Surname
                : StudentNewSurnameEntry.Text;

            string updatedStudentIdNumber = string.IsNullOrWhiteSpace(NewStudentIdNumberEntry.Text)
                ? selectedStudent.StudentIdNumber
                : NewStudentIdNumberEntry.Text;

            string updatedGender = StudentNewGenderPicker.SelectedItem == null
                ? selectedStudent.Gender
                : StudentNewGenderPicker.SelectedItem.ToString();

            // Pārbauda ievades datu korektumu
            if (string.IsNullOrWhiteSpace(updatedName))
            {
                DisplayAlert("Validation Error", "Student's name cannot be empty.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(updatedSurname))
            {
                DisplayAlert("Validation Error", "Student's surname cannot be empty.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(updatedGender) || (updatedGender != "M" && updatedGender != "F"))
            {
                DisplayAlert("Validation Error", "Please select a valid gender (M/F).", "OK");
                return;
            }

            // Atjaunina studenta informāciju datubāzē
            try
            {
                dbManager.UpdateStudent(selectedStudent.Id, updatedName, updatedSurname, updatedGender, updatedStudentIdNumber);
                DisplayAlert("Success", "Student updated successfully!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update student: {ex.Message}", "OK");
            }
        }


        /// Apstrādā notikumu, kad tiek dzēsts students, pārbaudot un atjauninot sarakstu.
        private void OnDeleteStudentClicked(object sender, EventArgs e)
        {
            // Pārbauda un aizpilda StudentPickerForDelete, ja saraksts vēl nav ielādēts
            if (StudentPickerForDelete.ItemsSource == null)
            {
                try
                {
                    var students = dbManager.GetAllStudent(); // Iegūst visus studentus no datubāzes
                    StudentPickerForDelete.ItemsSource = students.Select(t => $"{t.Id}: {t.Name} {t.Surname} {t.StudentIdNumber}").ToList();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load students: {ex.Message}", "OK");
                    return;
                }
            }

            // Iegūst izvēlēto studentu no StudentPickerForDelete
            MD3.Student selectedStudent = StudentPickerForDelete.SelectedItem as MD3.Student;
            if (selectedStudent == null)
            {
                DisplayAlert("Validation Error", "Please select a student.", "OK");
                return;
            }

            // Dzēš izvēlēto studentu no datubāzes
            try
            {
                dbManager.DeleteStudent(selectedStudent.Id);
                DisplayAlert("Success", "Student deleted successfully!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to delete student: {ex.Message}", "OK");
            }
        }


        ///////////////////////////////Course block///////////////////////////////////////////////

        private void OnToggleCourseClicked(object sender, EventArgs e)
        {
            CourseForm.IsVisible = !CourseForm.IsVisible;
        }

        private void OnToggleCourseCreationClicked(object sender, EventArgs e)
        {
            CourseCreationForm.IsVisible = !CourseCreationForm.IsVisible;
            LoadTeachersForCourseIntoPicker();
        }

        private void OnToggleCourseUpdateClicked(object sender, EventArgs e)
        {
            CourseUpdateForm.IsVisible = !CourseUpdateForm.IsVisible;
            LoadCourseIntoPicker();
            LoadTeachersForCourseIntoPicker();
        }

        private void OnToggleCourseDeleteClicked(object sender, EventArgs e)
        {
            CourseDeleteForm.IsVisible = !CourseDeleteForm.IsVisible;
            LoadCourseIntoPicker();
        }

        private void CourseList(object sender, EventArgs e)
        {

            OutputLabel.Text = emptyString;
            OutputLabel.Text += dbManager.ShowCourse();
        }

        /// Apstrādā notikumu, kad tiek pievienots jauns kurss, pārbaudot un validējot ievades datus.
        private void OnAddCourseClicked(object sender, EventArgs e)
        {
            // Iegūst ievades datus no saskarnes
            string courseName = CourseNameEntry.Text;
            MD3.Teacher selectedTeacher = CourseTeacherPicker?.SelectedItem as MD3.Teacher;

            // Validē ievades datus
            if (string.IsNullOrWhiteSpace(courseName))
            {
                DisplayAlert("Validation Error", "Please enter the course name.", "OK"); // Kļūdas ziņojums
                return;
            }
            if (selectedTeacher == null)
            {
                DisplayAlert("Validation Error", "Please select a valid teacher.", "OK"); // Kļūdas ziņojums
                return;
            }

            // Ja dati ir pareizi, pievieno kursu
            try
            {
                dbManager.AddCourse(courseName, selectedTeacher.Id.ToString()); // Pievieno kursu ar izvēlēto pasniedzēju
                DisplayAlert("Success", "Course added successfully!", "OK"); // Ziņojums par veiksmīgu pievienošanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to add course: {ex.Message}", "OK"); // Kļūdas ziņojums
            }
        }


        /// Ielādē pasniedzēju sarakstu kursu izvēlnēs, nodrošinot datus jauniem un esošiem kursiem.
        private void LoadTeachersForCourseIntoPicker()
        {
            try
            {
                var teachers = dbManager.GetAllTeachers(); // Iegūst pasniedzēju sarakstu no datu bāzes
                CourseTeacherPicker.ItemsSource = teachers; // Iestata datus kā avotu jauna kursa izvēlnei
                CourseTeacherPickerForUpdate.ItemsSource = teachers; // Iestata datus kā avotu esoša kursa atjaunināšanas izvēlnei
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK"); // Kļūdas paziņojums
            }
        }

        /// Ielādē kursu sarakstu izvēlnēs, nodrošinot datus jauniem un esošiem kursiem.
        private void LoadCourseIntoPicker()
        {
            try
            {
                var courses = dbManager.GetAllCourse(); // Iegūst kursu sarakstu no datu bāzes
                CoursePicker.ItemsSource = courses; // Iestata datus kā avotu kursa atjaunināšanas izvēlnei
                CoursePickerForDelete.ItemsSource = courses; // Iestata datus kā avotu kursa dzēšanas izvēlnei
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK"); // Kļūdas paziņojums
            }
        }


        /// Apstrādā lietotāja izvēli kursa izvēlnē un ielādē saistīto informāciju.
        private void OnCourseSelected(object sender, EventArgs e)
        {
            // Pārbauda, vai lietotājs ir izvēlējies kursu
            if (CoursePicker.SelectedIndex == -1)
            {
                return; // Ja nekas nav izvēlēts, funkcija pārtrauc darbību
            }

            // Iegūst izvēlēto kursu no datu bāzes, pamatojoties uz izvēlnes indeksu
            var selectedCourse = dbManager.GetAllCourse()[CoursePicker.SelectedIndex];

            // Šeit var pievienot papildu loģiku, piemēram, rādīt kursa informāciju vai ielādēt saistītos datus
        }



        /// Apstrādā lietotāja pieprasījumu rediģēt kursa informāciju un atjaunina datus datu bāzē.
        private void OnEditCourseClicked(object sender, EventArgs e)
        {
            // Ja CoursePicker vēl nav aizpildīts ar datiem, to aizpilda
            if (CoursePicker.ItemsSource == null)
            {
                try
                {
                    var courses = dbManager.GetAllCourse(); // Iegūst visus kursus no datu bāzes
                    CoursePicker.ItemsSource = courses; // Piešķir kursu sarakstu kā datu avotu
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu
                    return;
                }
            }

            // Iegūst izvēlēto kursu no CoursePicker
            MD3.Course selectedCourse = CoursePicker.SelectedItem as MD3.Course;
            if (selectedCourse == null)
            {
                DisplayAlert("Validation Error", "Please select a course.", "OK"); // Parāda kļūdas paziņojumu, ja nav izvēlēts kurss
                return;
            }

            // Ja CourseTeacherPickerForUpdate nav aizpildīts ar datiem, to aizpilda
            if (CourseTeacherPickerForUpdate.ItemsSource == null)
            {
                try
                {
                    var teachers = dbManager.GetAllTeachers(); // Iegūst visus skolotājus no datu bāzes
                    CourseTeacherPickerForUpdate.ItemsSource = teachers; // Piešķir skolotāju sarakstu kā datu avotu
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu
                    return;
                }
            }

            // Iegūst izvēlēto skolotāju no CourseTeacherPickerForUpdate
            MD3.Teacher selectedTeacher = CourseTeacherPickerForUpdate.SelectedItem as MD3.Teacher;

            // Iegūst datus no interfeisa vai saglabā esošās vērtības
            string updatedName = string.IsNullOrWhiteSpace(CourseNewNameEntry.Text)
                ? selectedCourse.Name
                : CourseNewNameEntry.Text;

            int updatedTeacherId = selectedTeacher != null
                ? selectedTeacher.Id
                : selectedCourse.TeacherId;

            // Validē ievadītos datus
            if (string.IsNullOrWhiteSpace(updatedName))
            {
                DisplayAlert("Validation Error", "Course name cannot be empty.", "OK"); // Parāda kļūdas paziņojumu
                return;
            }

            // Atjaunina kursa informāciju datu bāzē
            try
            {
                dbManager.UpdateCourse(selectedCourse.Id, updatedName, updatedTeacherId); // Izsauc datu bāzes metodi, lai atjauninātu kursa informāciju
                DisplayAlert("Success", "Course updated successfully!", "OK"); // Parāda paziņojumu par veiksmīgu atjaunināšanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update course: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu
            }
        }



        /// Apstrādā lietotāja pieprasījumu dzēst kursu un atjaunina pieejamo kursu sarakstu.
        private void OnDeleteCourseClicked(object sender, EventArgs e)
        {
            // Pārbauda, vai CoursePickerForDelete ir aizpildīts ar datiem, ja nav, tad to aizpilda
            if (CoursePickerForDelete.ItemsSource == null)
            {
                try
                {
                    var courses = dbManager.GetAllCourse(); // Iegūst visus kursus no datu bāzes
                    CoursePickerForDelete.ItemsSource = courses; // Piešķir kursu sarakstu kā datu avotu
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu
                    return;
                }
            }

            // Iegūst izvēlēto kursu no CoursePickerForDelete
            MD3.Course selectedCourse = CoursePickerForDelete.SelectedItem as MD3.Course;
            if (selectedCourse == null)
            {
                DisplayAlert("Validation Error", "Please select a course to delete.", "OK"); // Parāda kļūdas paziņojumu, ja kurss nav izvēlēts
                return;
            }

            // Dzēš izvēlēto kursu no datu bāzes
            try
            {
                dbManager.DeleteCourse(selectedCourse.Id); // Dzēš kursu, izmantojot datu bāzes metodi
                DisplayAlert("Success", "Course deleted successfully!", "OK"); // Parāda paziņojumu par veiksmīgu dzēšanu

                // Atjaunina kursu sarakstu CoursePickerForDelete
                var updatedCourses = dbManager.GetAllCourse(); // Iegūst atjaunotu kursu sarakstu
                CoursePickerForDelete.ItemsSource = updatedCourses; // Atjaunina datu avotu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to delete course: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu
            }
        }


        ////////////////////////////   Assignment Block  /////////////////////

        private void OnToggleAssignmentClicked(object sender, EventArgs e)
        {
            AssignmentForm.IsVisible = !AssignmentForm.IsVisible;
        }

        private void OnToggleAssignmentCreationClicked(object sender, EventArgs e)
        {
            AssignmentCreationForm.IsVisible = !AssignmentCreationForm.IsVisible;
            LoadCourseForAssignmentIntoPicker();
        }

        private void OnToggleAssignmentUpdateClicked(object sender, EventArgs e)
        {
            AssignmentUpdateForm.IsVisible = !AssignmentUpdateForm.IsVisible;
            LoadAssignmentsIntoPicker();
            LoadCourseForAssignmentIntoPicker();
        }

        private void OnToggleAssignmentDeleteClicked(object sender, EventArgs e)
        {
            AssignmentDeleteForm.IsVisible = !AssignmentDeleteForm.IsVisible;
            LoadAssignmentsIntoPicker();
        }

        private void AssignmentList(object sender, EventArgs e)
        {
            OutputLabel.Text = string.Empty;
            OutputLabel.Text += dbManager.ShowAssignments();
        }

        private void OnToggleCoursesAssignmentClicked(object sender, EventArgs e)
        {
            CoursesAssignments.IsVisible = !CoursesAssignments.IsVisible;
            LoadCourseForAssignmentIntoPicker();
        }

        /// Parāda izvēlētā kursa uzdevumus un tos attēlo lietotāja interfeisā.
        private void ShowCoursesAssignment(object sender, EventArgs e)
        {
            OutputLabel.Text = string.Empty; // Notīra esošo tekstu no OutputLabel

            // Pārbauda, vai kurss ir izvēlēts
            MD3.Course selectedCourse = CoursesAssignmenPicker.SelectedItem as MD3.Course;

            if (selectedCourse == null)
            {
                DisplayAlert("Validation Error", "Please select a course to view assignments.", "OK"); // Parāda kļūdas paziņojumu
                return;
            }

            try
            {
                // Iegūst izvēlētā kursa uzdevumu informāciju
                string assignmentsInfo = dbManager.ShowAssignmentsByCourse(selectedCourse.Id);

                // Attēlo uzdevumu informāciju OutputLabel
                OutputLabel.Text = assignmentsInfo;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load assignments: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu, ja rodas problēma
            }
        }



        /// Izveido jaunu uzdevumu un pievieno to izvēlētajam kursam.
        private void OnAddAssignmentClicked(object sender, EventArgs e)
        {
            string assignmentDescription = AssignmentDescriptionEntry.Text; // Iegūst uzdevuma aprakstu no ievades lauka
            DateTime deadline = AssignmentDeadlinePicker.Date; // Iegūst uzdevuma termiņu no datuma atlasītāja
            MD3.Course selectedCourse = AssignmentCoursePicker?.SelectedItem as MD3.Course; // Iegūst atlasīto kursu no izvēles saraksta

            // Validē ievades datus
            if (string.IsNullOrWhiteSpace(assignmentDescription))
            {
                DisplayAlert("Validation Error", "Please enter the assignment description.", "OK"); // Parāda kļūdas paziņojumu
                return;
            }
            if (selectedCourse == null)
            {
                DisplayAlert("Validation Error", "Please select a valid course.", "OK"); // Parāda kļūdas paziņojumu
                return;
            }
            if (deadline < DateTime.Now)
            {
                DisplayAlert("Validation Error", "Deadline cannot be in the past.", "OK"); // Parāda kļūdas paziņojumu
                return;
            }

            try
            {
                // Pievieno jauno uzdevumu datu bāzei
                dbManager.AddAssignment(assignmentDescription, deadline, selectedCourse.Id);
                DisplayAlert("Success", "Assignment added successfully!", "OK"); // Parāda paziņojumu par veiksmīgu pievienošanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to add assignment: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu, ja rodas problēma
            }
        }


        /// Ielādē visus uzdevumus izvēles sarakstā.
        private void LoadAssignmentsIntoPicker()
        {
            try
            {
                var assignments = dbManager.GetAllAssignments(); // Iegūst visus uzdevumus no datu bāzes
                AssignmentPicker.ItemsSource = assignments; // Uzstāda izvēles sarakstu ar uzdevumiem rediģēšanai
                AssignmentPickerForDelete.ItemsSource = assignments; // Uzstāda izvēles sarakstu ar uzdevumiem dzēšanai
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load assignments: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu, ja rodas problēma
            }
        }


        /// Ielādē visus kursus uzdevumu saistīšanai izvēles sarakstā.
        private void LoadCourseForAssignmentIntoPicker()
        {
            try
            {
                var courses = dbManager.GetAllCourse(); // Iegūst visus kursus no datu bāzes
                AssignmentCoursePicker.ItemsSource = courses; // Iestata izvēles sarakstu kursu pievienošanai uzdevumam
                AssignmentNewCoursePicker.ItemsSource = courses; // Iestata izvēles sarakstu kursu rediģēšanai uzdevumam
                CoursesAssignmenPicker.ItemsSource = courses; // Iestata izvēles sarakstu kursu uzdevumu pārskatīšanai
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK"); // Parāda kļūdas paziņojumu, ja rodas problēma
            }
        }


        /// Apstrādā notikumu, kad lietotājs izvēlas uzdevumu no saraksta.
        private void OnAssignmentSelected(object sender, EventArgs e)
        {
            // Pārbauda, vai ir izvēlēts uzdevums
            if (AssignmentPicker.SelectedIndex == -1)
            {
                return; // Ja nekas nav izvēlēts, darbība netiek veikta
            }

            // Iegūst izvēlēto uzdevumu no datu bāzes
            var selectedAssignment = dbManager.GetAllAssignments()[AssignmentPicker.SelectedIndex]; // Iegūst saraksta elementu

            // Iestata uzdevuma termiņu datuma atlasītājā
            AssignmentNewDeadlinePicker.Date = selectedAssignment.Deadline;
        }


        /// Apstrādā uzdevuma rediģēšanas pieprasījumu no lietotāja.
        private void OnEditAssignmentClicked(object sender, EventArgs e)
        {
            // Iegūst izvēlēto uzdevumu no saraksta
            MD3.Assignment selectedAssignment = AssignmentPicker.SelectedItem as MD3.Assignment;
            if (selectedAssignment == null)
            {
                DisplayAlert("Validation Error", "Please select an assignment.", "OK"); // Brīdinājums, ja uzdevums nav izvēlēts
                return;
            }

            // Iegūst jauno uzdevuma aprakstu vai atstāj esošo
            string updatedDescription = string.IsNullOrWhiteSpace(AssignmentNewDescriptionEntry.Text)
                ? selectedAssignment.Description
                : AssignmentNewDescriptionEntry.Text;

            // Iegūst jauno termiņu vai atstāj esošo
            DateTime updatedDeadline = AssignmentNewDeadlinePicker.Date == default
                ? selectedAssignment.Deadline
                : AssignmentNewDeadlinePicker.Date;

            // Iegūst jauno kursa ID vai atstāj esošo
            int updatedCourseId = AssignmentNewCoursePicker.SelectedItem != null
                ? (AssignmentNewCoursePicker.SelectedItem as MD3.Course).Id
                : selectedAssignment.CourseId;

            // Pārbauda, vai dati ir derīgi
            if (string.IsNullOrWhiteSpace(updatedDescription))
            {
                DisplayAlert("Validation Error", "Description cannot be empty.", "OK"); // Brīdinājums, ja apraksts ir tukšs
                return;
            }
            if (updatedDeadline < DateTime.Now)
            {
                DisplayAlert("Validation Error", "Deadline cannot be in the past.", "OK"); // Brīdinājums, ja termiņš ir pagātnē
                return;
            }

            // Atjaunina uzdevumu datu bāzē
            try
            {
                dbManager.UpdateAssignment(selectedAssignment.Id, updatedDescription, updatedDeadline, updatedCourseId);
                DisplayAlert("Success", "Assignment updated successfully!", "OK"); // Ziņojums par veiksmīgu atjaunināšanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update assignment: {ex.Message}", "OK"); // Ziņojums par kļūdu
            }
        }


        /// Apstrādā uzdevuma dzēšanas pieprasījumu no lietotāja.
        private void OnDeleteAssignmentClicked(object sender, EventArgs e)
        {
            // Iegūst izvēlēto uzdevumu no dzēšanas saraksta
            MD3.Assignment selectedAssignment = AssignmentPickerForDelete.SelectedItem as MD3.Assignment;
            if (selectedAssignment == null)
            {
                DisplayAlert("Validation Error", "Please select an assignment to delete.", "OK"); // Brīdinājums, ja uzdevums nav izvēlēts
                return;
            }

            // Veic uzdevuma dzēšanu
            try
            {
                dbManager.DeleteAssignment(selectedAssignment.Id);
                DisplayAlert("Success", "Assignment deleted successfully!", "OK"); // Ziņojums par veiksmīgu dzēšanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to delete assignment: {ex.Message}", "OK"); // Ziņojums par kļūdu dzēšanā
            }
        }


        ///////////////////////////// Submission block /////////////////////////////////////////////

        private void OnToggleSubmissionClicked(object sender, EventArgs e)
        {
            SubmissionForm.IsVisible = !SubmissionForm.IsVisible;
        }

        private void OnToggleSubmissionCreationClicked(object sender, EventArgs e)
        {
            SubmissionCreationForm.IsVisible = !SubmissionCreationForm.IsVisible;
            LoadAssignmentsForSubmissionPicker();
            LoadStudentsForSubmissionPicker();
        }

        private void OnToggleSubmissionUpdateClicked(object sender, EventArgs e)
        {
            SubmissionUpdateForm.IsVisible = !SubmissionUpdateForm.IsVisible;
            LoadSubmissionsIntoPicker();
            LoadAssignmentsForSubmissionPicker();
            LoadStudentsForSubmissionPicker();
        }

        private void OnToggleSubmissionDeleteClicked(object sender, EventArgs e)
        {
            SubmissionDeleteForm.IsVisible = !SubmissionDeleteForm.IsVisible;
            LoadSubmissionsIntoPicker();
        }

        private void SubmissionList(object sender, EventArgs e)
        {
            OutputLabel.Text = string.Empty;
            OutputLabel.Text += dbManager.ShowSubmission();
        }

        /// Apstrādā jauna iesnieguma pievienošanas pieprasījumu no lietotāja.
        private void OnAddSubmissionClicked(object sender, EventArgs e)
        {
            // Iegūst izvēlēto studentu un uzdevumu no Picker
            MD3.Student selectedStudent = SubmissionStudentPicker?.SelectedItem as MD3.Student;
            MD3.Assignment selectedAssignment = SubmissionAssignmentPicker?.SelectedItem as MD3.Assignment;

            // Pārbauda un pārvērš ievadīto vērtējumu
            decimal score = 0;
            if (!decimal.TryParse(SubmissionScoreEntry.Text, out score) || score < 0 || score > 100)
            {
                DisplayAlert("Validation Error", "Score must be a number between 0 and 100.", "OK"); // Brīdinājums par nederīgu vērtējumu
                return;
            }

            // Pārbauda, vai ir izvēlēts students un uzdevums
            if (selectedStudent == null)
            {
                DisplayAlert("Validation Error", "Please select a valid student.", "OK"); // Brīdinājums, ja students nav izvēlēts
                return;
            }
            if (selectedAssignment == null)
            {
                DisplayAlert("Validation Error", "Please select a valid assignment.", "OK"); // Brīdinājums, ja uzdevums nav izvēlēts
                return;
            }

            // Pievieno iesniegumu
            try
            {
                dbManager.AddSubmission(selectedAssignment.Id, selectedStudent.Id, DateTime.Now, score);
                DisplayAlert("Success", "Submission added successfully!", "OK"); // Ziņojums par veiksmīgu pievienošanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to add submission: {ex.Message}", "OK"); // Ziņojums par kļūdu pievienošanā
            }
        }


        /// Apstrādā iesnieguma rediģēšanas pieprasījumu no lietotāja.
        private void OnEditSubmissionClicked(object sender, EventArgs e)
        {
            // Iegūst izvēlēto iesniegumu no Picker
            MD3.Submission selectedSubmission = SubmissionPicker.SelectedItem as MD3.Submission;
            if (selectedSubmission == null)
            {
                DisplayAlert("Validation Error", "Please select a valid submission.", "OK"); // Brīdinājums, ja iesniegums nav izvēlēts
                return;
            }

            // Iegūst izvēlēto uzdevumu un studentu vai izmanto esošās vērtības
            MD3.Assignment selectedAssignment = SubmissionNewAssignmentPicker.SelectedItem as MD3.Assignment;
            MD3.Student selectedStudent = SubmissionNewStudentPicker.SelectedItem as MD3.Student;

            // Pārbauda un pārvērš ievadīto vērtējumu
            decimal score = 0;
            if (!decimal.TryParse(SubmissionNewScoreEntry.Text, out score) || score < 0 || score > 100)
            {
                DisplayAlert("Validation Error", "Score must be a number between 0 and 100.", "OK"); // Brīdinājums par nederīgu vērtējumu
                return;
            }

            // Atjaunina iesnieguma informāciju
            try
            {
                dbManager.UpdateSubmission(
                    selectedSubmission.Id,
                    selectedAssignment?.Id ?? selectedSubmission.AssignmentId, // Ja nav izvēlēts, izmanto esošo uzdevumu
                    selectedStudent?.Id ?? selectedSubmission.StudentId, // Ja nav izvēlēts, izmanto esošo studentu
                    DateTime.Now, // Atjaunina iesniegšanas laiku uz pašreizējo
                    score // Jaunais vērtējums
                );
                DisplayAlert("Success", "Submission updated successfully!", "OK"); // Ziņojums par veiksmīgu atjaunināšanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update submission: {ex.Message}", "OK"); // Ziņojums par kļūdu atjaunināšanā
            }
        }


        /// Apstrādā iesnieguma dzēšanas pieprasījumu no lietotāja.
        private void OnDeleteSubmissionClicked(object sender, EventArgs e)
        {
            // Iegūst izvēlēto iesniegumu no dzēšanas Picker
            MD3.Submission selectedSubmission = SubmissionPickerForDelete.SelectedItem as MD3.Submission;
            if (selectedSubmission == null)
            {
                DisplayAlert("Validation Error", "Please select a valid submission to delete.", "OK"); // Brīdinājums, ja iesniegums nav izvēlēts
                return;
            }

            // Veic iesnieguma dzēšanu
            try
            {
                dbManager.DeleteSubmission(selectedSubmission.Id); // Dzēš iesniegumu no datu bāzes
                DisplayAlert("Success", "Submission deleted successfully!", "OK"); // Ziņojums par veiksmīgu dzēšanu
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to delete submission: {ex.Message}", "OK"); // Ziņojums par kļūdu dzēšanā
            }
        }


        /// Ielādē uzdevumus iesniegumu veidošanas un rediģēšanas izvēlnēm.
        private void LoadAssignmentsForSubmissionPicker()
        {
            try
            {
                var assignments = dbManager.GetAllAssignments(); // Iegūst visus uzdevumus no datu bāzes
                SubmissionAssignmentPicker.ItemsSource = assignments; // Uzstāda uzdevumus kā datu avotu iesniegumu veidošanas izvēlnei
                SubmissionNewAssignmentPicker.ItemsSource = assignments; // Uzstāda uzdevumus kā datu avotu iesniegumu rediģēšanas izvēlnei
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load assignments: {ex.Message}", "OK"); // Ziņojums par kļūdu uzdevumu ielādē
            }
        }


        /// Apstrādā izvēlēto iesniegumu, ielādē tā informāciju rediģēšanai.
        private void OnSubmissionSelected(object sender, EventArgs e)
        {
            // Pārbauda, vai ir izvēlēts iesniegums
            if (SubmissionPicker.SelectedIndex == -1)
            {
                return; // Ja nekas nav izvēlēts, funkcija pārtrauc darbību
            }

            // Iegūst izvēlēto iesniegumu no datu bāzes
            var selectedSubmission = dbManager.GetAllSubmissions()[SubmissionPicker.SelectedIndex]; // Iegūst iesniegumu pēc indeksa

            // Uzstāda izvēlētā iesnieguma laiku un rezultātu rediģēšanai
            SubmissionNewTimePicker.Date = selectedSubmission.SubmissionTime;
            SubmissionNewScoreEntry.Text = selectedSubmission.Score.ToString();
        }


        /// Ielādē studentu sarakstu, lai tos izmantotu iesniegumu pievienošanā vai rediģēšanā.
        private void LoadStudentsForSubmissionPicker()
        {
            try
            {
                // Iegūst visus studentus no datu bāzes
                var students = dbManager.GetAllStudent();

                // Uzstāda iegūtos studentus kā datu avotu izvēles sarakstiem
                SubmissionStudentPicker.ItemsSource = students;
                SubmissionNewStudentPicker.ItemsSource = students;
            }
            catch (Exception ex)
            {
                // Parāda kļūdas paziņojumu, ja studentu ielādēšana neizdevās
                DisplayAlert("Error", $"Failed to load students: {ex.Message}", "OK");
            }
        }


        /// Ielādē iesniegumu sarakstu, lai tos izmantotu rediģēšanā vai dzēšanā.
        private void LoadSubmissionsIntoPicker()
        {
            try
            {
                // Iegūst visus iesniegumus no datu bāzes
                var submissions = dbManager.GetAllSubmissions();

                // Uzstāda iegūtos iesniegumus kā datu avotu izvēles sarakstiem
                SubmissionPicker.ItemsSource = submissions;
                SubmissionPickerForDelete.ItemsSource = submissions;
            }
            catch (Exception ex)
            {
                // Parāda kļūdas paziņojumu, ja iesniegumu ielādēšana neizdevās
                DisplayAlert("Error", $"Failed to load submissions: {ex.Message}", "OK");
            }
        }


    }
}
