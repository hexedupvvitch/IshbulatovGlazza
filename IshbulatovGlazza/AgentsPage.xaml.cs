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
            var currentAgents = IshbulatovEyezEntities1.GetContext().Agent.ToList();
            AgentsListView.ItemsSource = currentAgents;

            ComboType.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;

            UpdateAgents();
        }
        private void UpdateAgents()
        {
            var currentAgents = IshbulatovEyezEntities1.GetContext().Agent.ToList();

            if (ComboType.SelectedIndex == 1)
            currentAgents = currentAgents.Where(p => p.AgentText == "МФО").ToList(); // ищет жесткие совпадения, конкретно МФО, то есть ничего другого он не найдет
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

            string NormalniyPhone(string Phone) //с помощью replace заменяем все специальные символы, пробелы на ничего, получается простоо убираем их
            {
                return Phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "");
            }

            //приводим почту, название к нижнему регистру и код просматривает есть ли буква или цифра в адресе почты или названии
            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || NormalniyPhone(p.Phone).Contains(NormalniyPhone(TBoxSearch.Text)) ||
           p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList(); //Tbox это само поле ввода, а text это переменная которая хранит то что написано внутри

            AgentsListView.ItemsSource = currentAgents.ToList();

            if (ComboSort.SelectedIndex == 1)
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList(); //сортировка по возрастанию
            if (ComboSort.SelectedIndex == 2)
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList(); //сортировка по убыванию
            if (ComboSort.SelectedIndex == 3)
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            if (ComboSort.SelectedIndex == 4)
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
            if (ComboSort.SelectedIndex == 5)
                currentAgents = currentAgents.OrderBy(p => p.Discount).ToList();
            if (ComboSort.SelectedIndex == 6)
                currentAgents = currentAgents.OrderByDescending(p => p.Discount).ToList();



            AgentsListView.ItemsSource = currentAgents;
            TableList = currentAgents;
            ChangePage(0, 0);
        }
        int CountRecords, CountPage, CurrentPage = 0;
        const int RecordsPage = 10;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        public void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;
            CountPage = (CountRecords + RecordsPage - 1) / RecordsPage;

            if (selectedPage.HasValue && selectedPage >= 0 && selectedPage < CountPage)
                CurrentPage = selectedPage.Value;
            else if (direction == 1 && CurrentPage > 0)
                CurrentPage--;
            else if (direction == 2 && CurrentPage < CountPage - 1)
                CurrentPage++;
            else
                return;

            int start = CurrentPage * RecordsPage;
            int end = Math.Min(start + RecordsPage, CountRecords);
            for (int i = start; i < end; i++)
                CurrentPageList.Add(TableList[i]);

            PageListBox.Items.Clear();
            for (int i = 1; i <= CountPage; i++)
                PageListBox.Items.Add(i);
            PageListBox.SelectedIndex = CurrentPage;
            AgentsListView.ItemsSource = CurrentPageList;
            AgentsListView.Items.Refresh();
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

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }
    }
}
