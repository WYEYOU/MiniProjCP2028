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
    public partial class RegViewModel : ObservableObject
    {
        [ObservableProperty]
        private User _user = new User();

        [ObservableProperty]
        private ObservableCollection<Course> _term1 = new ObservableCollection<Course>();
        
        [ObservableProperty]
        private ObservableCollection<Course> _term2 = new ObservableCollection<Course>();
        
        [ObservableProperty]
        private ObservableCollection<Course> _term3 = new ObservableCollection<Course>();

        // Adding the WithdrawCommand property
        public ICommand WithdrawCommand { get; }

        public RegViewModel()
        {
            WithdrawCommand = new Command<Course>(async (course) => await WithdrawCourse(course));
        }

        public async Task LoadUserAsync()
        {
            User = await ReadUserJsonAsync();

            if (User?.Registration?.Terms != null)
            {
                Term1.Clear();
                Term2.Clear();
                Term3.Clear();
                
                foreach (var course in User.Registration.Terms.FirstOrDefault(t => t.TermTerm == 1)?.Courses ?? new List<Course>())
                    Term1.Add(course);
                
                foreach (var course in User.Registration.Terms.FirstOrDefault(t => t.TermTerm == 2)?.Courses ?? new List<Course>())
                    Term2.Add(course);
                
                foreach (var course in User.Registration.Terms.FirstOrDefault(t => t.TermTerm == 3)?.Courses ?? new List<Course>())
                    Term3.Add(course);

                Debug.WriteLine($"Term1 Count: {Term1.Count}, Term2 Count: {Term2.Count}, Term3 Count: {Term3.Count}");
            }
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

        // Withdraw course method
        private async Task WithdrawCourse(Course course)
        {
            if (course == null)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "ยืนยันการถอน",
                $"คุณต้องการถอน {course.CourseName} หรือไม่?",
                "โอเค", "ยกเลิก");

            if (confirm)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "สำเร็จ",
                    $"ถอน {course.CourseName} สำเร็จ",
                    "ตกลง");
            }
        }
    }
}
