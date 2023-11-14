using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Commands_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String? NomDocument { get; set; }
        bool IsDesat { get; set; }
        double MidaFont { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            IsDesat = true;
            MidaFont = 12D;
            CreaBindings();
        }
        #region Commands amb codi
        private void CreaBindings()
        {
            // Desfes
            CommandBinding desfesBinding = new CommandBinding(ApplicationCommands.Undo);
            desfesBinding.Executed += DesfesBinding_Executed;
            desfesBinding.CanExecute += DesfesBinding_CanExecute;
            this.CommandBindings.Add(desfesBinding);

            // Refes
            CommandBinding refesBinding = new CommandBinding(ApplicationCommands.Redo);
            refesBinding.Executed += RefesBinding_Executed;
            refesBinding.CanExecute += RefesBinding_CanExecute;
            this.CommandBindings.Add(refesBinding);

            // Copia
            CommandBinding copiaBinding = new CommandBinding(ApplicationCommands.Copy);
            copiaBinding.Executed += CopiaBinding_Executed;
            copiaBinding.CanExecute += CopiaBinding_CanExecute;
            this.CommandBindings.Add(copiaBinding);

            // Talla
            CommandBinding tallaBinding = new CommandBinding(ApplicationCommands.Cut);
            tallaBinding.Executed += TallaBinding_Executed;
            //tallaBinding.CanExecute += TallaBinding_CanExecute;
            this.CommandBindings.Add(tallaBinding);

            // Enganxa
            CommandBinding enganxaBinding = new CommandBinding(ApplicationCommands.Paste);
            enganxaBinding.Executed += EnganxaBinding_Executed;
            //enganxaBinding.CanExecute += EnganxaBinding_CanExecute;
            this.CommandBindings.Add(enganxaBinding);

            // Elimina contenido
            CommandBinding eliminaBinding = new CommandBinding(ApplicationCommands.Delete);
            eliminaBinding.Executed += EliminaBinding_Executed;
            eliminaBinding.CanExecute += EliminaBinding_CanExecute;
            this.CommandBindings.Add(eliminaBinding);

            // Seleccionar contenido
            CommandBinding selectBinding = new CommandBinding(ApplicationCommands.SelectAll);
            selectBinding.Executed += SelectBinding_Executed;
            selectBinding.CanExecute += SelectBinding_CanExecute;
            this.CommandBindings.Add(selectBinding);

            // ZoomIn
            CommandBinding CommandZoomOutBiding = new CommandBinding(EditorCommands.ZoomOut);
            CommandZoomOutBiding.Executed += CommandZoomOutBiding_Executed;
            this.CommandBindings.Add(CommandZoomOutBiding);
        }

        private void CommandZoomOutBiding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MidaFont -= Convert.ToInt32(e.Parameter);
            txtDocument.FontSize = MidaFont;
        }

        private void SelectBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = txtDocument?.Text.Length > 0;
        }

        private void SelectBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtDocument.SelectAll();
        }
        private void EliminaBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = txtDocument?.Text.Length > 0;
        }

        private void EliminaBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtDocument.Clear();
        }

        //private void EnganxaBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    txtDocument.
        //}

        private void EnganxaBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtDocument.Paste();
        }

        //private void TallaBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void TallaBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtDocument.Cut();
            
        }

        private void CopiaBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = txtDocument.SelectionLength > 0;  // !!!
        }

        private void CopiaBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //txtDocument.Copy();
            Title = "Has fet un COPY";
        }

        private void RefesBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = txtDocument.CanRedo;
        }

        private void RefesBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtDocument.Redo();
        }

        private void DesfesBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = txtDocument.CanUndo;
        }

        private void DesfesBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtDocument.Undo();
        }

        #endregion

        #region Codi C#
        public void DocumentNou()
        {
            txtDocument.Text = "";
            NomDocument = null;
            IsDesat = true;
        }

        public void ObreFitxer()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arxius de text (*.txt;*.csv)|*.txt;*.csv|Tots els arxius (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                txtDocument.Text = File.ReadAllText(openFileDialog.FileName);
                NomDocument = openFileDialog.FileName;
                Title = NomDocument;
                IsDesat = true;
            }
        }

        public void DesaFitxer()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Arxius de text (*.txt;*.csv)|*.txt;*.csv|Tots els arxius (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, txtDocument.Text);
                NomDocument = saveFileDialog.FileName;
                Title = NomDocument;
                IsDesat = true;
            }
        } 

        public void DesaFitxer (String? nomFitxer)
        {
            if (nomFitxer == null)
                DesaFitxer();
            else
            {
                File.WriteAllText(nomFitxer, txtDocument.Text);
                IsDesat = true;
            }
        }

        public MessageBoxResult Brut(Action accio)
        {
            MessageBoxResult resultat = MessageBoxResult.OK;

            if (!IsDesat)
            {
                MessageBoxResult dr = MessageBox.Show("Atenció", "El text actual no està desat i no es conservaran els canvis des de la darrera vegada que es va desar el document/n Vol desar els canvis abans de sortir?",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                if (dr == MessageBoxResult.Yes)
                {
                    DesaFitxer();
                    Brut(accio);
                }
                else if (dr == MessageBoxResult.No)
                {
                    accio();
                }
                resultat = dr;
            }
            else
            {
                accio();
            }
            return resultat;
        }



        /*
        public void ObreBrut()
        {
            if (!isDesat)
            {
                MessageBoxResult dr = MessageBox.Show("Atenció", "El text actual no està desat i no es conservaran els canvis des de la darrera vegada que es va desar el document/n Vol desar els canvis abans de sortir?",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                if (dr == MessageBoxResult.Yes)
                {
                    DesaFitxer();
                    ObreBrut();
                }
                else if (dr == MessageBoxResult.No)
                {
                    ObreFitxer();
                }
            }
            else
            {
                ObreFitxer();
            }
        }
        */
        private void txtDocument_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDesat = false;
        }

        private void txtDocument_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int linia = txtDocument.GetLineIndexFromCharacterIndex(txtDocument.SelectionStart);
            int columna = txtDocument.SelectionStart - txtDocument.GetCharacterIndexFromLineIndex(linia);
            tbPosicio.Text = "Ln " +
                (linia+1) +
                " Col " +
                (columna+1);
        }

       
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            if (menu.IsChecked)
                stbBarraEstat.Visibility = Visibility.Visible;
            else
                stbBarraEstat.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Commands XAML
        private void CommandDocumentNou_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtDocument.Text = "";
            NomDocument = null;
            IsDesat = true; 
        }

        private void CommandDocumentNou_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = txtDocument?.Text.Length > 0; // ejecutar cuando el texto tiene algo, nulable (?) !
            // si no hay texto se inhabilitan (El CanExecute no siemore hace falta ponerlo)
        }
        

        private void CommandObreDocument_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Brut(ObreFitxer); // pregunta si se puede con Brut y luego activa el ObreFitxer
        }

        private void CommandDesaDocument_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DesaFitxer(NomDocument); // si el nom es null llama a DesaCom...
        }

        private void CommandDesaDocumentCom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DesaFitxer();
        }

        private void CommandTanca_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CommandDesaDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsDesat;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Brut(Close); // pregunta si esta brut y luego cerrar
        }
        #endregion

        private void CommandZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MidaFont += Convert.ToInt32(e.Parameter);
            txtDocument.FontSize = MidaFont;
        }
    }
}
