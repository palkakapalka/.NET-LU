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

        private void OnToggleTeacherClicked(object sender, EventArgs e)
        {
            TeacherForm.IsVisible = !TeacherForm.IsVisible;
        }

        private void OnToggleTeacherCreationClicked(object sender, EventArgs e)
        {
            TeacherCreationForm.IsVisible = !TeacherCreationForm.IsVisible;
        }

        private void OnToggleTeacherUpdateClicked(object sender, EventArgs e)
        {
            TeacherUpdateForm.IsVisible = !TeacherUpdateForm.IsVisible;
            LoadTeachersIntoPicker();
        }

        private void OnToggleDeleteClicked(object sender, EventArgs e)
        {
            TeacherDeleteForm.IsVisible = !TeacherDeleteForm.IsVisible;
            LoadTeachersIntoPicker();
        }

        // Teacher izveidošana
        private void OnAddTeacherClicked(object sender, EventArgs e)
        {
            // Получаем данные из интерфейса
            string TeacherName = TeacherNameEntry.Text;
            string TeacherSurname = TeacherSurnameEntry.Text;
            DateTime contractDate = ContractDatePicker.Date;
            string TeacherGender = TeacherGenderPicker?.SelectedItem?.ToString();

            // Проверяем, что данные заполнены корректно
            if (string.IsNullOrWhiteSpace(TeacherName))
            {
                DisplayAlert("Validation Error", "Please enter the teacher's name.", "OK"); // Сообщение об ошибке
                return;
            }
            if (string.IsNullOrWhiteSpace(TeacherSurname))
            {
                DisplayAlert("Validation Error", "Please enter the teacher's surname.", "OK"); // Сообщение об ошибке
                return;
            }
            if (string.IsNullOrWhiteSpace(TeacherGender) ||
                (TeacherGender != "M" && TeacherGender != "F"))
            {
                DisplayAlert("Validation Error", "Please select a valid gender (M/F).", "OK"); // Сообщение об ошибке
                return;
            }
            if (contractDate > DateTime.Now)
            {
                DisplayAlert("Validation Error", "Contract date cannot be in the future.", "OK"); // Сообщение об ошибке
                return;
            }

            // Если все данные корректны, добавляем учителя
            try
            {
                dbManager.AddTeacher(TeacherName, TeacherSurname, TeacherGender, contractDate);
                DisplayAlert("Success", "Teacher added successfully!", "OK"); // Сообщение об успешной операции
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to add teacher: {ex.Message}", "OK"); // Сообщение об ошибке
            }
        }

        private void LoadTeachersIntoPicker()
        {
            try
            {
                var teachers = dbManager.GetAllTeachers(); // Получаем список учителей из базы данных
                TeacherPicker.ItemsSource = teachers; // Устанавливаем список в качестве источника данных
                TeacherPickerForDelete.ItemsSource = teachers;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK"); // Уведомление об ошибке
            }
        }

        private void OnTeacherSelected(object sender, EventArgs e)
        {
            // Проверяем, что выбран учитель
            if (TeacherPicker.SelectedIndex == -1)
            {
                return; // Если ничего не выбрано, ничего не делаем
            }

            // Получаем выбранного учителя
            var selectedTeacher = dbManager.GetAllTeachers()[TeacherPicker.SelectedIndex]; // Или другой метод для получения списка учителей

            // Устанавливаем дату контракта в DatePicker
            NewContractDatePicker.Date = selectedTeacher.ContractDate;
        }


        private void OnEditTeacherClicked(object sender, EventArgs e)
        {
            // Загружаем список учителей в Picker, если он еще не заполнен
            if (TeacherPicker.ItemsSource == null)
            {
                try
                {
                    var teachers = dbManager.GetAllTeachers(); // Предполагаемая функция для получения всех учителей
                    TeacherPicker.ItemsSource = teachers.Select(t => $"{t.Id}: {t.Name} {t.Surname} {t.ContractDate}").ToList();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK");
                    return;
                }
            }

            // Получаем выбранного учителя
            MD3.Teacher selectedTeacher = TeacherPicker.SelectedItem as MD3.Teacher;
            if (selectedTeacher == null)
            {
                DisplayAlert("Validation Error", "Please select a teacher.", "OK");
                return;
            }

            // Получаем данные из интерфейса (или оставляем текущие значения)
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

            // Проверяем данные на корректность
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

            // Обновляем информацию об учителе
            try
            {
                dbManager.UpdateTeacher(selectedTeacher.Id, updatedName, updatedSurname, updatedGender, updatedContractDate);
                DisplayAlert("Success", "Teacher updated successfully!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update teacher: {ex.Message}", "OK");
            }
        }

        private void OnDeleteTeacherClicked(object sender, EventArgs e)
        {
            // Загружаем список учителей в Picker, если он еще не заполнен
            if (TeacherPickerForDelete.ItemsSource == null)
            {
                try
                {
                    var teachers = dbManager.GetAllTeachers(); // Предполагаемая функция для получения всех учителей
                    TeacherPickerForDelete.ItemsSource = teachers.Select(t => $"{t.Id}: {t.Name} {t.Surname} {t.ContractDate}").ToList();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK");
                    return;
                }
            }

            // Получаем выбранного учителя
            MD3.Teacher selectedTeacher = TeacherPickerForDelete.SelectedItem as MD3.Teacher;
            if (selectedTeacher == null)
            {
                DisplayAlert("Validation Error", "Please select a teacher.", "OK");
                return;
            }

            // Обновляем информацию об учителе
            try
            {
                dbManager.DeleteTeacher(selectedTeacher.Id);
                DisplayAlert("Success", "Teacher deleted successfully!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update teacher: {ex.Message}", "OK");
            }
        }


        private void TeacherList(object sender, EventArgs e)
        {
            
            OutputLabel.Text = emptyString;
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

        // Student izveidošana
        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            // Получаем данные из интерфейса
            string StudentName = StudentNameEntry.Text;
            string StudentSurname = StudentSurnameEntry.Text;
            string StudentIdNumber = StudentIdNumberEntry.Text;
            string StudentGender = StudentGenderPicker?.SelectedItem?.ToString();

            // Проверяем, что данные заполнены корректно
            if (string.IsNullOrWhiteSpace(StudentName))
            {
                DisplayAlert("Validation Error", "Please enter the teacher's name.", "OK"); // Сообщение об ошибке
                return;
            }
            if (string.IsNullOrWhiteSpace(StudentSurname))
            {
                DisplayAlert("Validation Error", "Please enter the teacher's surname.", "OK"); // Сообщение об ошибке
                return;
            }
            if (string.IsNullOrWhiteSpace(StudentGender) ||
                (StudentGender != "M" && StudentGender != "F"))
            {
                DisplayAlert("Validation Error", "Please select a valid gender (M/F).", "OK"); // Сообщение об ошибке
                return;
            }
            if (string.IsNullOrWhiteSpace(StudentGender))
            {
                DisplayAlert("Validation Error", "Please select a valid gender (M/F).", "OK"); // Сообщение об ошибке
                return;
            }


            // Если все данные корректны, добавляем учителя
            try
            {
                dbManager.AddStudent(StudentName, StudentSurname, StudentGender, StudentIdNumber);
                DisplayAlert("Success", "Teacher added successfully!", "OK"); // Сообщение об успешной операции
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to add stydent: {ex.Message}", "OK"); // Сообщение об ошибке
            }
        }

        private void LoadStudentIntoPicker()
        {
            try
            {
                var teachers = dbManager.GetAllStudent(); // Получаем список учителей из базы данных
                StudentPicker.ItemsSource = teachers; // Устанавливаем список в качестве источника данных
                StudentPickerForDelete.ItemsSource = teachers;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK"); // Уведомление об ошибке
            }
        }

        private void OnStudentSelected(object sender, EventArgs e)
        {
            // Проверяем, что выбран учитель
            if (StudentPicker.SelectedIndex == -1)
            {
                return; // Если ничего не выбрано, ничего не делаем
            }

            // Получаем выбранного учителя
            var selectedStudent = dbManager.GetAllStudent()[StudentPicker.SelectedIndex]; // Или другой метод для получения списка учителей

        }


        private void OnEditStudentClicked(object sender, EventArgs e)
        {
            // Загружаем список учителей в Picker, если он еще не заполнен
            if (StudentPicker.ItemsSource == null)
            {
                try
                {
                    var students = dbManager.GetAllStudent(); // Предполагаемая функция для получения всех учителей
                    StudentPicker.ItemsSource = students.Select(t => $"{t.Id}: {t.Name} {t.Surname} {t.StudentIdNumber}").ToList();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK");
                    return;
                }
            }

            // Получаем выбранного учителя
            MD3.Student selectedStudent = StudentPicker.SelectedItem as MD3.Student;
            if (selectedStudent == null)
            {
                DisplayAlert("Validation Error", "Please select a teacher.", "OK");
                return;
            }

            // Получаем данные из интерфейса (или оставляем текущие значения)
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


            // Проверяем данные на корректность
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

            // Обновляем информацию об учителе
            try
            {
                dbManager.UpdateStudent(selectedStudent.Id, updatedName, updatedSurname, updatedGender, updatedStudentIdNumber);
                DisplayAlert("Success", "Teacher updated successfully!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update teacher: {ex.Message}", "OK");
            }
        }

        private void OnDeleteStudentClicked(object sender, EventArgs e)
        {
            // Загружаем список учителей в Picker, если он еще не заполнен
            if (StudentPickerForDelete.ItemsSource == null)
            {
                try
                {
                    var Students = dbManager.GetAllStudent(); // Предполагаемая функция для получения всех учителей
                    StudentPickerForDelete.ItemsSource = Students.Select(t => $"{t.Id}: {t.Name} {t.Surname} {t.StudentIdNumber}").ToList();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK");
                    return;
                }
            }

            // Получаем выбранного учителя
            MD3.Student selectedStudent = StudentPickerForDelete.SelectedItem as MD3.Student;
            if (selectedStudent == null)
            {
                DisplayAlert("Validation Error", "Please select a teacher.", "OK");
                return;
            }

            // Обновляем информацию об учителе
            try
            {
                dbManager.DeleteStudent(selectedStudent.Id);
                DisplayAlert("Success", "Teacher deleted successfully!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update teacher: {ex.Message}", "OK");
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

        // Course izveidošana
        private void OnAddCourseClicked(object sender, EventArgs e)
        {
            // Получаем данные из интерфейса
            string courseName = CourseNameEntry.Text;
            MD3.Teacher selectedTeacher = CourseTeacherPicker?.SelectedItem as MD3.Teacher;


            // Проверяем, что данные заполнены корректно
            if (string.IsNullOrWhiteSpace(courseName))
            {
                DisplayAlert("Validation Error", "Please enter the course name.", "OK"); // Сообщение об ошибке
                return;
            }
            if (selectedTeacher == null)
            {
                DisplayAlert("Validation Error", "Please select a valid teacher.", "OK"); // Сообщение об ошибке
                return;
            }

            // Если все данные корректны, добавляем курс
            try
            {
                dbManager.AddCourse(courseName, selectedTeacher.Id.ToString()); // Добавляем курс с выбранным учителем
                DisplayAlert("Success", "Course added successfully!", "OK"); // Сообщение об успешной операции
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to add course: {ex.Message}", "OK"); // Сообщение об ошибке
            }
        }

        private void LoadTeachersForCourseIntoPicker()
        {
            try
            {
                var teachers = dbManager.GetAllTeachers(); // Получаем список учителей из базы данных
                CourseTeacherPicker.ItemsSource = teachers; // Устанавливаем список в качестве источника данных
                CourseTeacherPickerForUpdate.ItemsSource = teachers; // Устанавливаем список в качестве источника данных
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK"); // Уведомление об ошибке
            }
        }
        private void LoadCourseIntoPicker()
        {
            try
            {
                var courses = dbManager.GetAllCourse(); // Получаем список учителей из базы данных
                CoursePicker.ItemsSource = courses; // Устанавливаем список в качестве источника данных
                CoursePickerForDelete.ItemsSource = courses;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK"); // Уведомление об ошибке
            }
        }

        private void OnCourseSelected(object sender, EventArgs e)
        {
            // Проверяем, что выбран учитель
            if (CoursePicker.SelectedIndex == -1)
            {
                return; // Если ничего не выбрано, ничего не делаем
            }

            // Получаем выбранного учителя
            var selectedCourse = dbManager.GetAllCourse()[CoursePicker.SelectedIndex]; // Или другой метод для получения списка учителей

        }


        private void OnEditCourseClicked(object sender, EventArgs e)
        {
            // Загружаем список курсов в Picker, если он еще не заполнен
            if (CoursePicker.ItemsSource == null)
            {
                try
                {
                    var courses = dbManager.GetAllCourse(); // Предполагаемая функция для получения всех курсов
                    CoursePicker.ItemsSource = courses; // Наполняем список объектами Course
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
                    return;
                }
            }

            // Получаем выбранный курс
            MD3.Course selectedCourse = CoursePicker.SelectedItem as MD3.Course;
            if (selectedCourse == null)
            {
                DisplayAlert("Validation Error", "Please select a course.", "OK");
                return;
            }

            // Загружаем список курсов в Picker, если он еще не заполнен
            if (CourseTeacherPickerForUpdate.ItemsSource == null)
            {
                try
                {
                    var teachers = dbManager.GetAllTeachers(); // Предполагаемая функция для получения всех курсов
                    CoursePicker.ItemsSource = teachers; // Наполняем список объектами Course
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
                    return;
                }
            }
            MD3.Teacher selectedTeacher = CourseTeacherPickerForUpdate.SelectedItem as MD3.Teacher;

            // Получаем данные из интерфейса (или оставляем текущие значения)
            string updatedName = string.IsNullOrWhiteSpace(CourseNewNameEntry.Text)
                ? selectedCourse.Name
                : CourseNewNameEntry.Text;

            int updatedTeacherId = CourseTeacherPickerForUpdate.SelectedItem != null
                ? (CourseTeacherPickerForUpdate.SelectedItem as MD3.Teacher).Id
                : selectedCourse.TeacherId;

            // Проверяем данные на корректность
            if (string.IsNullOrWhiteSpace(updatedName))
            {
                DisplayAlert("Validation Error", "Course name cannot be empty.", "OK");
                return; 
            }

            // Обновляем информацию о курсе
            try
            {
                dbManager.UpdateCourse(selectedCourse.Id, updatedName, updatedTeacherId);
                DisplayAlert("Success", "Course updated successfully!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to update course: {ex.Message}", "OK");
            }
        }


        private void OnDeleteCourseClicked(object sender, EventArgs e)
        {
            // Загружаем список курсов в Picker, если он еще не заполнен
            if (CoursePickerForDelete.ItemsSource == null)
            {
                try
                {
                    var courses = dbManager.GetAllCourse(); // Предполагаемая функция для получения всех курсов
                    CoursePickerForDelete.ItemsSource = courses; // Наполняем список объектами Course
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
                    return;
                }
            }

            // Получаем выбранный курс
            MD3.Course selectedCourse = CoursePickerForDelete.SelectedItem as MD3.Course;
            if (selectedCourse == null)
            {
                DisplayAlert("Validation Error", "Please select a course to delete.", "OK");
                return;
            }

            // Удаляем курс
            try
            {
                dbManager.DeleteCourse(selectedCourse.Id); // Удаляем курс из базы данных
                DisplayAlert("Success", "Course deleted successfully!", "OK");

                // Обновляем список курсов в Picker
                var updatedCourses = dbManager.GetAllCourse();
                CoursePickerForDelete.ItemsSource = updatedCourses;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to delete course: {ex.Message}", "OK");
            }
        }


    }
}
