using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EpicList1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static string FIRST_RUN_FLAG = "FIRST_RUN_FLAG";
        public static string LEVEL_FLAG = "LEVEL_FLAG";
        public static string PROGRESS_FLAG = "PROGRESS_FLAG";
        private int nivel;
        private int progresso;
        private int nroAtividades;


        public MainPage()
        {
            this.InitializeComponent();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values[FIRST_RUN_FLAG];

            if (value == null)
            {
                // FIRST RUN
                localSettings.Values[FIRST_RUN_FLAG] = "FIRST RUN";
                localSettings.Values[LEVEL_FLAG] = 1;
                localSettings.Values[PROGRESS_FLAG] = 0;
                populateDB();
            }
            else
            {
                using (var db = new TasksContext())
                {
                    MyListView.ItemsSource = db.Tasks.ToList();
                    //  localSettings.Values[PROGRESS_FLAG] = 0;
                    progresso = (int)localSettings.Values[PROGRESS_FLAG];
                    nivel = (int)localSettings.Values[LEVEL_FLAG];
                    Nivel n = db.Niveis.FirstOrDefault(p => p.NroNivel == nivel);
                    if (n != null)
                    {
                        pbTask.Value = (progresso * 100) / n.NroAtividades;
                        nroAtividades = n.NroAtividades;
                    }
                }
            }
            using (var db = new TasksContext())
            {
                cbCategorias.Items.Add("Todas Categorias");
                foreach (Categoria c in db.Categorias.ToList())
                {
                    cbCategorias.Items.Add(c.Descricao);
                }
            }
        }

        private void populateDB()
        {
            using (var db = new TasksContext())
            {
                var categorias = new Categoria();
                categorias.Descricao = "Trabalho";
                categorias.IsRemovivel = false;
                db.Categorias.Add(categorias);

                var categorias2 = new Categoria();
                categorias2.Descricao = "Saúde";
                categorias2.IsRemovivel = false;
                db.Categorias.Add(categorias2);

                var categorias3 = new Categoria();
                categorias3.Descricao = "Compras";
                categorias3.IsRemovivel = false;
                db.Categorias.Add(categorias3);

                var nivel = new Nivel();
                nivel.NroNivel = 1;
                nivel.NroAtividades = 2;
                nivel.Texto = "Nível Básico";
                nivel.Funcionalidade = "basico";
                db.Niveis.Add(nivel);

                var nivel2 = new Nivel();
                nivel2.NroNivel = 2;
                nivel2.NroAtividades = 2;
                nivel2.Texto = "Parabéns! Você está no nível 2";
                nivel2.Funcionalidade = "Descrição";
                db.Niveis.Add(nivel2);

                var nivel3 = new Nivel();
                nivel3.NroNivel = 3;
                nivel3.NroAtividades = 3;
                nivel3.Texto = "Parabéns! Você está no nível 3";
                nivel3.Funcionalidade = "Data";
                db.Niveis.Add(nivel3);

                var nivel4 = new Nivel();
                nivel3.NroNivel = 4;
                nivel3.NroAtividades = 3;
                nivel3.Texto = "Parabéns! Você está no nível 4";
                nivel3.Funcionalidade = "Imagem";
                db.Niveis.Add(nivel4);

                var nivel5 = new Nivel();
                nivel5.NroNivel = 5;
                nivel5.NroAtividades = 3;
                nivel5.Texto = "Parabéns! Você está no nível 5";
                nivel5.Funcionalidade = "Email";
                db.Niveis.Add(nivel5);
                db.SaveChanges();

                var nivel6 = new Nivel();
                nivel6.NroNivel = 6;
                nivel6.NroAtividades = 4;
                nivel6.Texto = "Parabéns! Você está no nível 6";
                nivel6.Funcionalidade = "Camera";
                db.Niveis.Add(nivel6);
                db.SaveChanges();

            }

        }

        private void bTAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddTask));
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task item = MyListView.SelectedItem as Task;
            this.Frame.Navigate(typeof(AddTask), item);

            //Task item = MyListView.SelectedItem as Task;
            //var msgDialog = new MessageDialog("TITULO: " + item.Titulo);
            //msgDialog.Commands.Add(new UICommand("OK"));
            //msgDialog.DefaultCommandIndex = 1;
            //await msgDialog.ShowAsync();
        }

        private async void btFeito_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            try
            {
                Task task = item as Task;

                using (var db = new TasksContext())
                {
                    Categoria cat = db.Categorias.FirstOrDefault(c => c.Descricao.Equals(task.Categorias));
                    if (cat.IsRemovivel)
                        db.Categorias.Remove(cat);
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    if ((progresso + 1) >= nroAtividades)
                    {
                        //Sobe de nivel
                        if (nivel != 5)
                            nivel++;
                        localSettings.Values[LEVEL_FLAG] = nivel;
                        Nivel n = db.Niveis.FirstOrDefault(p => p.NroNivel == nivel);
                        if (n != null)
                            nroAtividades = n.NroAtividades;

                        pbTask.Value = 0;
                        progresso = 0;
                        localSettings.Values[PROGRESS_FLAG] = progresso;

                        var msgDialog = new MessageDialog(n.Texto);
                        msgDialog.Commands.Add(new UICommand("OK"));
                        msgDialog.DefaultCommandIndex = 1;
                        await msgDialog.ShowAsync();
                        MyListView.ItemsSource = db.Tasks.ToList();
                       
                    }
                    else
                    {
                        progresso++;
                        localSettings.Values[PROGRESS_FLAG] = progresso;
                        pbTask.Value = (progresso * 100) / nroAtividades;

                        var msgDialog = new MessageDialog("Tarefa realizada!");
                        msgDialog.Commands.Add(new UICommand("OK"));
                        msgDialog.DefaultCommandIndex = 1;
                        await msgDialog.ShowAsync();
                        MyListView.ItemsSource = db.Tasks.ToList();
                    }
                    cbCategorias.Items.Add("Todas Categorias");
                    foreach (Categoria c in db.Categorias.ToList())
                    {
                        cbCategorias.Items.Add(c.Descricao);
                    }


                }
            }
            catch (Exception ex)
            {
                var msgDialog = new MessageDialog("Ocorreu um erro!");
                msgDialog.Commands.Add(new UICommand("OK"));
                msgDialog.DefaultCommandIndex = 1;
                await msgDialog.ShowAsync();
            }
            //  int index = MyListView.Items.IndexOf(item);
        }

        private async void btApagar_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            try
            {
                Task task = item as Task;

                using (var db = new TasksContext())
                {
                    Categoria cat = db.Categorias.FirstOrDefault(c => c.Descricao.Equals(task.Categorias));
                    if (cat.IsRemovivel)
                        db.Categorias.Remove(cat);
                    db.Tasks.Remove(task);
                    db.SaveChanges();

                    var msgDialog = new MessageDialog("Tarefa excluida!");
                    msgDialog.Commands.Add(new UICommand("OK"));
                    msgDialog.DefaultCommandIndex = 1;
                    await msgDialog.ShowAsync();
                    MyListView.ItemsSource = db.Tasks.ToList();
                    cbCategorias.Items.Add("Todas Categorias");
                    foreach (Categoria c in db.Categorias.ToList())
                    {
                        cbCategorias.Items.Add(c.Descricao);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgDialog = new MessageDialog("Ocorreu um erro!");
                msgDialog.Commands.Add(new UICommand("OK"));
                msgDialog.DefaultCommandIndex = 1;
                await msgDialog.ShowAsync();
            }
        }

        private void cbCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selecionado = (sender as ComboBox).SelectedItem as string;
            int intSelecionado = (sender as ComboBox).SelectedIndex;
            if (intSelecionado == 0)
            {
                using (var db = new TasksContext())
                {
                    MyListView.ItemsSource = db.Tasks.ToList();
                }
            }
            else
            {
                using (var db = new TasksContext())
                {
                    MyListView.ItemsSource = db.Tasks.ToList().Where(t => t.Categorias.Contains(selecionado));
                }
            }
        }
    }
}
