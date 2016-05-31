using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EpicList1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTask : Page
    {
        private int nivel;
        private Task task;
        private bool isUpdate;
        private List<string> listCategorias;
        public AddTask()
        {
            this.InitializeComponent();
            isUpdate = false;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values[MainPage.LEVEL_FLAG];
            if (value != null)
            {
                nivel = (int)value;
            }
            else
            {
                nivel = 1;
            }
            switch (nivel)
            {
                case 1:
                    txtDescricao.Visibility = Visibility.Collapsed;
                    txtUrl.Visibility = Visibility.Collapsed;
                    txtEmail.Visibility = Visibility.Collapsed;
                    dtData.Visibility = Visibility.Collapsed;
                    btCompartilhar.Visibility = Visibility.Collapsed;
                    btFoto.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    txtDescricao.Visibility = Visibility.Visible;
                    txtUrl.Visibility = Visibility.Collapsed;
                    txtEmail.Visibility = Visibility.Collapsed;
                    dtData.Visibility = Visibility.Collapsed;
                    btCompartilhar.Visibility = Visibility.Collapsed;
                    btFoto.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    txtDescricao.Visibility = Visibility.Visible;
                    txtUrl.Visibility = Visibility.Collapsed;
                    txtEmail.Visibility = Visibility.Collapsed;
                    dtData.Visibility = Visibility.Visible;
                    btCompartilhar.Visibility = Visibility.Collapsed;
                    btFoto.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    txtDescricao.Visibility = Visibility.Visible;
                    txtUrl.Visibility = Visibility.Visible;
                    txtEmail.Visibility = Visibility.Collapsed;
                    dtData.Visibility = Visibility.Visible;
                    btCompartilhar.Visibility = Visibility.Collapsed;
                    btFoto.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    txtDescricao.Visibility = Visibility.Visible;
                    txtUrl.Visibility = Visibility.Visible;
                    txtEmail.Visibility = Visibility.Visible;
                    dtData.Visibility = Visibility.Visible;
                    btCompartilhar.Visibility = Visibility.Visible;
                    btFoto.Visibility = Visibility.Collapsed;
                    break;
                case 6:
                    txtDescricao.Visibility = Visibility.Visible;
                    txtUrl.Visibility = Visibility.Visible;
                    txtEmail.Visibility = Visibility.Visible;
                    dtData.Visibility = Visibility.Visible;
                    btCompartilhar.Visibility = Visibility.Visible;
                    btFoto.Visibility = Visibility.Visible;
                    break;
                default:
                    txtDescricao.Visibility = Visibility.Visible;
                    txtUrl.Visibility = Visibility.Visible;
                    txtEmail.Visibility = Visibility.Visible;
                    dtData.Visibility = Visibility.Visible;
                    btCompartilhar.Visibility = Visibility.Visible;
                    break;

            }

            using (var db = new TasksContext())
            {
                listCategorias = new List<string>();
                foreach (Categoria c in db.Categorias.ToList())
                {
                    listCategorias.Add(c.Descricao);
                }

                boxCategoria.ItemsSource = listCategorias;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                task = (Task)e.Parameter;
                using (var db = new TasksContext())
                {
                    task = db.Tasks.FirstOrDefault(t => t.TaskId.Equals(task.TaskId));
                }
                txtTitulo.Text = task.Titulo;
                if (task.Descricao != null)
                    txtDescricao.Text = task.Descricao;
                if (task.Categorias != null)
                    boxCategoria.Text = task.Categorias;
                if (task.Imagem != null)
                    txtUrl.Text = task.Imagem;
                if (task.Data != null)
                    dtData.Date = task.Data;
                isUpdate = true;
            }
        }
        private async void btSalvar_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new TasksContext())
            {
                if (String.IsNullOrEmpty(txtTitulo.Text))
                {
                    var msgDialog = new MessageDialog("Insira pelo menos o título!", "Atenção");
                    msgDialog.Commands.Add(new UICommand("OK"));
                    msgDialog.DefaultCommandIndex = 1;
                    await msgDialog.ShowAsync();
                    return;
                }
                if (task == null)
                    task = new Task();
                if (!String.IsNullOrEmpty(boxCategoria.Text))
                {
                    Categoria categ = db.Categorias.FirstOrDefault(c => c.Descricao.ToLower().Equals(boxCategoria.Text.ToLower()));
                    if (categ == null)
                    {
                        categ = new Categoria();
                        categ.Descricao = boxCategoria.Text;
                        categ.IsRemovivel = true;
                        db.Categorias.Add(categ);
                        db.SaveChanges();
                    }
                    task.Categorias = boxCategoria.Text;
                }
                task.Titulo = txtTitulo.Text;
                if (nivel > 1)
                    task.Descricao = txtDescricao.Text;
                if (nivel > 2)
                    task.Data = dtData.Date.DateTime;
                if (nivel > 3)
                    task.ImagemURL = txtUrl.Text;
                //  sendEmail("larissag@dbserver.com.br", task);
                if (nivel > 4 && !String.IsNullOrEmpty(txtEmail.Text))
                    sendEmail(txtEmail.Text);
             //   if (nivel > 5)
                 //   task.Imagem = imgCamera.Source.ToString();
                if (isUpdate)
                    db.Tasks.Update(task);
                else
                    db.Tasks.Add(task);
                db.SaveChanges();

                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void btVoltar_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }



        private async void sendEmail(String email)
        {
            EmailRecipient sendTo = new EmailRecipient();
            sendTo.Address = email;
            sendTo.Name = email;

            // Create email object
            EmailMessage mail = new EmailMessage();
            mail.Subject = "Tarefa:  " + txtTitulo.Text;
            mail.Body = "Título: " + txtTitulo.Text + "\nDescrição: " + txtDescricao.Text + "\nData: " + dtData.ToString();

            // Add recipients to the mail object
            mail.To.Add(sendTo);
            //mail.Bcc.Add(sendTo);
            //mail.CC.Add(sendTo);

            // Open the share contract with Mail only:
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private void btCompartilhar_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtEmail.Text))
                sendEmail(txtEmail.Text);
        }

        private async void btFoto_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }
            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
        BitmapPixelFormat.Bgra8,
        BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            imgCamera.Source = bitmapSource;
        }
    }
}
