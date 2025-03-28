using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniProject.Model;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MiniProject.ViewModel
{
    public partial class RegSearchViewModel : ObservableObject
    {
        [ObservableProperty]
        private User _user = new User();

        [ObservableProperty]
        private ObservableCollection<Course> _subjects = new ObservableCollection<Course>();

        private ObservableCollection<Course> _allSubjects = new ObservableCollection<Course>();

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterSubjects();
            }
        }

        public ICommand RegisterCourseCommand { get; }

        public RegSearchViewModel()
        {
            RegisterCourseCommand = new AsyncRelayCommand<Course?>(RegisterCourse);
            Task.Run(async () => await LoadUserAsync());
        }

        public async Task LoadUserAsync()
        {
            User = await ReadUserJsonAsync();

            _allSubjects.Clear();
            Subjects.Clear();

            if (User?.Subject != null)
            {
                foreach (var subject in User.Subject)
                {
                    var course = new Course
                    {
                        CourseId = subject.CourseId,
                        CourseName = subject.CourseName,
                        Credits = subject.Credits,
                        Status = "Available"
                    };

                    _allSubjects.Add(course);
                    Subjects.Add(course);
                }
            }

            Debug.WriteLine($"Subjects Loaded: {Subjects.Count}");
        }

        private async Task<User> ReadUserJsonAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("user.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                Debug.WriteLine(json);

                return JsonConvert.DeserializeObject<User>(json) ?? new User();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error reading JSON: " + ex.Message);
                return new User();
            }
        }

        private async Task RegisterCourse(Course? course)
        {
            if (course == null) return;

            bool confirm = await Shell.Current.CurrentPage.DisplayAlert(
                "ยืนยันการลงทะเบียน",
                $"คุณต้องการลงทะเบียนวิชา {course.CourseName} หรือไม่?",
                "โอเค", "ยกเลิก");

            if (confirm)
            {
                course.Status = "Registered";
                await Shell.Current.CurrentPage.DisplayAlert("สำเร็จ", $"ลงทะเบียนวิชา {course.CourseName} สำเร็จ", "ตกลง");
            }
        }

        private void FilterSubjects()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Subjects = new ObservableCollection<Course>(_allSubjects);
            }
            else
            {
                var filtered = _allSubjects
                    .Where(s => s.CourseName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                s.CourseId.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Subjects = new ObservableCollection<Course>(filtered);
            }
        }
    }
}
