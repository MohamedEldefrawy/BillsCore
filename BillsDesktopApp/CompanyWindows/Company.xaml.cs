using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BillsDesktopApp.CompanyWindows
{
    /// <summary>
    /// Interaction logic for Company.xaml
    /// </summary>
    public partial class Company : Window
    {
        private readonly BillsContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<Companies> CompaniesService;
        private readonly string[] AllcolNames = typeof(Companies).GetProperties().Select(p => p.Name).ToArray();
        private readonly List<string> selectedColNames = new List<string>();
        public static ObservableCollection<Companies> CompaniesObservalbleCollection = new ObservableCollection<Companies>();

        public Company(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            CompaniesService = unitOfWork.Repository<Companies>();
            InitializeComponent();
        }

    }
}
