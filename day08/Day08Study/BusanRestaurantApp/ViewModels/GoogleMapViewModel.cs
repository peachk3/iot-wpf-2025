using BusanRestaurantApp.Helpers;
using BusanRestaurantApp.Models;
using CefSharp.DevTools.CSS;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusanRestaurantApp.ViewModels
{
    public class GoogleMapViewModel :ObservableObject
    {
        private BusanItem _selectedMatjibItem;
        private string _matjibLocatioin;

        public BusanItem SelectedMatjibItem
        {
            get => _selectedMatjibItem;
            set { 
                SetProperty(ref _selectedMatjibItem, value);
                // 위도(Latitude/Lat), 경도(Longitude/Lng)
                MatjibLocation = $"https://google.com/maps/place/{SelectedMatjibItem.Lat},{SelectedMatjibItem.Lng}";
            }
        }

        public string MatjibLocation
        {
            get => _matjibLocatioin;
            set => SetProperty(ref _matjibLocatioin, value);
        }

        public GoogleMapViewModel()
        {
            MatjibLocation = "";
        }
    } 
}
