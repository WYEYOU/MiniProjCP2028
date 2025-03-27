using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniProject.Model;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using MiniProject.Pages;

namespace MiniProject.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        // Observable properties for binding
        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string password = "";

        [RelayCommand]
        public async Task LoginAsync()
        {
            // อ่านข้อมูลจากไฟล์ JSON
            var userJson = await ReadUserJsonAsync();
            if (userJson != null)
            {
                // ตรวจสอบข้อมูลที่ผู้ใช้กรอกกับข้อมูลในไฟล์ JSON
                var user = userJson.Student;
                if (user.Email == Email && user.Password == Password)
                {
                    // ถ้าข้อมูลถูกต้อง จะสามารถเข้าสู่ระบบได้
                    // เช่น: Navigate ไปยังหน้า Home
                    await Shell.Current.DisplayAlert("Login Successful", "Welcome, " + user.Name, "OK");
                    // คุณสามารถเพิ่มโค้ดสำหรับการนำทางที่นี่
                    await Shell.Current.GoToAsync($"{nameof(HomePage)}?User={JsonConvert.SerializeObject(user)}");
                }
                else
                {
                    // ถ้าข้อมูลไม่ถูกต้อง แสดงข้อความผิดพลาด
                    await Shell.Current.DisplayAlert("Login Failed", "Invalid email or password", "OK");
                }
            }
            else
            {
                // ถ้าอ่านข้อมูลจากไฟล์ไม่ได้
                await Shell.Current.DisplayAlert("Error", "Unable to read user data", "OK");
            }
        }

        private async Task<User> ReadUserJsonAsync()
        {
            try
            {
                // เปิดไฟล์ JSON จาก Resources
                using var stream = await FileSystem.OpenAppPackageFileAsync("user.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                Debug.WriteLine(json);

                // แปลง JSON เป็น object ของ User
                var data = JsonConvert.DeserializeObject<User>(json);

                // ส่งคืนข้อมูลของ User หรือสามารถเปลี่ยนไปใช้ Student ได้
                return data ?? new User(); // ถ้าไม่พบข้อมูลจะส่งคืน object ว่าง

            }
            catch (Exception ex)
            {
                // ถ้ามีข้อผิดพลาดในการอ่านไฟล์ JSON
                Console.WriteLine("Error reading JSON: " + ex.Message);
                return new User();
            }
        }
    }
}
