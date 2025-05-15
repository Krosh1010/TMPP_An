using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using App.Services;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IEmployeeServices _service;

    public MainWindowViewModel(IEmployeeServices service)
    {
        _service = service;
    }

    public void AddHR(string name, string role, string team)
    {
        _service.AddEmployee(name, role, team);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}