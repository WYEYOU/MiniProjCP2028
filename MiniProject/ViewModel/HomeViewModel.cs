using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using MiniProject.Model;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MiniProject.Pages;

namespace MiniProject.ViewModel;

public partial class HomeViewModel : ObservableObject
{
    private User _user = new User();
    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    public async Task LoadUserAsync()
    {
        User = await ReadUserJsonAsync();
    }

    private async Task<User> ReadUserJsonAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("user.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            Debug.WriteLine(json);

            var data = JsonConvert.DeserializeObject<User>(json);
            return data ?? new User();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading JSON: " + ex.Message);
            return new User();
        }
    }

    private readonly INavigation _navigation;

    public HomeViewModel(INavigation navigation)
    {
        _navigation = navigation;
        NavigateToRegisterCommand = new Command(async () => await NavigateToRegisterPage());
        NavigateToRegisterSearchCommand = new Command(async () => await NavigateToRegisterSearchPage());
        LogoutCommand = new Command(async () => await Logout());
    }

    public ICommand NavigateToRegisterCommand { get; }
    public ICommand NavigateToRegisterSearchCommand { get; }
    public ICommand LogoutCommand { get; }

    private async Task NavigateToRegisterPage()
    {
        await _navigation.PushAsync(new RegisterPage());
    }

    private async Task NavigateToRegisterSearchPage()
    {
        await _navigation.PushAsync(new RegisterSearchPage());
    }

    private async Task Logout()
    {
        // สร้างหน้า LoginPage ขึ้นมา
        var loginPage = new LoginPage();

        // นำหน้า LoginPage มาแทนที่ HomePage
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
