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

        private void LoadDataOnStartup()
        {
            
            OutputLabel.Text += "";
        }

        private void OnToggleStudentCreationClicked(object sender, EventArgs e)
        {
            StudentCreationForm.IsVisible = !StudentCreationForm.IsVisible; // parsledz studenta pievienošanas formas redzamibu
        }

        private void OnToggleAssignmentCreationClicked(object sender, EventArgs e)
        {
            AssignmentCreationForm.IsVisible = !AssignmentCreationForm.IsVisible; 
        }

        private void OnToggleSubmissionCreationClicked(object sender, EventArgs e)
        {
            SubmissionCreationForm.IsVisible = !SubmissionCreationForm.IsVisible; 
        }
        private void OnToggleAssignmentFormClicked(object sender, EventArgs e)
        {
            AssignmentForm.IsVisible = !AssignmentForm.IsVisible;
        }

        private void OnToggleSubmissionFormClicked(object sender, EventArgs e)
        {
            SubmissionForm.IsVisible = !SubmissionForm.IsVisible;
        }

        // studenta izveidošana
        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            string name = StudentNameEntry.Text;
            string surname = StudentSurnameEntry.Text;
            string studentIdNumber = StudentIdNumberEntry.Text;
            Gender gender = (Gender)Enum.Parse(typeof(Gender), GenderPicker.SelectedItem.ToString());
            _dataManager.AddStudent(name, surname, studentIdNumber, gender);
            OutputLabel.Text = $"Students pievienots: {name} {surname}, Dzimums: {gender}, Student ID: {studentIdNumber}";
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

        // Assignmet update
        private void OnUpdateAssignmentClicked(object sender, EventArgs e)
        {
            // Jaunu datu ievade
            string currentDescription = CurrentAssignmentDescription.Text;
            string newDescription = NewAssignmentDescription.Text;
            DateTime newDeadline = DateTime.Parse(NewAssignmentDeadline.Text);
            string newCourseName = NewCourseName.Text;

            _dataManager.UpdateAssignment(currentDescription, newDescription, newDeadline, newCourseName);
            OutputLabel.Text = $"Uzdevuma dati izlaboti";
        }

        // Submision update
        private void OnUpdateSubmissionClicked(object sender, EventArgs e)
        {
            // Jaunu datu ievade
            string studentName = SubmissionStudentName.Text;
            string assignmentDescription = SubmissionAssignmentDescription.Text;
            int score = int.Parse(NewSubmissionScore.Text);

            _dataManager.UpdateSubmission(studentName, assignmentDescription, score);
            OutputLabel.Text = $"Nodevuma dati izlaboti";
        }

        //  Assignment dzešanas metode
        private void OnDeleteAssignmentClicked(object sender, EventArgs e)
        {
            string description = AssignmentDescriptionEnter.Text; 

            if (!string.IsNullOrWhiteSpace(description)) // paziņojumu izvade
            {
                _dataManager.DeleteAssignment(description);
                DisplayAlert("Success", $"Assignment '{description}' has been deleted.", "OK");
            }
            else
            {
                DisplayAlert("Error", "Please enter a valid assignment description.", "OK");
            }
        }

        // Submission dzešanas metode
        private void OnDeleteSubmissionClicked(object sender, EventArgs e)
        {
            string studentName = SubmissionStudentNameEnter.Text;
            string studentSurname = SubmissionStudentSurnameEnter.Text;
            string assignmentDescription = SubmissionAssignmentDescriptionEnter.Text;

            if (!string.IsNullOrWhiteSpace(studentName) && !string.IsNullOrWhiteSpace(studentSurname) && !string.IsNullOrWhiteSpace(assignmentDescription)) // parbaudam vai vissi lauki ir aizpulditi
            {
                _dataManager.DeleteSubmission(studentName, studentSurname, assignmentDescription);
                DisplayAlert("Success", $"Submission for '{assignmentDescription}' by '{studentName}' has been deleted.", "OK");
            }
            else
            {
                DisplayAlert("Error", "Please enter  student name, surname and assignment description.", "OK");
            }
        }
    }
}
