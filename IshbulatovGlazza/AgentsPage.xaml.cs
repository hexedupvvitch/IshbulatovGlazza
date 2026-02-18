using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IshbulatovGlazza
{
    /// <summary>
    /// Логика взаимодействия для AgentsPage.xaml
    /// </summary>
    public partial class AgentsPage : Page
    {
        public AgentsPage()
        {
            InitializeComponent();
            var currentAgents = IshbulatovEyezEntities.GetContext().Agent.ToList();
            AgentsListView.ItemsSource = currentAgents;

            ComboType.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;

            UpdateAgents();
        }
        private void UpdateAgents()
        {
            var currentAgents = IshbulatovEyezEntities.GetContext().Agent.ToList();

            if (ComboType.SelectedIndex == 1)
            currentAgents = currentAgents.Where(p => p.AgentText == "МФО").ToList();
            if (ComboType.SelectedIndex == 2)
            currentAgents = currentAgents.Where(p => p.AgentText == "ООО").ToList();
            if (ComboType.SelectedIndex == 3)
            currentAgents = currentAgents.Where(p => p.AgentText == "ЗАО").ToList();
            if (ComboType.SelectedIndex == 4)
            currentAgents = currentAgents.Where(p => p.AgentText == "МКК").ToList();
            if (ComboType.SelectedIndex == 5)
            currentAgents = currentAgents.Where(p => p.AgentText  == "ОАО").ToList();
            if (ComboType.SelectedIndex == 6)
            currentAgents = currentAgents.Where(p => p.AgentText  == "ПАО").ToList();

            string NormalniyPhone(string Phone)
            {
                return Phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "");
            }

            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || NormalniyPhone(p.Phone).Contains(NormalniyPhone(TBoxSearch.Text)) ||
           p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            AgentsListView.ItemsSource = currentAgents.ToList();

            if (ComboSort.SelectedIndex == 1)
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 2)
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 3)
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            if (ComboSort.SelectedIndex == 4)
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();



            AgentsListView.ItemsSource = currentAgents;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void AgentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
