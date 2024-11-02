using project; 

namespace MD2
{
    public partial class MainPage : ContentPage
    {
        private DataManager _dataManager = new DataManager();

        public MainPage()
        {
            InitializeComponent();
            LoadDataOnStartup(); 
        }

        private void OnGenerateDataClicked(object sender, EventArgs e)
        {
            _dataManager.Reset();
            _dataManager.CreateTestData(); // testa dary generacija
            OutputLabel.Text = _dataManager.Print(); 
        }

        private void OnViewDataClicked(object sender, EventArgs e)
        {
            OutputLabel.Text = _dataManager.Print(); // atminā datu izvade
        }

        private void OnLoadDataClicked(object sender, EventArgs e) // skatit datus no faila
        {
            
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формируем путь к файлу
            string filePath = Path.Combine(baseDirectory, @"..\..\..\..\..\..\MD\data.txt"); //
            _dataManager.Load(filePath); 
            OutputLabel.Text = "Dati ielādēti no faila.\n\n" + _dataManager.Print(); 
        }

        private void SaveDataInFile(object sender, EventArgs e) // saglabat datus faila
        {
          
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            
            string filePath = Path.Combine(baseDirectory, @"..\..\..\..\..\..\MD\data.txt");
            _dataManager.Save(filePath);
        }

        private void LoadDataOnStartup()
        {
            OutputLabel.Text += "\nIzveleties darbibu";
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnToggleStudentCreationClicked(object sender, EventArgs e)
        {
            StudentCreationForm.IsVisible = !StudentCreationForm.IsVisible; // Переключаем видимость формы студента
        }

        private void OnToggleAssignmentCreationClicked(object sender, EventArgs e)
        {
            AssignmentCreationForm.IsVisible = !AssignmentCreationForm.IsVisible; // Переключаем видимость формы задания
        }

        private void OnToggleSubmissionCreationClicked(object sender, EventArgs e)
        {
            SubmissionCreationForm.IsVisible = !SubmissionCreationForm.IsVisible; // Переключаем видимость формы отправки
        }



        // studenta izveidošana
        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            string name = StudentNameEntry.Text;
            string surname = StudentSurnameEntry.Text;
            Gender gender = (Gender)Enum.Parse(typeof(Gender), GenderPicker.SelectedItem.ToString());
            _dataManager.AddStudent(name, surname, gender);
            OutputLabel.Text = $"Students pievienots: {name} {surname}, Dzimums: {gender}";
        }

        // uzdevumu izveidošana
        private void OnAddAssignmentClicked(object sender, EventArgs e)
        {
            string description = AssignmentDescriptionEntry.Text;
            DateTime deadline = AssignmentDeadlinePicker.Date;
            string courseName = CourseNameEntry.Text;
            _dataManager.AddAssignment(description, deadline, courseName);
            OutputLabel.Text = $"Uzdevums pievienots: {description} ar termiņu {deadline.ToShortDateString()}";
        }

        // nodevumu izveidošana
        private void OnAddSubmissionClicked(object sender, EventArgs e)
        {
            string studentName = SubmissionStudentNameEntry.Text;
            string assignmentDescription = SubmissionAssignmentDescriptionEntry.Text;
            int score = int.Parse(SubmissionScoreEntry.Text);
            _dataManager.AddSubmission(studentName, assignmentDescription, score);
            OutputLabel.Text = $"Atdošana pievienota: Studentam {studentName} par uzdevumu '{assignmentDescription}' ar vērtējumu {score}";
        }

    }
}
