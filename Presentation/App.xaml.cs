﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NNTP_NEWS_CLIENT.Presentation.ViewModel;

namespace NNTP_NEWS_CLIENT.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application, INotifyPropertyChanged
{

    private ViewModelBase? _currentViewModel;
    public ViewModelBase? CurrentViewModel
    { 
        get => _currentViewModel; 
        set => SetField(ref _currentViewModel, value); 
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}